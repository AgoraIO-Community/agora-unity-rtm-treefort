using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.UI;

public class AgoraEngine : MonoBehaviour
{
    [SerializeField] private string appID = "8ac5b43a061d49d6a57360ce4ae6e92b";
    private IRtcEngine mRtcEngine;

    [SerializeField] private List<GameObject> playerVideoList;

// disable variable warnings
#pragma warning disable 649
    [SerializeField] private List<Transform> spawnPointLocations;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private RtmEngine rtmEngine;
#pragma warning restore 649

    [SerializeField] private string currentChannel;

    void Start()
    {
        playerVideoList = new List<GameObject>();

        if(mRtcEngine != null)
        {
            IRtcEngine.Destroy();
        }

        HelperTools.AssignedInEditorCheck(uiManager);
        HelperTools.AssignedInEditorCheck(rtmEngine);

        // Agora Essentials --------------------------- //
        mRtcEngine = IRtcEngine.GetEngine(appID);

        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccessHandler;
        mRtcEngine.OnUserJoined = OnUserJoinedHandler;
        mRtcEngine.OnLeaveChannel = OnLeaveChannelHandler;
        mRtcEngine.OnUserOffline = OnUserOfflineHandler;

        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();

        mRtcEngine.JoinChannel("LOBBY", null, 0);
        // -------------------------------------------- //
    }

    // Button assigned in Editor
    public void Button_JoinButtonPressed()
    {
        mRtcEngine.LeaveChannel();
        mRtcEngine.JoinChannel(uiManager.GetCurrentChannelSelection(), null, 0);
    }

    // Local client joins channel
    private void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        Debug.Log("Local user: " + uid + " joined channel: " + channelName);

        CreateUserVideoSurface(uid, true);
        uiManager.AddChannelToDropDownList(channelName);
        uiManager.UpdateChannelNameText(channelName);
        currentChannel = channelName;

        if (channelName.ToUpper() != UIManager.DEMO_LOBBY || channelName.ToUpper() != UIManager.NEW_ROOM_CREATOR)
        {
            // send message to all players in "RTM network"
            rtmEngine.SendRTMChannelMessage(RtmEngine.ADD_CHANNEL_COMMAND + channelName);
        }
    }

    // Remote Client Joins Channel.
    private void OnUserJoinedHandler(uint uid, int elapsed)
    {
        Debug.Log("Remote user joined channel:" + uid);

        CreateUserVideoSurface(uid, false);
    }

    // Local user leaves channel.
    private void OnLeaveChannelHandler(RtcStats stats)
    {
        Debug.Log("Local user left channel");

        foreach (GameObject player in playerVideoList)
        {
            Destroy(player.gameObject);
        }
        playerVideoList.Clear();

        if(stats.userCount == 1)
        {
            uiManager.RemoveChannelFromDropDownList(currentChannel);

            // send message to all players in "RTM network"
            rtmEngine.SendRTMChannelMessage(RtmEngine.DELETE_CHANNEL_COMMAND + currentChannel);
        }
    }

    // Remote User Leaves the Channel.
    private void OnUserOfflineHandler(uint uid, USER_OFFLINE_REASON reason)
    {
        Debug.Log("Remote user left: " + uid + " for reason: " + reason);

        RemoveUserVideoSurface(uid);
    }

    // Create new image plane to display users in party.
    private void CreateUserVideoSurface(uint uid, bool isLocalUser)
    {
        // Avoid duplicating Local player VideoSurface image plane.
        for (int i = 0; i < playerVideoList.Count; i++)
        {
            if (playerVideoList[i].name == uid.ToString())
            {
                return;
            }
        }

        // Create Gameobject that will serve as our VideoSurface.
        GameObject newUserVideo = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (newUserVideo == null)
        {
            Debug.LogError("CreateUserVideoSurface() - newUserVideoIsNull");
            return;
        }

        newUserVideo.transform.localScale = Vector3.one * 3;
        newUserVideo.name = uid.ToString();
        playerVideoList.Add(newUserVideo);

        // this is because we are using 4 spawn points per room
        if(playerVideoList.Count < 5)
        {
            newUserVideo.transform.position = spawnPointLocations[playerVideoList.Count - 1].position;
        }

        // Update our VideoSurface to reflect new users
        VideoSurface newVideoSurface = newUserVideo.AddComponent<VideoSurface>();
        if (newVideoSurface == null)
        {
            Debug.LogError("CreateUserVideoSurface() - VideoSurface component is null on newly joined user");
            return;
        }

        if (isLocalUser == false)
        {
            newVideoSurface.SetForUser(uid);
        }
        newVideoSurface.SetGameFps(30);
    }

    private void RemoveUserVideoSurface(uint deletedUID)
    {
        foreach (GameObject player in playerVideoList)
        {
            if (player.name == deletedUID.ToString())
            {
                playerVideoList.Remove(player);
                Destroy(player.gameObject);
                break;
            }
        }
    }

    // Agora Essentials -------------- //
    private void OnApplicationQuit()
    {
        IRtcEngine.Destroy();
        mRtcEngine = null;
    }
}