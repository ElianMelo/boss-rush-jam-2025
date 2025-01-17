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

    [Header("Movement Settings")]
    [SerializeField]
    public float movementSpeed = 0f;
    public float rotationSpeed = 200f;

    [Header("Slope Settings")]
    [SerializeField]
    public Transform slopeDetector;
    public float rayDistance = 5f;
    public float minSlopeAngle = 1f;

    [Header("Player Detector Settings")]
    [SerializeField]
    public GameObject player;
    public bool playerNear;
    public float detectionRadius = 15f;

    [Header("Obstacle Avoidance Settings")]
    [SerializeField]
    public float obstacleRayLength = 3f; // Length of the ray to detect obstacles
    public LayerMask obstacleLayer;      // Layer mask for obstacles
    public float obstacleAvoidanceStrength = 2f; // Strength of direction change when avoiding obstacles

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Ray slopeRay = new Ray(slopeDetector.position, transform.forward);
            Debug.DrawRay(slopeRay.origin, slopeRay.direction * 4f, Color.yellow);

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
        moveDirection = (player.transform.position - transform.position).normalized;
        float followSpeed = movementSpeed * 1.1f;

        if (OnSlope())
        {
            Vector3 slopeDirection = GetSlopeDirection();

            // Smoothly adjust velocity to align with the slope and move toward the player
            m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, slopeDirection * followSpeed, Time.fixedDeltaTime * 5f);

            // Dynamic rotation to face the player, adjusted by slope
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, hit.normal);
            float dynamicRotationSpeed = rotationSpeed * (m_Rigidbody.velocity.magnitude / followSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, dynamicRotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Smooth velocity adjustment toward the player when not on a slope
            m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, moveDirection * followSpeed, Time.fixedDeltaTime * 5f);

            // Rotate to face the player
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Clamp velocity
        Vector3 flatVel = new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z);

        if (flatVel.magnitude > followSpeed)
        {
            Vector3 clampedVel = flatVel.normalized * followSpeed;
            m_Rigidbody.velocity = new Vector3(clampedVel.x, m_Rigidbody.velocity.y, clampedVel.z);
        }
    }

    private void NormalMovement()
    {
        moveDirection = (transform.forward * 0.5f).normalized;

        Ray obstacleRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(obstacleRay.origin, obstacleRay.direction * obstacleRayLength, Color.red);

        if (Physics.Raycast(obstacleRay, out RaycastHit obstacleHit, obstacleRayLength, obstacleLayer))
        {
            Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, obstacleHit.normal).normalized;
            moveDirection += avoidanceDirection * obstacleAvoidanceStrength;
            moveDirection.Normalize();
        }

        if (OnSlope())
        {
            Vector3 slopeDirection = GetSlopeDirection();

            // Smoothly adjust velocity to align with the slope
            m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, slopeDirection * movementSpeed, Time.fixedDeltaTime * 5f);

            // Dynamic rotation speed based on velocity
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            float dynamicRotationSpeed = rotationSpeed * (m_Rigidbody.velocity.magnitude / movementSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, dynamicRotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Smooth velocity adjustment when not on a slope
            m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, moveDirection * movementSpeed, Time.fixedDeltaTime * 5f);

            // Rotate to follow the adjusted move direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        // Clamp velocity
        Vector3 flatVel = new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z);
        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 clampedVel = flatVel.normalized * movementSpeed;
            m_Rigidbody.velocity = new Vector3(clampedVel.x, m_Rigidbody.velocity.y, clampedVel.z);
        }
    }


    private bool OnSlope()
    {
        Ray slopeRay = new Ray(slopeDetector.position, -transform.up);
        Debug.DrawRay(slopeRay.origin, slopeRay.direction * 4f, Color.yellow);

        if (Physics.Raycast(slopeRay, out hit, rayDistance))
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
