using GameServices.MobileInputService;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JoyStick))]
public class JoystickEditor : Editor
{
    public void OnSceneGUI()
    {
        JoyStick joystick = target as JoyStick;
        Handles.color = Color.yellow;

        if (!EditorApplication.isPlaying)
            Handles.DrawWireDisc(joystick.transform.position, joystick.transform.forward, joystick.MovementRadius);
        else
            Handles.DrawWireDisc(joystick.InitialPosition, joystick.transform.forward, joystick.MovementRadius);
    }
}