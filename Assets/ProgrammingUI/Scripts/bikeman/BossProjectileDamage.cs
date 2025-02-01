using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectileDamage : MonoBehaviour
{
    [SerializeField] private GameObject triggerPrefab;
    [SerializeField] private LayerMask groundLayer;
    public GameObject explosionVFX;
    Quaternion collisionRotation;

    private void OnCollisionEnter(Collision collision)
    {

        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            ContactPoint contact = collision.contacts[0];
            Vector3 collisionPoint = contact.point;
            collisionRotation = Quaternion.LookRotation(contact.normal);

            Destroy(gameObject);

            if (triggerPrefab != null)
            {
                GameObject triggerInstance = Instantiate(triggerPrefab, collisionPoint, Quaternion.identity);
                HitEffects(triggerInstance.transform);
                Destroy(triggerInstance, 0.5f); // Destroy after 1 second.
            }

        }
    }

    public void HitEffects(Transform hitposition)
    {
        Vector3 offsetPosition = hitposition.up * 2f;

        var explosionVFXObject = Instantiate(explosionVFX, hitposition.position + offsetPosition, collisionRotation);

        explosionVFXObject.transform.localScale = new Vector3(
            Random.Range(0.8f, 1.2f),
            Random.Range(0.8f, 1.2f),
            Random.Range(0.8f, 1.2f)
        );
        ScreenShakeManager.Instance.ShakeScreen();
    }
}
