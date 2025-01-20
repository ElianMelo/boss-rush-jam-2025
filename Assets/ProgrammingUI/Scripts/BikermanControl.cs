using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class BikermanControl : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private Vector3 moveDirection;
    private RaycastHit hit;

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
        NormalMovement();
    }

    private void NormalMovement()
    {
        moveDirection = (transform.forward).normalized;

        //ObstacleDetector();

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
        Ray obstacleRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(obstacleRay.origin, obstacleRay.direction * obstacleRayLength, Color.red);

        if (Physics.Raycast(obstacleRay, out RaycastHit obstacleHit, obstacleRayLength, obstacleLayer))
        {
            Vector3 avoidanceDirection = Vector3.Cross(obstacleHit.normal, Vector3.up).normalized;
            moveDirection = Vector3.Lerp(moveDirection, avoidanceDirection, Time.deltaTime * obstacleAvoidanceStrength);
            Debug.Log(moveDirection);
        }
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
