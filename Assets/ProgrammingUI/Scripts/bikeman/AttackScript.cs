using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public GameObject explosionVFX;
    public GameObject hitVFX;

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
            HitEffects(other.transform);
            enemyControl enemy = other.GetComponent<enemyControl>();
            enemy.Death();
            Destroy(other.gameObject, 0.2f);
        }
        if (other.gameObject.layer == obstacleBrokenLayer)
        {
            HitEffects(other.transform);
            obstacleScript.HandleObstacle(other);
            BossManager.Instance.TakeDamage(25f);
            ScreenShakeManager.Instance.ShakeScreen();
            Debug.Log("Boss tomou 25 de dano");
        }
        if (other.gameObject.layer == obstacleLayer)
        {
            HitEffects(other.transform);
            obstacleScript.HandleObstacle(other);
        }
        if (other.gameObject.layer == BossLayer)
        {
            BossManager.Instance.TakeDamage(5f);
            ScreenShakeManager.Instance.ShakeScreen();
            Debug.Log("Boss tomou 10 de dano");
        }
    }

    public void HitEffects(Transform hitposition)
    {
        Vector3 offsetPosition = hitposition.up * 2f;

        var hitVFXObject = Instantiate(hitVFX, hitposition.position + offsetPosition, hitposition.rotation);
        var explosionVFXObject = Instantiate(explosionVFX, hitposition.position + offsetPosition, hitposition.rotation);

        explosionVFXObject.transform.localScale = new Vector3(
            Random.Range(0.8f, 1.2f),
            Random.Range(0.8f, 1.2f),
            Random.Range(0.8f, 1.2f)
        );
        ScreenShakeManager.Instance.ShakeScreen();
    }
}
