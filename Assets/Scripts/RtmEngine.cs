using UnityEngine;
using agora_rtm;
using System;
using UnityEngine.UI;

public class RtmEngine : MonoBehaviour
{
    public const string ADD_CHANNEL_COMMAND = "ADD-";
    public const string DELETE_CHANNEL_COMMAND = "DEL-";

#pragma warning disable 649
    [SerializeField] private UIManager uiManager;
    [SerializeField] private string appID;
    [SerializeField] private string userName;
#pragma warning restore 649

    [SerializeField] private InputField userNameInputField;
    [SerializeField] private Button joinTreefortButton;
    [SerializeField] private GameObject RTMPanel;
    [SerializeField] private GameObject ChannelPanel;
    [SerializeField] private AgoraEngine agoraEngine;

    private RtmClient rtmClient = null;
    private RtmChannel rtmChannel;
    private RtmClientEventHandler clientEventHandler;
    private RtmChannelEventHandler channelEventHandler;

    void Start()
    {
        HelperTools.AssignedInEditorCheck(uiManager);
        HelperTools.AssignedInEditorCheck(appID);
        HelperTools.AssignedInEditorCheck(userName);
        HelperTools.AssignedInEditorCheck(userNameInputField);

        ChannelPanel.SetActive(false);

        userNameInputField.onValueChanged.AddListener(delegate { CheckForInput(); });
        joinTreefortButton.interactable = false;
        // RTM client callbacks
        
    }

    // Agora Essentials -------------- //
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
    // -------------------------------- //

    private void CheckForInput()
    {
        if(userNameInputField.text == "")
        {
            joinTreefortButton.interactable = false;
        }
        else
        {
            joinTreefortButton.interactable = true;
        }
    }

    public void Button_JoinTreefort()
    {
        // Disable the Fort Joining Panel
        RTMPanel.SetActive(false);
        ChannelPanel.SetActive(true);
        Login();
    }

    public void Login()
    {
        clientEventHandler = new RtmClientEventHandler();
        channelEventHandler = new RtmChannelEventHandler();

        clientEventHandler.OnLoginSuccess = OnClientLoginSuccessHandler;
        clientEventHandler.OnLoginFailure = OnClientLoginFailureHandler;

        // RTM channel-wide callbacks
        channelEventHandler.OnMessageReceived = OnChannelMessageReceivedHandler;
        channelEventHandler.OnSendMessageResult = OnSendMessageResultHandler;
        channelEventHandler.OnJoinSuccess = OnJoinSuccessHandler;

        rtmClient = new RtmClient(appID, clientEventHandler);
        rtmClient.Login("", userName);
    }

    public void Button_Logout()
    {
        if (rtmChannel != null)
        {
            rtmChannel.Dispose();
            rtmChannel = null;
        }

        if (rtmClient != null)
        {
            rtmClient.Dispose();
            rtmClient = null;
        }

        ChannelPanel.SetActive(false);
        RTMPanel.SetActive(true);
    }

    void OnClientLoginSuccessHandler(int id)
    {
        string msg = "client login successful! id = " + id;
        Debug.Log(msg);

        // Agora Essentials ---------------------------------------------------------- //
        // Make sure to Join the RTM channel in the callback of a successful RTM login //
        JoinChannel();
        // --------------------------------------------------------------------------- //
    }

    void OnClientLoginFailureHandler(int id, LOGIN_ERR_CODE errorCode)
    {
        string msg = "client login unsuccessful! id = " + id + " errorCode = " + errorCode;
        Debug.Log(msg);
    }

    public void JoinChannel()
    {
        rtmChannel = rtmClient?.CreateChannel("NETWORK", channelEventHandler);
        rtmChannel.Join();
    }

    void OnJoinSuccessHandler(int id)
    {
        agoraEngine.JoinLobby();
    }

    public void SendRTMChannelMessage(string message)
    {
        rtmChannel?.SendMessage(rtmClient.CreateMessage(message));

        Debug.Log("Attempting to send RTM channel message: " + message);
    }

    void OnSendMessageResultHandler(int id, Int64 messageId, CHANNEL_MESSAGE_ERR_CODE errorCode)
    {
        Debug.Log("Message: " + id + " " + messageId + " sent status: " + errorCode);
    }

    void OnChannelMessageReceivedHandler(int id, string userId, TextMessage message)
    {
        Debug.Log("client OnChannelMessageReceived id = " + id + ", from user:" + userId + " text:" + message.GetText());

        string messageString = message.GetText();
        if (messageString.Contains(ADD_CHANNEL_COMMAND))
        {
            uiManager.AddChannelToDropDownList(messageString.Substring(4));
        }
        else if (messageString.Contains(DELETE_CHANNEL_COMMAND))
        {
            uiManager.RemoveChannelFromDropDownList(messageString.Substring(4));
        }
    }
}