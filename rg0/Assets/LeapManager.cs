using UnityEngine;
using System.Collections;
using Leap;

public class LeapManager : MonoBehaviour {
    Controller controller;

    void Start ()
    {
        controller = new Controller();
        controller.EnableGesture(Gesture.GestureType.TYPECIRCLE,true);
        controller.EnableGesture(Gesture.GestureType.TYPESWIPE,true);
    }

    void Update ()
    {
        Frame frame = controller.Frame();
        // do something with the tracking data in the frame...
        GestureList gestures = frame.Gestures();
        if (gestures.Count > 0)
        {
            foreach (Gesture gesture in gestures)
            {
                if (gesture.State == Gesture.GestureState.STATESTOP)
                {
                    Debug.Log(gesture.Type + " : " + gesture.Duration);
                }
            }
        }
    }
}