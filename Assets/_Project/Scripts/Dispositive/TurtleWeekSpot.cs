using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurtleWeekSpot : MonoBehaviour
{
    public Transform expelDirection;
    public float expelForce;
    public Rigidbody playerRigidbody;
    public UnityEvent expelPlayerEvent;

    public void ExpelPlayer()
    {
        expelPlayerEvent?.Invoke();
        playerRigidbody.transform.position = expelDirection.transform.position;
        playerRigidbody.AddForce(expelDirection.forward * expelForce, ForceMode.Impulse);
    }
}
