using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileDamage : MonoBehaviour
{
    [SerializeField] private GameObject triggerPrefab;
    [SerializeField] private LayerMask groundLayer;

    private void OnCollisionEnter(Collision collision)
    {

        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 collisionPoint = contact.point;

            Destroy(gameObject);

            if (triggerPrefab != null)
            {
                GameObject triggerInstance = Instantiate(triggerPrefab, collisionPoint, Quaternion.identity);
                Destroy(triggerInstance, 0.5f); // Destroy after 1 second.
            }

        }
    }
}
