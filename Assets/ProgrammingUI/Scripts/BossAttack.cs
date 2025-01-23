using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [Header("References")]
    public GameObject player; // Reference to the player's GameObject
    public GameObject projectilePrefab; // The prefab of the projectile to shoot

    [Header("Shooting Settings")]
    public Transform shootPoint; // The position from which the projectile will be shot
    public float projectileSpeed = 10f; // Speed of the projectile
    public float fireRate = 2f; // Time in seconds between each shot

    private float nextFireTime = 0f;

    void Update()
    {
        
    }

    void ShootAtTarget()
    {
        Vector3 targetPosition = player.transform.position + player.transform.forward * 50f;

        // Calcular a direção para o ponto à frente
        Vector3 directionToTarget = (targetPosition - shootPoint.position).normalized;

        // Instanciar o projétil na posição do ponto de disparo
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        // Configurar a velocidade do projétil
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = directionToTarget * projectileSpeed;
        }

        // Destruir o projétil após 5 segundos para evitar clutter
        Destroy(projectile, 5f);
    }
}
