using UnityEngine;
using System.Collections;
using Leap;

public class LeapManager : MonoBehaviour
{
    private PointerController m_pointerController;

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
        if (tools.Count > 0 && m_pointerController)
        {
            Tool tool = tools[0];
            // leap motion uses different object for vectors
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

    public void ConnectPlayer (GameObject player)
    {
        m_pointerController = player.GetComponent<Wand>().m_pointerController;
    }
}