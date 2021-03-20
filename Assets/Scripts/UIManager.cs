using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public const string DEMO_LOBBY = "LOBBY";
    public const string NEW_ROOM_CREATOR = "NEW ROOM";

#pragma warning disable 649
    [SerializeField] private Dropdown channelDropdown;
    [SerializeField] private Text channelNameText;
    [SerializeField] private Button joinNewChannelButton;
    [SerializeField] private InputField newChannelInputField;
    [SerializeField] private string currentChannelSelection = "";
#pragma warning restore 649

    void Start()
    {
        HelperTools.AssignedInEditorCheck(channelDropdown);
        HelperTools.AssignedInEditorCheck(channelNameText);
        HelperTools.AssignedInEditorCheck(joinNewChannelButton);
        HelperTools.AssignedInEditorCheck(newChannelInputField);

        channelDropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(); });
        newChannelInputField.onValueChanged.AddListener(delegate { CheckNewChannelName(); });

        joinNewChannelButton.interactable = false;
        newChannelInputField.gameObject.SetActive(false);
    }

    public string GetCurrentChannelSelection() => currentChannelSelection;

    // When the user selects a new choice from the dropdown, this function fires
    private void DropdownItemSelected()
    {
        int index = channelDropdown.value;
        string currentDropdownChoice = channelDropdown.options[index].text;

        if(currentDropdownChoice == NEW_ROOM_CREATOR)
        {
            newChannelInputField.gameObject.SetActive(true);
            newChannelInputField.text = "";
            joinNewChannelButton.interactable = false;
        }
        else
        {
            newChannelInputField.gameObject.SetActive(false);
            currentChannelSelection = channelDropdown.options[index].text;
            joinNewChannelButton.interactable = true;
        }

        if(currentDropdownChoice.ToUpper() == channelNameText.text.ToUpper())
        {
            joinNewChannelButton.interactable = false;
        }
    }

    // When the user types a character into the ChannelInputField, this function fires
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
        if(deletedChannelName == DEMO_LOBBY || deletedChannelName == NEW_ROOM_CREATOR)
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

    public void Button_LogOut()
    {

    }
}