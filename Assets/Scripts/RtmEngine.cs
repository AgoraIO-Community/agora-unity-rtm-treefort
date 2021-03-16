﻿using System.Collections;
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

    private string appID = "8ac5b43a061d49d6a57360ce4ae6e92b";

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
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Login();
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            JoinChannel();
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            rtmChannel.SendMessage(rtmClient.CreateMessage("Test!"));
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
        rtmClient.Login("", "userID");
    }

    public void JoinChannel()
    {
        rtmChannel = rtmClient.CreateChannel("Lobby", channelEventHandler);
        rtmChannel.Join();
    }

    void OnClientLoginSuccessHandler(int id)
    {
        string msg = "client login successful! id = " + id;
        Debug.Log(msg);
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

    void OnChannelMessageReceivedHandler(int id, string userId, TextMessage message)
    {
        Debug.Log("client OnChannelMessageReceived id = " + id + ", from user:" + userId + " text:" + message.GetText());

        AddChannelToList(message.ToString());
        // *** UPDATE THE CHANNEL BOX HERE *** //
    }

    void OnSendMessageResultHandler(int id, Int64 messageId, CHANNEL_MESSAGE_ERR_CODE errorCode)
    {
        Debug.Log("Message: " + id + " " + messageId + " sent status: " + errorCode);
    }

    void OnMemberJoinedHandler(int id, RtmChannelMember member)
    {
        string msg = "channel OnMemberJoinedHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
        Debug.Log(msg);
        //messageDisplay.AddTextToDisplay(msg, Message.MessageType.Info);
    }

    void OnMemberLeftHandler(int id, RtmChannelMember member)
    {
        string msg = "channel OnMemberLeftHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
        Debug.Log(msg);
        //messageDisplay.AddTextToDisplay(msg, Message.MessageType.Info);
    }

    public void SendMessageToChannel()
    {
        string messageChannelName = channelNameField.text;
        rtmChannel.SendMessage(rtmClient.CreateMessage(messageChannelName));
        //AddChannelToList(messageChannelName);
    }

    void AddChannelToList(string channelName)
    {
        foreach(string channel in channelNames)
        {
            if(channelName == channel)
            {
                return;
            }
        }

        channelNames.Add(channelName);
    }

    void RemoveChannelNameFromList(string channelName)
    {
        for(int i = 0;  i < channelNames.Count; i++)
        {
            if(channelNames[i] == channelName)
            {
                channelNames.RemoveAt(i);
                channelNames.TrimExcess();
                return;
            }
        }
    }
}
