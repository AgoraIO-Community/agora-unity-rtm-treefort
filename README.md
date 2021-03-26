# agora unity rtm treefort
 
<!-----
NEW: Check the "Suppress top comment" option to remove this info from the output.

Conversion time: 4.841 seconds.


Using this Markdown file:

1. Paste this output into your source file.
2. See the notes and action items below regarding this conversion run.
3. Check the rendered output (headings, lists, code blocks, tables) for proper
   formatting and use a linkchecker before you publish this page.

Conversion notes:

* Docs to Markdown version 1.0β29
* Thu Mar 25 2021 21:21:57 GMT-0700 (PDT)
* Source doc: Agora Treefort: Using RTM to Join Breakout Rooms in Unity
* This document has images: check for >>>>>  gd2md-html alert:  inline image link in generated source and store images to your server. NOTE: Images in exported zip file from Google Docs may not appear in  the same order as they do in your doc. Please check the images!


WARNING:
You have 6 H1 headings. You may want to use the "H1 -> H2" option to demote all headings by one level.

----->


<p style="color: red; font-weight: bold">>>>>>  gd2md-html alert:  ERRORs: 0; WARNINGs: 1; ALERTS: 8.</p>
<ul style="color: red; font-weight: bold"><li>See top comment block for details on ERRORs and WARNINGs. <li>In the converted Markdown or HTML, search for inline alerts that start with >>>>>  gd2md-html alert:  for specific instances that need correction.</ul>

<p style="color: red; font-weight: bold">Links to alert messages:</p><a href="#gdcalert1">alert1</a>
<a href="#gdcalert2">alert2</a>
<a href="#gdcalert3">alert3</a>
<a href="#gdcalert4">alert4</a>
<a href="#gdcalert5">alert5</a>
<a href="#gdcalert6">alert6</a>
<a href="#gdcalert7">alert7</a>
<a href="#gdcalert8">alert8</a>

<p style="color: red; font-weight: bold">>>>>> PLEASE check and correct alert issues and delete this message and the inline alerts.<hr></p>



# Agora Treefort: Using RTM to Join Breakout Rooms in Unity

_Meta Description: _

_Learn how to create a 3D hangout space in Unity using Agora RTE video chat and a simple network using Agora’s RTM signaling layer._

*NEED TITLE IMAGE*

Hello, brave developers! In this tutorial you’ll set up a video chat in a 3D Unity environment in Unity 2019.4.1 (LTS) and a messaging feature using agora’s RTM signaling layer. This is essentially a 3D hangout where users communicate with people in the same channel, and have the option to create, enter, and share a new channel with users in the RTM network. On joining the scene, each player chooses their username for the network, and joins a community “LOBBY” channel. Each user can select “NEW ROOM” and create a new Agora channel that will show up for joining by other users.


# Getting Started with Agora

