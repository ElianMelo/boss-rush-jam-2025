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
            var hitVFXObject = Instantiate(hitVFX, transform.position, Quaternion.identity);
            hitVFXObject.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f));
            var explosionVFXObject = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            explosionVFXObject.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f));
        }
    }
}
