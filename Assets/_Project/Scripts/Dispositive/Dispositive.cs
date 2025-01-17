using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispositive : MonoBehaviour
{
    public Transform targetDispositive;
    public Vector3 rotation;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Lance"))
        {
            targetDispositive.rotation = Quaternion.Euler(rotation);
        }
    }
}