To get started, you need an Agora account. If you don’t have one, [here is a guide to](https://www.agora.io/en/blog/how-to-get-started-with-agora?utm_source=medium&utm_medium=blog&utm_campaign=unity_treefort) setting up an account. 

Here is a [github link](https://github.com/AgoraIO-Community/agora-unity-rtm-treefort) to the completed project

Install the Agora SDK from the Unity Asset Store, 



<p id="gdcalert1" ><span style="color: red; font-weight: bold">>>>>>  gd2md-html alert: inline image link here (to images/image1.png). Store image on your image server and adjust path/filename/extension if necessary. </span><br>(<a href="#">Back to top</a>)(<a href="#gdcalert2">Next alert</a>)<br><span style="color: red; font-weight: bold">>>>>> </span></p>


![alt_text](images/image1.png "image_tooltip")


And install the RTM SDK Unity package from our [github here](https://github.com/AgoraIO-Community/Agora-Unity-RTM-SDK/releases): \


<p id="gdcalert2" ><span style="color: red; font-weight: bold">>>>>>  gd2md-html alert: inline image link here (to images/image2.png). Store image on your image server and adjust path/filename/extension if necessary. </span><br>(<a href="#">Back to top</a>)(<a href="#gdcalert3">Next alert</a>)<br><span style="color: red; font-weight: bold">>>>>> </span></p>


![alt_text](images/image2.png "image_tooltip")



# UI Layout

There are two main states for the UI in this demo. The first is an introductory panel where you choose your network username and join the lobby channel. The second state contains a panel that displays all the active channels, an input field to instantiate a new channel, and a logout button used to exit the channel and RTM network. 

First you’ll create all the necessary UI before coding. Feel free to set up your UI however you’d like, but this is how I made mine, and this is what it looks like:



<p id="gdcalert3" ><span style="color: red; font-weight: bold">>>>>>  gd2md-html alert: inline image link here (to images/image3.png). Store image on your image server and adjust path/filename/extension if necessary. </span><br>(<a href="#">Back to top</a>)(<a href="#gdcalert4">Next alert</a>)<br><span style="color: red; font-weight: bold">>>>>> </span></p>


![alt_text](images/image3.png "image_tooltip")



### Introduction Panel

Right click in the Hierarchy window and select UI > Panel. Select the newly created Canvas object and inside the Inspector window set the Canvas > Render Mode to Screen Space - Overlay. Name your newly created panel “RTMJoinPanel” and set the Rect Transform values to:

Left: 10

Top: 5

Right: 10

Bottom: 340.

 Create a Text object childed to the panel, center it and type a welcome message, I used “Welcome to the Treefort.” Right click on RTMJoinPanel and select UI > Input Field. Name the new object UserNameInputField and set it’s Rect Transform values to: 

Pos X: -85

Pos Y: -20

Width: 160

Height: 30.

Right click on RTMJoinPanel and select UI > Button. Name the button JoinTreefortButton, set the button text to “Join the Fort!” and set it’s Rect Transform values to: 

Pos X: 85

Pos Y: -20

Width: 160

Height: 30.


### Channel Panel

Duplicate the RTMJoinPanel to create another panel in the same location and name the panel ChannelPanel. Rename the duplicated input field to ChannelInputField, the duplicated button to JoinNewChannelButton and the duplicated welcome text to ChannelName. Right click on ChannelPanel and select UI > Dropdown. Last, create a button called LogoutButton. Adjust the X Position of your UI items to suit your liking, or follow the layout I made here:



<p id="gdcalert4" ><span style="color: red; font-weight: bold">>>>>>  gd2md-html alert: inline image link here (to images/image4.png). Store image on your image server and adjust path/filename/extension if necessary. </span><br>(<a href="#">Back to top</a>)(<a href="#gdcalert5">Next alert</a>)<br><span style="color: red; font-weight: bold">>>>>> </span></p>


![alt_text](images/image4.png "image_tooltip")


This “orthographic” environment is a series of simple cubes arranged together. There are 4 spawn points in a square on the floor, that mark where the user video cubes will spawn, like so: 



<p id="gdcalert5" ><span style="color: red; font-weight: bold">>>>>>  gd2md-html alert: inline image link here (to images/image5.png). Store image on your image server and adjust path/filename/extension if necessary. </span><br>(<a href="#">Back to top</a>)(<a href="#gdcalert6">Next alert</a>)<br><span style="color: red; font-weight: bold">>>>>> </span></p>


![alt_text](images/image5.png "image_tooltip")



# Agora Engine

Create an empty GameObject called AgoraEngine in the scene, and attach a script called AgoraEngine.cs. The AgoraEngine will manage the joining and leaving of Agora video channels, and spawning in cubes that render the video from each user.


### Helper Tools

I made a simple script called HelperTools.cs to respond if critical objects are null. It’s not critical to Agora-specific functionality, but if you find it useful the code can be found here.

https://gist.github.com/joelthomas362/6b45f4733b25b307ddf76ed7dca528dd


### Agora Engine

First you’ll add the fields and initialize the RTE engine.

https://gist.github.com/joelthomas362/26280c0a5ec97db8011bad847d5e9fc5

 

Next we’ll create the join channel functionality and implement the callbacks

[https://gist.github.com/joelthomas362/ceccd88dfcbad5e0aa92ee324dcf612d](https://gist.github.com/joelthomas362/ceccd88dfcbad5e0aa92ee324dcf612d)

Last, you’ll implement the spawning and deleting of cubes rendering the Agora video data, and an essential cleanup function.

[https://gist.github.com/joelthomas362/3e93db0dd1a07a0b94941b397fb282e6](https://gist.github.com/joelthomas362/3e93db0dd1a07a0b94941b397fb282e6)


# UI Manager

Create an empty GameObject called UIManager in the scene, and attach a script called UIManager.cs. The UIManager is responsible for updating the available active channels, removing inactive channels, and adjusting the UI to ensure bugs and edge cases are proactively avoided.

First, add the fields and initialize the UIManager.

[https://gist.github.com/joelthomas362/046f6e7f119e0f120aa4945085757ccc](https://gist.github.com/joelthomas362/046f6e7f119e0f120aa4945085757ccc)

Next you’ll implement the functionality that handles the user choosing a channel out of the dropdown list, or creating their own channel using the input field name.

https://gist.github.com/joelthomas362/f408a104bdaadd1de4373fbea89935b7

In this last section, you implement methods called by the RTMManager when a command is sent from a user through the RTM “network” to all other users. When a new channel is added or removed, the network alerts all users and these methods fire.  

[https://gist.github.com/joelthomas362/24239aabf186ebc5e29c7ec37bb7a91d](https://gist.github.com/joelthomas362/24239aabf186ebc5e29c7ec37bb7a91d)


# RtmEngine

The RtmManager is responsible for connecting to Agora’s signaling layer, where you can send text messages through Agora’s network. The RTM client is a unique entity and can be utilized without installing the Agora SDK in your project. In this project, we are using RTM as a “network” that exists outside of the Agora channels. When a user leaves or joins a channel, this action will be communicated to all users in the app, regardless of what channel they are in. 

Create an empty GameObject called RtmEngine in the scene, and attach a script called RtmEngine.cs. Next create the necessary fields and Start() sequence. 

https://gist.github.com/joelthomas362/404c24b037969c171b1109c2b6aa03f2

Next you will implement the functionality placed into buttons to log in and out of the RTM client, and initialize the callbacks referenced in the next section.

[https://gist.github.com/joelthomas362/084e453219fd32f0a7cfdeaa2fe98b26](https://gist.github.com/joelthomas362/084e453219fd32f0a7cfdeaa2fe98b26)

In this last section you implement the RTM related callbacks, and the function to send a message across the “network.” When sent, the message is received in the OnChannelMessageReceivedHandler for every user, where they adjust their UI accordingly based on the message. 

[https://gist.github.com/joelthomas362/2ba739f854b10c511b6976d8bbee6a42](https://gist.github.com/joelthomas362/2ba739f854b10c511b6976d8bbee6a42)


# Editor Setup


### Insector Elements

Now that all the code is written, there will be empty fields inside the editor where you need to drag and drop the respective elements into the empty slots. Go through the AgoraEngine, UIManager, and RtmEngine and make sure they are properly filled out. 

Here is an example:



<p id="gdcalert6" ><span style="color: red; font-weight: bold">>>>>>  gd2md-html alert: inline image link here (to images/image6.png). Store image on your image server and adjust path/filename/extension if necessary. </span><br>(<a href="#">Back to top</a>)(<a href="#gdcalert7">Next alert</a>)<br><span style="color: red; font-weight: bold">>>>>> </span></p>


![alt_text](images/image6.png "image_tooltip")



### Button Scripts

In the Hierarchy panel: select the JoinTreefortButton, drag the RtmEngine object into the OnClick() event in the button’s Inspector, and select Button_JoinTreefort.

Select the JoinNewChannelButton, drag the AgoraEngine object into the OnClick() event, and select Button_JoinButtonPressed.

Last, select the LogoutButton, drag the RtmEngine into the OnClick() event, and select Button_Logout(), like so: 



<p id="gdcalert7" ><span style="color: red; font-weight: bold">>>>>>  gd2md-html alert: inline image link here (to images/image7.png). Store image on your image server and adjust path/filename/extension if necessary. </span><br>(<a href="#">Back to top</a>)(<a href="#gdcalert8">Next alert</a>)<br><span style="color: red; font-weight: bold">>>>>> </span></p>


![alt_text](images/image7.png "image_tooltip")


Dropdown

Click on the Dropdown object. In the Inspector window > Dropdown Component > Options, add “NEW ROOM” as the first and only option in the window, like so:



<p id="gdcalert8" ><span style="color: red; font-weight: bold">>>>>>  gd2md-html alert: inline image link here (to images/image8.png). Store image on your image server and adjust path/filename/extension if necessary. </span><br>(<a href="#">Back to top</a>)(<a href="#gdcalert9">Next alert</a>)<br><span style="color: red; font-weight: bold">>>>>> </span></p>


![alt_text](images/image8.png "image_tooltip")



# Conclusion

Voila! You’ve now used Agora’s RTE and RTM modules to create your very own 3D hangout. If you learned something, make sure to tell someone else, and don’t forget to fill in your app ID in the editor!

Cheers.
