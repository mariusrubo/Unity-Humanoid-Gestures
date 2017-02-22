using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GesturesInterface : MonoBehaviour {

    [Tooltip("Manually drag your character here.")]
    public Transform Character;
    private Gestures CharacterGestures = null;
    int GestureRight = 0;
    int GestureLeft = 0;
    float GestureSpeed = 0.015f; // set this to public to controll speed while script is running

    // Use this for initialization
    void Start () {
        CharacterGestures = Character.GetComponent<Gestures>();
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(230, 10, 100, 200)); 
        if (GUILayout.Button("Right Scissors")) { GestureRight = 1; }
        if (GUILayout.Button("Right Rock")) { GestureRight = 2; }
        if (GUILayout.Button("Right Paper")) { GestureRight = 3; }
        if (GUILayout.Button("Right Neutral")) { GestureRight = 4; }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(340, 10, 100, 200)); 
        if (GUILayout.Button("Left Scissors")) { GestureLeft = 1; }
        if (GUILayout.Button("Left Rock")) { GestureLeft = 2; }
        if (GUILayout.Button("Left Paper")) { GestureLeft = 3; }
        if (GUILayout.Button("Left Neutral")) { GestureLeft = 4; }
        GUILayout.EndArea();
    }

    // Update is called once per frame
    void Update () {
        CharacterGestures.SetGestures(GestureRight, GestureLeft, GestureSpeed);
    }
}
