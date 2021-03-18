using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    void RefreshUI()
    {
        CheckNewChannelName();
    }

    public void AddChannelToDropDownList(string newChannel)
    {
        string newChannelName = newChannel.ToUpper();
        channelNameText.text = newChannelName;

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

        RefreshUI();
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
