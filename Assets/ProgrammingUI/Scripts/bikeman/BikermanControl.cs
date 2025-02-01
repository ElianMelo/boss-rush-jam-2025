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
    private Animator animator;

    [Header("Movement Settings")]
    [SerializeField]
    public float movementSpeed = 0f;
    public float rotationSpeed = 0f;
    public AttackScript attackScript;

    [Header("Slope Settings")]
    [SerializeField]
    public Transform slopeDetector;
    public float rayDistance = 5f;
    public float minSlopeAngle = 1f;
    public LayerMask groundLayer;

    private string targetColliderTag = "BossTriggerZone";

    int randomIntervalProvocation;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        StartCoroutine(callProvocationsLines());
        //StartCoroutine(ChangeDirectionRoutine());
    }

    private void Update()
    {
        NormalMovement();

        if (attackScript.bossHits > 3)
        {
            movementSpeed = 80;
        }

    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            float randomAngle = Random.Range(-20f, 20f); // Adjust the range as needed
            transform.forward = Quaternion.Euler(0, randomAngle, 0) * transform.forward;

            yield return new WaitForSeconds(Random.Range(3, 7)); // Change direction every 3-7 seconds
        }
    }

    private IEnumerator callProvocationsLines()
    {
        randomIntervalProvocation = Random.Range(20, 30);
        SoundManager.Instance.PlayBikermanProvocationSound();
        yield return new WaitForSeconds(randomIntervalProvocation);
        
        StartCoroutine(callProvocationsLines());
    }

    private void NormalMovement()
    {
        moveDirection = (transform.forward).normalized;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetColliderTag))
        {
            if (animator != null)
            {
                animator.SetTrigger("AttackA");
            }
        }
    }
}
