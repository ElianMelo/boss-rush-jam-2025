using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MouseRotator : MonoBehaviour
{
    private NavMeshAgent _agent;
    public Vector2 _move;
    public Vector2 _look;
    public float aimValue;
    public float fireValue;

    public Vector3 nextPosition;
    public Quaternion nextRotation;

    public float rotationPower = 3f;
    public float rotationLerp = 0.5f;

    public float speed = 1f;
    public Camera camera;
    
    [Header("Input Settings")]
    public float joystickLookSensitivity = 2f;
    public float mouseLookSensitivity = 1f;
    public float joystickDeadzone = 0.2f;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        // Get input from both mouse and joystick
        GetLookInput();
        
        #region Player Based Rotation

        //Move the player based on the X input on the controller
        //transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

        #endregion

        #region Follow Transform Rotation

        //Rotate the Follow Target transform based on the input
        transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

        #endregion

        #region Vertical Rotation
        transform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);

        var angles = transform.localEulerAngles;
        angles.z = 0;

        var angle = transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        transform.localEulerAngles = angles;
        #endregion

        nextRotation = Quaternion.Lerp(transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        if (_move.x == 0 && _move.y == 0)
        {
            nextPosition = transform.position;

            if (aimValue == 1)
            {
                //Set the player rotation based on the look transform
                transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                //reset the y rotation of the look transform
                transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }

            return;
        }
        float moveSpeed = speed / 100f;
        Vector3 position = (transform.forward * _move.y * moveSpeed) + (transform.right * _move.x * moveSpeed);
        nextPosition = transform.position + position;

        //Set the player rotation based on the look transform
        // playerTransform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }

    private void GetLookInput()
    {
        // Get mouse input
        Vector2 mouseLook = new Vector2(
            Input.GetAxis("Mouse X") * mouseLookSensitivity,
            Input.GetAxis("Mouse Y") * -1 * mouseLookSensitivity
        );

        // Get joystick right analog stick input
        Vector2 joystickLook = new Vector2(
            Input.GetAxis("RightStickHorizontal"),
            Input.GetAxis("RightStickVertical")
        );

        // Apply deadzone to joystick input
        if (joystickLook.magnitude < joystickDeadzone)
        {
            joystickLook = Vector2.zero;
        }
        else
        {
            joystickLook = joystickLook.normalized * ((joystickLook.magnitude - joystickDeadzone) / (1 - joystickDeadzone));
            joystickLook *= joystickLookSensitivity;
        }

        // Combine inputs (mouse takes priority if both are active)
        if (mouseLook != Vector2.zero)
        {
            _look = mouseLook;
        }
        else
        {
            _look = joystickLook;
        }
        
        // Alternative: Add both inputs together for simultaneous control
        // _look = mouseLook + joystickLook;
    }
}