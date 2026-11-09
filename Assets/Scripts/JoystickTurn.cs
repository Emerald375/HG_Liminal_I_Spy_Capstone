using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static OVRPlugin;

public class JoystickTurn : MonoBehaviour
{
    public bool joysticksEnabled = true;
    public GameObject playerHead;

    public float turnSpeed = 60f;
    public float smoothTime = 0.15f;
    public float deadzone = 0.15f;

    private float currentTurnSpeed = 0f;
    private float turnVelocity = 0f;
    void Update()
    {
        if (joysticksEnabled == false) return;

        float rightStick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
        float leftStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        float turnInput = rightStick + leftStick;
        turnInput = Mathf.Clamp(turnInput, -1f, 1f);

        if (Input.GetKey(KeyCode.E))
        {
            turnInput = 1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            turnInput = -1f;
        }

        if (Mathf.Abs(turnInput) < deadzone)
        {
            turnInput = 0f;
        }

        float targetSpeed = turnInput * turnSpeed;
        currentTurnSpeed = Mathf.SmoothDamp(currentTurnSpeed, targetSpeed, ref turnVelocity, smoothTime);

        if (Mathf.Abs(currentTurnSpeed) > 0.1f)
        {
            playerHead.transform.Rotate(0f, currentTurnSpeed * Time.deltaTime, 0f, Space.World);
        }
    }
}
