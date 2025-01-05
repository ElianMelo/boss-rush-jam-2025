using System.Collections;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Rotator")]
    public float rotationSpeed;
    public float rotationFactor;
    public float timeRotateBack;
    public float diveExitForce;
    private float calculatedTimeRotateBack;

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

    private int maxJumps = 1;
    private int jumps;

    public MovementState state;

    private IEnumerator rotateCoroutine;

    private readonly static string JumpAnim = "Jump";
    private readonly static string AttackAnim = "Attack";

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
        calculatedTimeRotateBack = timeRotateBack;
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        playerRb.freezeRotation = true;
        jumps = maxJumps;
    }

    private void Update()
    {
        // grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (grounded) jumps = maxJumps;

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

    private void GetInputsActions()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0 && state != MovementState.drilling)
        {
            jumps -= 1;
            Jump();
        }

        if(Input.GetKeyDown(KeyCode.Mouse0) && state != MovementState.drilling)
        {
            playerAnimator.SetTrigger(AttackAnim);
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
        else if (!grounded && state != MovementState.drilling)
        {
            state = MovementState.airing;
        }
        // Mode - Regular
        else
        {
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
        if(other.CompareTag("DiveGround"))
        {
            StartDrilling(other.transform);
        }
    }

    public void StartDrilling(Transform other)
    {
        if (state != MovementState.drilling)
        {
            playerRb.useGravity = false;
            if (rotateCoroutine != null)
            {
                StopCoroutine(rotateCoroutine);
            }
            transform.LookAt(other);
            transform.Rotate(new Vector3(90f, 0f, 0f));
            state = MovementState.drilling;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DiveGround") || other.CompareTag("DiveGroundTrigger"))
        {
            StopDrilling();
        }
    }

    public void StopDrilling()
    {
        if (state != MovementState.drilling) return;
        playerRb.useGravity = true;
        Vector3 exitDirection = drillOrientation.eulerAngles;
        calculatedTimeRotateBack = timeRotateBack;
        playerRb.velocity = playerRb.velocity.normalized;
        playerRb.AddForce(drillOrientation.forward * diveExitForce, ForceMode.Impulse);
        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
        }
        if(exitDirection.x < 40)
        {
            calculatedTimeRotateBack /= 5;
        }
        rotateCoroutine = SmoothRotate(calculatedTimeRotateBack);
        StartCoroutine(rotateCoroutine);
        StartCoroutine(DelayedDrill());
    }

    private IEnumerator DelayedDrill()
    {
        yield return new WaitForSeconds(calculatedTimeRotateBack);
        state = MovementState.airing;
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
        if (state == MovementState.drilling) return;
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("DiveGround"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // todo: create a better check
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("DiveGround"))
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
        var isRunning = state == MovementState.running && (verticalInput != 0 || horizontalInput != 0);
        playerAnimator.SetBool(RunningAnim, isRunning);
        playerAnimator.SetBool(DrillingAnim, state == MovementState.drilling);
        playerAnimator.SetBool(FallingAnim, state == MovementState.airing);
    }
    
}

