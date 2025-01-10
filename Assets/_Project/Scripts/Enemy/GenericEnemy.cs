using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenericEnemy : MonoBehaviour
{
    public GameObject explosionVFX;
    public GameObject hitVFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lance"))
        {
            Instantiate(hitVFX, transform.position, Quaternion.identity);
        }
    }
}
