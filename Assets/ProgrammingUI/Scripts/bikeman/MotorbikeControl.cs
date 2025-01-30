using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MotorbikeControl : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private Vector3 moveDirection;
    private RaycastHit hit;

    [Header("Movement Settings")]
    [SerializeField]
    public float movementSpeed = 5f;
    public float originalSpeed = 14f;
    public float maxSpeed = 24f;
    public float speedIncrease = 2f;
    public float rotationSpeed = 100f;

    [Header("Slope Settings")]
    [SerializeField]
    public Transform slopeDetector;
    public float minSlopeAngle = 1f;
    public float rayDistance = 5f;
    

    [Header("Nitro Settings")]
    [SerializeField]
    public bool isBoostActive = false;
    private float boostTimeRemaining = 0f;
    public float boostDuration = 5f;
    public float rechargeRate = 1f;
    public Animator animator;


    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        boostTimeRemaining = boostDuration;
    }

    void FixedUpdate()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        moveDirection = (transform.forward + transform.right * inputHorizontal * 0.5f).normalized;

        if (OnSlope())
        {
            Vector3 slopeDirection = GetSlopeDirection();

            m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, slopeDirection * movementSpeed, Time.fixedDeltaTime * 5f);

            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            float dynamicRotationSpeed = rotationSpeed * (m_Rigidbody.velocity.magnitude / movementSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, dynamicRotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Smooth velocity adjustment when not on a slope
            m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, moveDirection * movementSpeed, Time.fixedDeltaTime * 5f);
        }

        // Clamp velocity
        Vector3 flatVel = new Vector3(m_Rigidbody.velocity.x, 0, m_Rigidbody.velocity.z);
        if (flatVel.magnitude > movementSpeed)
        {
            Vector3 clampedVel = flatVel.normalized * movementSpeed;
            m_Rigidbody.velocity = new Vector3(clampedVel.x, m_Rigidbody.velocity.y, clampedVel.z);
        }

        // Apply rotation based on input
        transform.Rotate(0, inputHorizontal * rotationSpeed * Time.fixedDeltaTime, 0);
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

    public void IncreaseMovementSpeed()
    {
        movementSpeed = maxSpeed;
    }

    public void DecreaseMovementSpeed()
    {
        movementSpeed = originalSpeed;
    }
}
