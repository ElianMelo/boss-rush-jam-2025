using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public MotorbikeControl motorbikeControl;
    public ObstacleControl obstacleScript;
    public GameObject particleEffectPrefab;
    int obstacleBrokenLayer;
    int obstacleLayer;

    public Collider lanceCollider;

    void Start()
    {
        lanceCollider = GetComponent<Collider>();
    }


    public void Update()
    {
        if (motorbikeControl.isBoostActive)
        {
            lanceCollider.enabled = true;
        }
        else
        {
            lanceCollider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        obstacleBrokenLayer = LayerMask.NameToLayer("ObstacleBroken");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            InstantiateParticleEffect(other.transform.position);
        }

        if (other.gameObject.layer == obstacleBrokenLayer)
        {
            Debug.Log("bateu");
            InstantiateParticleEffect(other.transform.position);
            obstacleScript.HandleObstacle(other);
            BossManager.Instance.TakeDamage(25f); 
            //gameControl.TakeDamage(10);
        }
        if (other.gameObject.layer == obstacleLayer)
        {
            InstantiateParticleEffect(other.transform.position);
            obstacleScript.HandleObstacle(other);
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
