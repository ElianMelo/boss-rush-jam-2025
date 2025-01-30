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
    int BossLayer;

    private void OnTriggerEnter(Collider other)
    {
        obstacleBrokenLayer = LayerMask.NameToLayer("ObstacleBroken");
        obstacleLayer = LayerMask.NameToLayer("Obstacle");
        BossLayer = LayerMask.NameToLayer("Bikerman");

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == obstacleBrokenLayer)
        {
            obstacleScript.HandleObstacle(other);
            BossManager.Instance.TakeDamage(25f);
            Debug.Log("Boss tomou 25 de dano");
        }
        if (other.gameObject.layer == obstacleLayer)
        {
            obstacleScript.HandleObstacle(other);     
        }
        if (other.gameObject.layer == BossLayer)
        {
            BossManager.Instance.TakeDamage(5f);
            Debug.Log("Boss tomou 10 de dano");
        }
    }
}
