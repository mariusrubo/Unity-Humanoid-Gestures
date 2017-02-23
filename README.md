# Purpose

These scripts allow you to make your character display the gestures 'scissors', 'stone', 'paper' and 'neutral', and to record and display your own gestures. The scripts allow for a smooth transition between any two gestures, making this approach more flexible than the use of animations.

![alt tag](https://github.com/mariusrubo/Unity-Humanoid-Gestures/blob/master/Gestures.png)

# Installation

* The scripts are designed with characters made with Autodesk Character Generator. If your character contains more or less hand bones than characters from Autodesk, adaptations may be needed.

* Attach the script "Gestures.cs" to your character. Drag both hands' transforms to the corresponding places in the inspector's view on the script. 

* Attach the script "GesturesInterface.cs" to any object in the scene (possibly, but not necessarily the character). Drag your character's transform to the corresponding place in the inspector's view on the script. 

* Create a folder ".../Assets/Gestures" in your project and copy all the files "Gesture0.csv" to "Gesture4.csv" here.

* Press play, click on GUI buttons in the game view.

　

# Record your own gestures

* Attach the script "GestureSave.cs" to your character and drag the character's right hand to the corresponding place in the inspector's view on the script. 

* Press play and disable "GestureInterface.cs", "Gestures.cs" and the character's animator. 

* Manually set the rotations of the right hand's bone to the desired gesture (if you cannot move the hand's bones although the animator and the other scripts are disabled, perform these steps before pressing play, but remember to reenable the animator and the scripts afterwards). 

* Choose a number for the gesture in "GestureSave.cs" and click "Save this Gesture". Warning: If you choose a number that is already linked to a gesture, that gesture will be overwritten. 

* Open "GestureInterface.cs" in MonoDevelop or Visual studio, and set a line in the GUI that will initiate your gesture (as in "if (GUILayout.Button("Right Scissors")) { GestureRight = 1; }". You can do this for both hands, even though the gesture was recorded on the right hand. 

　

# Known issues

* When setting the left hand, the thumb does not exactly mirror the right hand (although the other fingers do). I have tried to adapt to this problem, but there seems to be no linear transformation of rotations that precisely corrects the left thumb in all gestures. The difference is quite subtle, though.

* The script cannot return to "Gesture0.csv" once one of the other four gestures was chosen. This does not constitue a problem if you just keep a gesture in "Gesture0.csv" which you do not need (as it is done in the example files).

# Extend
* For more complex actions (e.g. swinging arms before displaying one of the gestures), this script can be combined with any animation, as it overrides the hand bones' rotations.
* If the hands are supposed to precisely touch objects (e.g. grabbing a cup), I recommend looking into Final IK's interaction system instead.

# License

These scripts run under the GPLv3 license.

　
