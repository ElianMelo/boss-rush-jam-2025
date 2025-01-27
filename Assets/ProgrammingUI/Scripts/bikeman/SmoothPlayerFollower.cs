using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SmoothPlayerFollower : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    public float smoothRotation = 0.3F;
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        transform.rotation = target.rotation;

        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, smoothRotation);
    }

}
