using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [Header("References")]
    public Transform playerAim;
    public GameObject projectilePrefab;
    public GameObject dangerAreaPrefab;

    [Header("Shooting Settings")]
    public Transform shootPoint;
    public float projectileSpeed = 10f;
    public float fireRate = 2f;

    GameObject dangerArea;

    void Update()
    {
        
    }

    //void ShootAtTarget()
    //{
    //    Vector3 targetPosition = player.transform.position + player.transform.forward * 50f;
    //    Debug.Log(targetPosition);

    //    Vector3 directionToTarget = (targetPosition - shootPoint.position).normalized;
    //    GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

    //    Rigidbody rb = projectile.GetComponent<Rigidbody>();
    //    if (rb != null)
    //    {
    //        rb.velocity = directionToTarget * projectileSpeed;
    //    }

    //    Destroy(projectile, 5f);
    //}

    void AimTarget()
    {
        dangerArea = Instantiate(dangerAreaPrefab, playerAim.position, playerAim.rotation);
        Destroy(dangerArea, 2f);
    }

    void ShootAtTarget()
    {
        Vector3 directionToTarget = (dangerArea.transform.position - shootPoint.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = directionToTarget * projectileSpeed;
        }

        Destroy(projectile, 5f);
    }
}
