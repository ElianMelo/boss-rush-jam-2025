using System.Collections;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerHQMovementController : MonoBehaviour
{
    [Header("Movement Class")]

    [Header("Rotator")]
    public float rotationSpeed;
    public float rotationFactor;
    public float timeRotateBack;
    public float diveExitForce;
    public float smoothFollowMoveDirectionFactor;
    public float smoothFrictionFactor;
    private float calculatedTimeRotateBack;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float dashSpeed;
    public float dashSpeedChangeFactor;
    public float diveSpeed;
    public float diveSpeedChangeFactor;

    public float maxYSpeed;

    public float groundDrag;
    public float airDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float airMultiplier;
    private bool jumping;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsDiveGround;
    private bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public Transform drillOrientation;
    public Transform orientation;
    public Transform cameraOrientation;
    public Transform slopeDetectorFront;
    public Transform slopeDetectorBack;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Animator playerAnimator;
    private Rigidbody playerRb;

    private bool canJump;

    private Vector3 smoothDampvelocity = Vector3.zero;

    public MovementState state;

    private IEnumerator rotateCoroutine;

    public bool IsGrounded => grounded;

    public enum MovementState
    {
        idle,
        running,
        dashing,
        diving,
        drilling,
        airing
    }

    public bool dashing;
    public bool diving;

    private readonly static string JumpAnim = "Jump";
    private readonly static string WalkingAnim = "Walking";

    private void Start()
    {
        calculatedTimeRotateBack = timeRotateBack;
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
        canJump = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (grounded) canJump = true;
        if (HeadquartersMananger.Instance.CurrentState != HeadquartersState.Walking) return;

        GetInputsActions();
        SpeedControl();
        StateHandler();
        CheckAnimation();

        playerRb.velocity = new Vector3(
            Mathf.Clamp(playerRb.velocity.x, playerRb.velocity.x, 20f),
            Mathf.Clamp(playerRb.velocity.y, playerRb.velocity.y, 20f),
            Mathf.Clamp(playerRb.velocity.z, playerRb.velocity.z, 20f));

        if (state == MovementState.running)
        {
            playerRb.drag = groundDrag;
        }
        if(state == MovementState.drilling)
        {
            playerRb.drag = airDrag;
        }
        else
        {
            playerRb.drag = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (HeadquartersMananger.Instance.CurrentState != HeadquartersState.Walking) return;
        if (dashing) return;
        if (diving) return;
        Move();
        if(state == MovementState.drilling)
        {
            RotatePlayer();
        } else 
        {
            if(moveDirection != Vector3.zero)
            {
                SmoothRotateForward();
            }
        }
    }

    private void SmoothRotateForward()
    {
        transform.forward = Vector3.SmoothDamp(transform.forward, moveDirection, ref smoothDampvelocity, smoothFollowMoveDirectionFactor);
    }

    private void GetInputsActions()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (state == MovementState.drilling) return;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
            Jump();
        }
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;

    private void StateHandler()
    {
        // Mode - Dashing
        if (dashing && state != MovementState.drilling)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }
        // Mode - Diving
        else if (diving && state != MovementState.drilling && !grounded)
        {
            state = MovementState.diving;
            desiredMoveSpeed = diveSpeed;
            speedChangeFactor = diveSpeedChangeFactor;
        }
        // Mode - Walking
        else if (grounded && state != MovementState.drilling)
        {
            state = MovementState.running;
            desiredMoveSpeed = walkSpeed;
        }
        // Mode - Air
        else if (!grounded && state != MovementState.drilling)
        {
            state = MovementState.airing;
            desiredMoveSpeed = walkSpeed;
        }
        // Mode - Regular
        else
        {
            desiredMoveSpeed = walkSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing || lastState == MovementState.diving) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    private float speedChangeFactor;

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime * boostFactor;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private void Move()
    {
        if (state == MovementState.dashing) return;
        if (state == MovementState.diving) return;
        float currentMoveSpeed = 0f;

        if (state == MovementState.drilling)
        {
            moveDirection = drillOrientation.forward;
            currentMoveSpeed = moveSpeed / 2;
        } else
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            moveDirection.y = 0f;
            currentMoveSpeed = moveSpeed;
        }

        // on slope
        if (OnSlope())
        {
            playerRb.AddForce(GetSlopMoveDirection() * currentMoveSpeed * 10f, ForceMode.Force);
        }

        // on ground
        if (grounded)
        {
            playerRb.velocity = Vector3.SmoothDamp(playerRb.velocity, Vector3.zero, ref smoothDampvelocity, smoothFrictionFactor);
            playerRb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f, ForceMode.Force);
        }
        // in air
        else if (!grounded)
        {
            playerRb.AddForce(moveDirection.normalized * currentMoveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        
    }

    private void RotatePlayer()
    {
        Quaternion targetRotation = transform.rotation * 
            Quaternion.Euler(verticalInput * rotationSpeed, 0f, (horizontalInput * rotationSpeed * -1));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationFactor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state == MovementState.drilling) return;
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("DiveGround"))
        {
            grounded = true;
        }
    }

    private void SpeedControl()
    {
        // limiting speed on slop
        if (OnSlope())
        {
            Vector3 flatVel = new Vector3(playerRb.velocity.x, playerRb.velocity.y, playerRb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                playerRb.velocity = new Vector3(limitedVel.x, limitedVel.y, limitedVel.z);
            }
        }
        // limiting speed on ground or in air
        else if(state == MovementState.drilling)
        {
            Vector3 flatVel = new Vector3(playerRb.velocity.x, playerRb.velocity.y, playerRb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                playerRb.velocity = new Vector3(limitedVel.x, limitedVel.y, limitedVel.z);
            }
        } else
        {
            Vector3 flatVel = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                playerRb.velocity = new Vector3(limitedVel.x, playerRb.velocity.y, limitedVel.z);
            }
        }

        // limit y vel
        if (maxYSpeed != 0 && playerRb.velocity.y > maxYSpeed)
        {
            playerRb.velocity = new Vector3(playerRb.velocity.x, maxYSpeed, playerRb.velocity.z);
        }
    }

    private void Jump()
    {
        playerRb.drag = 0f;
        jumping = true;
        StartCoroutine(Jumping());
        playerAnimator.SetTrigger(JumpAnim);
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
        playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private IEnumerator Jumping()
    {
        yield return new WaitForSeconds(0.3f);
        jumping = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(slopeDetectorFront.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0 && !jumping;
        }
        return false;
    }

    private Vector3 GetSlopMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void CheckAnimation()
    {
        var isRunning = state == MovementState.running && (verticalInput != 0 || horizontalInput != 0);
        playerAnimator.SetBool(WalkingAnim, isRunning);
    }

}

