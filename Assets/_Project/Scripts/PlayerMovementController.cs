using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Rotator")]
    public float rotationSpeed;
    public float rotationFactor;
    public float timeRotateBack;
    public float diveExitForce;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float dashSpeed;
    public float dashSpeedChangeFactor;

    public float maxYSpeed;

    public float groundDrag;
    public float airDrag;

    [Header("Dashing")]
    public float dashForce;

    [Header("Jumping")]
    public float jumpForce;
    public float airMultiplier;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
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

    private int maxJumps = 2;
    private int jumps;

    public MovementState state;

    private IEnumerator rotateCoroutine;

    private readonly static string JumpAnim = "Jump";
    private readonly static string RunningAnim = "Running";
    private readonly static string DrillingAnim = "Drilling";
    private readonly static string FallingAnim = "Falling";

    public enum MovementState
    {
        idle,
        running,
        dashing,
        drilling,
        airing
    }

    public bool dashing;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
        jumps = maxJumps;
    }

    private void Update()
    {
        // grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (grounded) jumps = maxJumps;

        GetInputs();
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
        Move();
        if(state == MovementState.drilling)
        {
            RotatePlayer();
        } else 
        {
            if(moveDirection != Vector3.zero)
            {
                transform.forward = moveDirection;
            }
        }
    }

    private void GetInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            jumps -= 1;
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && state != MovementState.drilling)
        {
            // canDrill = true;
            StartCoroutine(DelayedCanDrill());
        }
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;

    private void StateHandler()
    {
        // Mode - Dashing
        if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }
        // Mode - Walking
        else if (grounded && state != MovementState.drilling)
        {
            state = MovementState.running;
            desiredMoveSpeed = walkSpeed;
        }
        // Mode - Air
        else
        {
            // state = MovementState.airing;
            desiredMoveSpeed = sprintSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;
        if (lastState == MovementState.dashing) keepMomentum = true;

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
            playerRb.AddForce(GetSlopMoveDirection() * currentMoveSpeed * 20f, ForceMode.Force);
        }

        // on ground
        if (grounded)
        {
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

    private void OnTriggerEnter(Collider other)
    {
        if(state != MovementState.drilling && other.CompareTag("DiveGround"))
        {
            playerRb.useGravity = false;
            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
            }
            transform.LookAt(other.transform);
            transform.Rotate(new Vector3(90f, 0f, 0f));
            state = MovementState.drilling;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DiveGround"))
        {
            playerRb.useGravity = true;
            Vector3 exitVelocity = playerRb.velocity;
            playerRb.velocity = playerRb.velocity.normalized;
            playerRb.AddForce(drillOrientation.forward * diveExitForce, ForceMode.Impulse);
            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
            }
            rotateCoroutine = SmoothRotate(timeRotateBack);
            StartCoroutine(rotateCoroutine);
            StartCoroutine(DelayedDrill());
        }
    }

    private IEnumerator DelayedDrill()
    {
        yield return new WaitForSeconds(timeRotateBack);
        state = MovementState.airing;
    }

    private IEnumerator DelayedCanDrill()
    {
        yield return new WaitForSeconds(0.2f);
        // canDrill = false;
    }

    private IEnumerator SmoothRotate(float waitingTime)
    {
        float currentTimer = 0f;
        float initialXRotation = transform.rotation.eulerAngles.x;
        while (currentTimer <= waitingTime)
        {
            transform.rotation = Quaternion.Slerp(
                Quaternion.Euler(initialXRotation, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 
                Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f)
                , currentTimer / waitingTime);
            currentTimer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            transform.up = collision.contacts[0].normal;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;
        }
    }

    private void SpeedControl()
    {
        // limiting speed on slop
        if (OnSlope())
        {
            if (playerRb.velocity.magnitude > moveSpeed)
            {
                playerRb.velocity = playerRb.velocity.normalized * moveSpeed;
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
        playerAnimator.SetTrigger(JumpAnim);
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
        playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(slopeDetectorFront.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void CheckAnimation()
    {
        playerAnimator.SetBool(RunningAnim, state == MovementState.running);
        playerAnimator.SetBool(DrillingAnim, state == MovementState.drilling);
        playerAnimator.SetBool(FallingAnim, state == MovementState.airing);
    }
    
}

