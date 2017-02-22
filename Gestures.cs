using UnityEngine;
using System.Collections;
using System.IO; //for csv
using System.Text; // for csv

public class Gestures : MonoBehaviour {
    
    //public int GestureRight;
    int GestureRightLastFrame = 0; //tracks GestureNumber so we can see if it changes
    float SlerpCoefRight = 1f; //for logistic interpolation between two gestures

    //public int GestureLeft;
    int GestureLeftLastFrame = 0; //tracks GestureNumber so we can see if it changes
    float SlerpCoefLeft = 1f;

    public Transform RightHand;
    Transform[] RightHandParts; // will note all bones that are part of the right hand

    public Transform LeftHand;
    Transform[] LeftHandParts;

    Quaternion[][] GestureDataRight; //Array of Arrays to store several gestures
    Quaternion[][] GestureDataLeft; // somewhat strange: data must be stored individually for each hand, otherwise reading process in one will also initiate reading process in other

    Quaternion[] RightHandRotations; //actual bone rotations in specific frame
    Quaternion[] RightHandRotationsGoal; //current goal gesture (to which bones are gradually moved)
    Quaternion[] RightHandRotationsLast; //needed for logistic movement towards Goal. Captures last Gesture.

    Quaternion[] LeftHandRotations; //actual bone rotations in specific frame
    Quaternion[] LeftHandRotationsGoal; //current goal gesture (to which bones are gradually moved)
    Quaternion[] LeftHandRotationsLast; //needed for logistic movement towards Goal. Captures last Gesture.


    void Start () {
        RightHandParts = RightHand.GetComponentsInChildren<Transform>(); //get all fingers and finger parts
        LeftHandParts = LeftHand.GetComponentsInChildren<Transform>(); //get all fingers and finger parts  

        GestureDataRight = LoadGestures(); // load hand rotations
        GestureDataLeft = LoadGestures();

        RightHandRotations = GestureDataRight[GestureRightLastFrame]; //take Gesture0 as first Gesture
        RightHandRotationsGoal = GestureDataRight[GestureRightLastFrame]; //aswell as first goal
        RightHandRotationsLast = GestureDataRight[GestureRightLastFrame]; //aswell as last rotation
        //GestureRightLastFrame = GestureRight;

        LeftHandRotations = GestureDataLeft[GestureLeftLastFrame];
        LeftHandRotationsGoal = GestureDataLeft[GestureLeftLastFrame];
        LeftHandRotationsLast = GestureDataLeft[GestureLeftLastFrame];
        //GestureLeftLastFrame = GestureLeft;
    }

        // function to be called from outside
     public void SetGestures(int GestureRight, int GestureLeft, float GestureSpeed) { 

        // Right Hand
        if (GestureRight != GestureRightLastFrame) // detect when gesture is changed, an start lerping
        {
            RightHandRotationsGoal = GestureDataRight[GestureRight]; 
            RightHandRotationsLast = RightHandRotations; //save current rotations as reference for lerping:
            //(needed in linear (lerp) and logistic (slerp) transformation as opposed to limited growth transformation, which is used for facial expressions but looks odd in gestures)
            SlerpCoefRight = 0f; // start (s)lerping at 0
        }

        if (SlerpCoefRight < 1) {

            SlerpCoefRight += GestureSpeed; 
            for (int i = 0; i < 22; i++)
            {
            RightHandRotations[i] = Quaternion.Lerp(RightHandRotationsLast[i], RightHandRotationsGoal[i], SlerpCoefRight); // switch between Quaternion.Lerp and Quaternion.Slerp. No big difference imo
            }
        }
        GestureRightLastFrame = GestureRight; // note number to check if it changed in the next frame
        
        
        // Left Hand
        if (GestureLeft != GestureLeftLastFrame)
        {
            LeftHandRotationsGoal = GestureDataLeft[GestureLeft];
            LeftHandRotationsLast = LeftHandRotations; 
            SlerpCoefLeft = 0f; 
        }

        if (SlerpCoefLeft < 1)
        {

            SlerpCoefLeft += GestureSpeed;
            for (int i = 0; i < 22; i++)
            {
                LeftHandRotations[i] = Quaternion.Lerp(LeftHandRotationsLast[i], LeftHandRotationsGoal[i], SlerpCoefLeft); // switch between Quaternion.Lerp and Quaternion.Slerp. No big difference imo
            }
        }
        GestureLeftLastFrame = GestureLeft; // note number to check if it changed in the next frame
        
    }

    void LateUpdate()
    {
        // and actually adapt rotations. Do it in LateUpdate() to override animation
        for (int i = 0; i < 22; i++)
         {
                RightHandParts[i].localRotation = RightHandRotations[i];
                LeftHandParts[i].localRotation = LeftHandRotations[i]; 
         }

        // Correct left hand thumbs (I substract the difference I found between left and right bones when in same position. However, this difference may not be the same for all positions, e.g.
        // no linear transformation may correctly adjust the lefthand thumb in all positions. I asked in the autodesk forum about this problem, waiting for responses.)
        LeftHandParts[18].localRotation *= Quaternion.Euler(-84f, -45f, 29f);
        LeftHandParts[19].localRotation *= Quaternion.Euler(-2f, -22f, 28f);
        LeftHandParts[20].localRotation *= Quaternion.Euler(0f, -24f, 25f);
    }

    // load hand rotations from disc
    public Quaternion[][] LoadGestures()
    {
        string readpath = Path.GetFullPath(".");
        readpath = readpath + "/Assets/Gestures/"; // go to this folder
        string[] readpathsEachGesture = Directory.GetFiles(@readpath, "Gesture*" + "*.csv"); // and find all csv-files with "Gesture" in their names
        Quaternion[][] GestureData = new Quaternion[readpathsEachGesture.Length][]; // stretch array with all gesture data to the right size

        for (int i = 0; i < readpathsEachGesture.Length; i++) //load each gesture individually
        {
            string readpathThisGesture = readpathsEachGesture[i];
            string[] rotations = File.ReadAllText(readpathThisGesture).Split(';');

            GestureData[i] = new Quaternion[rotations.Length / 4]; // there must be rotations.Length/4 bones since each bone got value x,y,z,w

            for (int j = 0; j < rotations.Length / 4; j++)
            {
                GestureData[i][j].x = float.Parse(rotations[j * 4]);
                GestureData[i][j].y = float.Parse(rotations[j * 4 + 1]);
                GestureData[i][j].z = float.Parse(rotations[j * 4 + 2]);
                GestureData[i][j].w = float.Parse(rotations[j * 4 + 3]);
            }
        }

        return GestureData;
    }

}
