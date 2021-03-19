using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


// add RTM network calls to ADD and REMOVE channel from dropdown lists


public class UIManager : MonoBehaviour
{
    public Dropdown channelDropdown;
    public Text channelNameText;
    public Button joinNewChannelButton;
    public InputField newChannelInputField;
    public string currentChannelSelection = "";

    void Start()
    {
        channelDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(); });

        newChannelInputField.onValueChanged.AddListener(delegate { CheckNewChannelName(); });

        joinNewChannelButton.interactable = false;
        newChannelInputField.gameObject.SetActive(false);
    }

    public string GetCurrentChannelSelection() => currentChannelSelection;

    private void DropdownItemSelected()
    {
        int index = channelDropdown.value;
        string value = channelDropdown.options[index].text;

        if(value == "NEW ROOM")
        {
            newChannelInputField.gameObject.SetActive(true);
            newChannelInputField.text = "";
            joinNewChannelButton.interactable = false;
        }
        else
        {
            currentChannelSelection = channelDropdown.options[index].text;
            newChannelInputField.gameObject.SetActive(false);
            joinNewChannelButton.interactable = true;
        }

        if(value.ToUpper() == channelNameText.text.ToUpper())
        {
            joinNewChannelButton.interactable = false;
        }
    }

    void CheckNewChannelName()
    {
        if(newChannelInputField.text == "")
        {
            joinNewChannelButton.interactable = false;
            return;
        }
        else
        {
            for (int i = 0; i < channelDropdown.options.Count; i++)
            {
                if (newChannelInputField.text.ToUpper() == channelDropdown.options[i].text.ToUpper())
                {
                    joinNewChannelButton.interactable = false;
                    return;
                }
                else
                {
                    joinNewChannelButton.interactable = true;
                    currentChannelSelection = newChannelInputField.text;
                }
            }
        }
    }

    public void UpdateChannelNameText(string channelName)
    {
        channelNameText.text = channelName.ToUpper();
    }

    public void AddChannelToDropDownList(string newChannel)
    {
        string newChannelName = newChannel.ToUpper();

        for (int i = 0; i < channelDropdown.options.Count; i++)
        {
            if(newChannelName == channelDropdown.options[i].text.ToUpper())
            {
                Debug.LogWarning("Trying to add a channel name that is already in the dropdown list");
                return;
            }
        }

        List<string> newChannelOption = new List<string>() { newChannelName };
        channelDropdown.AddOptions(newChannelOption);
        channelDropdown.value = channelDropdown.options.Count;

        CheckNewChannelName();
    }

    public void RemoveChannelFromDropDownList(string channelNameToRemove)
    {
        string deletedChannelName = channelNameToRemove.ToUpper();
        if(deletedChannelName == "LOBBY")
        {
            return;
        }

        for(int i = 0; i < channelDropdown.options.Count; i++)
        {
            if(deletedChannelName == channelDropdown.options[i].text.ToUpper())
            {
                channelDropdown.options.RemoveAt(i);
                break;
            }
        }
    }
}
