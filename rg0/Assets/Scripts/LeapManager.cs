using UnityEngine;
using System.Collections;
using Leap;

public class LeapManager : MonoBehaviour
{
    public PointerController m_pointerController;

    private Controller m_controller;

    void Start ()
    {
        m_controller = new Controller();
        m_controller.EnableGesture(Gesture.GestureType.TYPECIRCLE,true);
        m_controller.EnableGesture(Gesture.GestureType.TYPESWIPE,true);
    }

    void Update ()
    {
        Frame frame = m_controller.Frame();

        // update the pointer controller
        ToolList tools = frame.Tools;
        if (tools.Count > 0)
        {
            Tool tool = tools[0];
            Vector3 positionVector = new Vector3(tool.TipPosition.x, tool.TipPosition.y, tool.TipPosition.z);
            m_pointerController.ToolMoved(positionVector);
        }

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