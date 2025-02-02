using System.Collections;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
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

    private int maxJumps = 1;
    private int jumps;

    private Vector3 smoothDampvelocity = Vector3.zero;

    public MovementState state;

    private PlayerDashingController playerDashingController;
    private PlayerDivingController playerDivingController;
    private PlayerAttackController playerAttackController;
    private PlayerVFXController playerVFXController;

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
    private readonly static string DashAnim = "Dash";
    private readonly static string DiveAnim = "Dive";

    private readonly static string RunningAnim = "Running";
    private readonly static string DrillingAnim = "Drilling";
    private readonly static string FallingAnim = "Falling";
    private readonly static string VerticalVelocityAnim = "VerticalVelocity";
    private readonly static string DashingAnim = "Dashing";
    private readonly static string DivingAnim = "Diving";

    private readonly static string TakeDamageAnim = "TakeDamage";
    private readonly static string DeathAnim = "Death";

    private void Start()
    {
        calculatedTimeRotateBack = timeRotateBack;
        playerAnimator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        playerDashingController = GetComponent<PlayerDashingController>();
        playerDivingController = GetComponent<PlayerDivingController>();
        playerAttackController = GetComponent<PlayerAttackController>();
        playerVFXController = GetComponent<PlayerVFXController>();
        playerRb.freezeRotation = true;
        jumps = maxJumps;

        PlayerManager.Instance.OnPlayerTakeDamage.AddListener(TakeDamage);
        PlayerManager.Instance.OnPlayerDeath.AddListener(Death);
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.OnPlayerTakeDamage.RemoveListener(TakeDamage);
        PlayerManager.Instance.OnPlayerDeath.RemoveListener(Death);
    }

    private void Update()
    {
        CheckAnimation();
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (grounded) jumps = maxJumps;
        if(HeadquartersMananger.Instance != null)
        {
            if (HeadquartersMananger.Instance.CurrentState != HeadquartersState.Walking) return;
        }

        GetInputsActions();
        SpeedControl();
        StateHandler();

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
        if (HeadquartersMananger.Instance != null)
        {
            if (HeadquartersMananger.Instance.CurrentState != HeadquartersState.Walking) return;
        }
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

        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            jumps -= 1;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DiveGround"))
        {
            StartDrilling(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("DiveGround") || other.CompareTag("DiveGroundTrigger"))
        //{
        //    playerAttackController.ForcedSafeStopDrilling();
        //}
    }

    public void StartDrilling(Transform other)
    {
        if (state != MovementState.drilling)
        {
            if(state == MovementState.dashing)
            {
                playerDashingController.ForcedResetDash();
            }
            if(state == MovementState.diving)
            {
                playerDivingController.ForcedResetDive();
            }
            PlayerManager.Instance.StartDrilling();
            SoundManager.Instance.StartDrillingSound();
            PostProcessingManager.Instance.ActivateVignette();
            playerVFXController.EnableDrilling();
            playerVFXController.EnableBooster(true);
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

    public void StopDrilling()
    {
        if (state != MovementState.drilling) return;
        playerVFXController.DisableDrilling();
        playerVFXController.DisableBooster();
        PlayerManager.Instance.StopDrilling();
        SoundManager.Instance.StopDrillingSound();
        PostProcessingManager.Instance.DeactivateVignette();
        jumps = 1;
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
        if (exitDirection.x > 40 && exitDirection.x < 90)
        {
            calculatedTimeRotateBack /= 10;
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
        //if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("DiveGround"))
        //{
        //    grounded = false;
        //}
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
        SoundManager.Instance.PlayJumpSound();
        PlayerManager.Instance.PlayerJumpEvent();
        playerVFXController.EnableBooster();
        playerVFXController.DisableBoosterDelayed(0.1f);
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
        playerAnimator.SetBool(RunningAnim, isRunning);
        playerAnimator.SetBool(DashingAnim, state == MovementState.dashing);
        playerAnimator.SetBool(DivingAnim, state == MovementState.diving);
        playerAnimator.SetBool(DrillingAnim, state == MovementState.drilling);
        playerAnimator.SetBool(FallingAnim, state == MovementState.airing);
        playerAnimator.SetFloat(VerticalVelocityAnim, Mathf.Lerp(0, 1, Mathf.Abs(playerRb.velocity.y) / 20));
    }

    public void CallDashAnimation()
    {
        playerAnimator.SetTrigger(DashAnim);
    }

    public void CallDiveAnimation()
    {
        playerAnimator.SetTrigger(DiveAnim);
    }

    public void TakeDamage()
    {
        ScreenShakeManager.Instance.ShakeScreen();
        playerVFXController.TriggerShockVFX(transform);
        playerAnimator.SetTrigger(TakeDamageAnim);
    }

    public void Death()
    {
        ScreenShakeManager.Instance.ShakeScreen();
        playerVFXController.TriggerShockVFX(transform);
        playerAnimator.SetTrigger(DeathAnim);
    }

}

