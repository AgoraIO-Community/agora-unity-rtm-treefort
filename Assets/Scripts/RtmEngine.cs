using UnityEngine;
using agora_rtm;
using System;

public class RtmEngine : MonoBehaviour
{
    public const string ADD_CHANNEL_COMMAND = "ADD-";
    public const string DELETE_CHANNEL_COMMAND = "DEL-";

#pragma warning disable 649
    [SerializeField] private UIManager uiManager;
    [SerializeField] private string appID;
    [SerializeField] private string userName;
#pragma warning restore 649

    private RtmClient rtmClient = null;
    private RtmChannel rtmChannel;
    private RtmClientEventHandler clientEventHandler;
    private RtmChannelEventHandler channelEventHandler;

    void Start()
    {
        HelperTools.AssignedInEditorCheck(uiManager);
        HelperTools.AssignedInEditorCheck(appID);
        HelperTools.AssignedInEditorCheck(userName);

        clientEventHandler = new RtmClientEventHandler();
        channelEventHandler = new RtmChannelEventHandler();

        rtmClient = new RtmClient(appID, clientEventHandler);

        // RTM client callbacks
        clientEventHandler.OnLoginSuccess = OnClientLoginSuccessHandler;
        clientEventHandler.OnLoginFailure = OnClientLoginFailureHandler;

        // RTM channel-wide callbacks
        channelEventHandler.OnMessageReceived = OnChannelMessageReceivedHandler;
        channelEventHandler.OnSendMessageResult = OnSendMessageResultHandler;

        Login();
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

    public void Login()
    {
        rtmClient.Login("", userName);
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