using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class MouseHorizontalRotator : MonoBehaviour
{
    public float rotationPower = 3f;

    private void FixedUpdate()
    {
        transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationPower, Vector3.up);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }

}
