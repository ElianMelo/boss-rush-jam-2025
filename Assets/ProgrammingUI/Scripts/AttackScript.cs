using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public ObstacleControl obstacleScript;
    public GameControl gameControl;
    public GameObject particleEffectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            InstantiateParticleEffect(other.transform.position);
        }

        int obstacleLayer = LayerMask.NameToLayer("ObstacleBroken");
        if (other.gameObject.layer == obstacleLayer)
        {
            InstantiateParticleEffect(other.transform.position);
            obstacleScript.HandleObstacle(other);
            BossManager.Instance.TakeDamage(25f); 
            //gameControl.TakeDamage(10);
        }
    }

    private void InstantiateParticleEffect(Vector3 position)
    {
        if (particleEffectPrefab != null)
        {
            Instantiate(particleEffectPrefab, position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Particle effect prefab is not assigned.");
        }
    }
}
