using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_rtm;
using UnityEngine.UI;
using System;

public class RtmEngine : MonoBehaviour
{
    public InputField channelNameField;
    public Text channelText;
    public List<string> channelNames;
    public GameObject buttonPrefab;
    public Transform scrollViewContent;
    public RectTransform panelContentWindow;
    public float scrollViewOffset = 60f;
    public float buttonSpacing = 20f;

    public const string ADD_CHANNEL_COMMAND = "ADD-";
    public const string DELETE_CHANNEL_COMMAND = "DEL-";

    public UIManager uiManager;

    private string appID = "8ac5b43a061d49d6a57360ce4ae6e92b";
    public string userName = "";

    private RtmClient rtmClient = null;
    private RtmChannel rtmChannel;
    private RtmCallManager rtmCallManager;

    private RtmClientEventHandler clientEventHandler;
    private RtmChannelEventHandler channelEventHandler;
    private RtmCallEventHandler callEventHandler;


    // Start is called before the first frame update
    void Start()
    {
        channelNames = new List<string>();

        clientEventHandler = new RtmClientEventHandler();
        channelEventHandler = new RtmChannelEventHandler();
        callEventHandler = new RtmCallEventHandler();

        rtmClient = new RtmClient(appID, clientEventHandler);

        clientEventHandler.OnLoginSuccess = OnClientLoginSuccessHandler;
        clientEventHandler.OnLoginFailure = OnClientLoginFailureHandler;

        channelEventHandler.OnJoinSuccess = OnJoinSuccessHandler;
        channelEventHandler.OnJoinFailure = OnJoinFailureHandler;
        channelEventHandler.OnLeave = OnLeaveHandler;
        channelEventHandler.OnMessageReceived = OnChannelMessageReceivedHandler;
        channelEventHandler.OnSendMessageResult = OnSendMessageResultHandler;
        channelEventHandler.OnMemberJoined = OnMemberJoinedHandler;
        channelEventHandler.OnMemberLeft = OnMemberLeftHandler;

        Login();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            rtmChannel?.SendMessage(rtmClient.CreateMessage("ADD-"+uiManager.GetCurrentChannelSelection()));
        }
    }

    private void OnApplicationQuit()
    {
        if(rtmChannel != null)
        {
            rtmChannel.Dispose();
            rtmChannel = null;
        }

        if(rtmClient != null)
        {
            rtmClient.Dispose();
            rtmClient = null;
        }
    }

    public void Login()
    {
        rtmClient.Login("", userName);
    }

    public void JoinChannel()
    {
        rtmChannel = rtmClient.CreateChannel("LOBBY", channelEventHandler);
        rtmChannel.Join();
    }

    void OnClientLoginSuccessHandler(int id)
    {
        string msg = "client login successful! id = " + id;
        Debug.Log(msg);

        JoinChannel();
    }

    void OnClientLoginFailureHandler(int id, LOGIN_ERR_CODE errorCode)
    {
        string msg = "client login unsuccessful! id = " + id + " errorCode = " + errorCode;
        Debug.Log(msg);
    }

    void OnJoinSuccessHandler(int id)
    {
        string msg = "OnJoinSuccess id = " + id;
        Debug.Log(msg);
    }

    void OnJoinFailureHandler(int id, JOIN_CHANNEL_ERR errorCode)
    {
        string msg = "channel OnJoinFailure  id = " + id + " errorCode = " + errorCode;
        Debug.Log(msg);
    }

    void OnLeaveHandler(int id, LEAVE_CHANNEL_ERR errorCode)
    {
        string msg = "client onleave id = " + id + " errorCode = " + errorCode;
        Debug.Log(msg);
    }

    public void SendRTMChannelMessage(string message)
    {
        rtmChannel?.SendMessage(rtmClient.CreateMessage(message));
    }

    void OnChannelMessageReceivedHandler(int id, string userId, TextMessage message)
    {
        Debug.Log("client OnChannelMessageReceived id = " + id + ", from user:" + userId + " text:" + message.GetText());

        string messageString = message.GetText();
        if(messageString.Contains("ADD-"))
        {
            uiManager.AddChannelToDropDownList(messageString.Substring(4));
        }
        else if(messageString.Contains("DEL-"))
        {
            uiManager.RemoveChannelFromDropDownList(messageString.Substring(4)); 
        }
    }

    void OnSendMessageResultHandler(int id, Int64 messageId, CHANNEL_MESSAGE_ERR_CODE errorCode)
    {
        Debug.Log("Message: " + id + " " + messageId + " sent status: " + errorCode);
    }

    void OnMemberJoinedHandler(int id, RtmChannelMember member)
    {
        string msg = "channel OnMemberJoinedHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
        Debug.Log(msg);
    }

    void OnMemberLeftHandler(int id, RtmChannelMember member)
    {
        string msg = "channel OnMemberLeftHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
        Debug.Log(msg);
    }
}
