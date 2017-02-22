using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //for csv
using System.Text; // for csv

public class GestureSave : MonoBehaviour {

    public int GestureNumber;
    public bool SaveThisGesture = false; // set public for gesture creation mode


    public Transform RightHand;
    Transform[] RightHandParts;

    // Use this for initialization
    void Start () {
        RightHandParts = RightHand.GetComponentsInChildren<Transform>(); //get all fingers and finger parts
    }
	
	// Update is called once per frame
	void Update () {
        // Save current gesture on click
        if (SaveThisGesture == true) { SaveThisGesture = SaveGesture(); }
    }

    public bool SaveGesture()
    {
        Quaternion[] GestureToSave = new Quaternion[22];
        for (int i = 0; i < RightHandParts.Length; i++)
        {
            GestureToSave[i] = RightHandParts[i].localRotation;
        }
        string path = Directory.GetCurrentDirectory();
        path = path + "/Assets/Gestures";
        if (!Directory.Exists(path)) { Directory.CreateDirectory(path); } // create "Gesture" folder if it does not exist
        path = path + "/Gesture" + GestureNumber + ".csv";
        string Text = "";
        for (int j = 0; j < GestureToSave.Length; j++)
        {
            Text = Text + GestureToSave[j].x.ToString() + ";";
            Text = Text + GestureToSave[j].y.ToString() + ";";
            Text = Text + GestureToSave[j].z.ToString() + ";";
            Text = Text + GestureToSave[j].w.ToString() + ";";
            //Text = Text + System.Environment.NewLine; //newline makes csv-files more human-readable, but strangely difficult to read in Unity.
        }
        File.WriteAllText(path, Text);

        return false; // always return false, to swith off "SaveThisGesture"
    }
}
