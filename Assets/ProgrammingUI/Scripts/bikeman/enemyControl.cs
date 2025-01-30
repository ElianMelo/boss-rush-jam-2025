using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class enemyControl : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private Vector3 moveDirection;
    private RaycastHit hit;

    public GenericEnemyEventListener genericEventListener;

    [Header("Movement Settings")]
    [SerializeField]
    public float movementSpeed = 0f;
    public float rotationSpeed = 0f;

    [Header("Slope Settings")]
    [SerializeField]
    public Transform slopeDetector;
    public float rayDistance = 5f;
    public float minSlopeAngle = 1f;
    public LayerMask groundLayer;

    [Header("Player Detector Settings")]
    [SerializeField]
    public GameObject player;
    public bool playerNear;
    public float detectionRadius = 15f;
    public float followSpeed = 10f;

    [Header("Obstacle Avoidance Settings")]
    [SerializeField]
    public float obstacleRayLength = 3f; // Length of the ray to detect obstacles
    public LayerMask obstacleLayer;      // Layer mask for obstacles
    public float obstacleAvoidanceStrength = 2f; // Strength of direction change when avoiding obstacles

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    public void Death()
    {
        if (genericEventListener != null) genericEventListener.OnEnemyDeath?.Invoke();
    }

    public void DealDamage()
    {
        if (genericEventListener != null) genericEventListener.OnEnemyAttack?.Invoke();
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            playerNear = distanceToPlayer <= detectionRadius && angleToPlayer <= 90f;
        }

        if (playerNear && player != null)
        {
            FollowPlayer();
        }
        else
        {
            NormalMovement();
        }
    }

    private void FollowPlayer()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        Vector3 newForward = Vector3.Lerp(transform.forward, directionToPlayer, Time.deltaTime * followSpeed).normalized;

        moveDirection = newForward;

        // Handle movement on slopes
        if (OnSlope())
        {
            Vector3 slopeDirection = GetSlopeDirection();

            // Velocity to align with the slope and move towards the player
            m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, slopeDirection * movementSpeed, Time.fixedDeltaTime);

            // Rotate the enemy to align with the slope's normal
            transform.rotation = Quaternion.LookRotation(moveDirection, hit.normal);
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void NormalMovement()
    {
        moveDirection = (transform.forward).normalized;

        ObstacleDetector();

        if (OnSlope())
        {
            Vector3 slopeDirection = GetSlopeDirection();

            // Velocity to align with the slope
            m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, slopeDirection * movementSpeed, Time.fixedDeltaTime);

            // Calculate the target rotation based on the slope's normal
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void ObstacleDetector()
    {
        
    }

    private bool OnSlope()
    {
        Ray slopeRay = new Ray(slopeDetector.position, transform.forward);
        Debug.DrawRay(slopeRay.origin, slopeRay.direction * rayDistance, Color.yellow);

        if (Physics.Raycast(slopeRay, out hit, rayDistance, groundLayer))
        {
            Vector3 normalSuperficie = hit.normal;
            float angle = Vector3.Angle(transform.up, normalSuperficie);

            return angle > minSlopeAngle;
        }
        return false;
    }

    public Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, hit.normal).normalized;
    }
}
