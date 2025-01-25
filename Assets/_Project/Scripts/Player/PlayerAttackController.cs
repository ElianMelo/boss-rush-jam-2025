using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    public Collider lanceCollider;
    public bool IsDrilling;
    public float attackDelay;

    private PlayerMovementController playerMovementController;
    private PlayerDashingController playerDashingController;
    private PlayerDivingController playerDivingController;
    private PlayerVFXController playerVFXController;

    private Animator playerAnimator;

    private float currentAttackDelay;
    private bool isLeftAttack = true;
    private DriveGroundTrigger lastDriveGroundTrigger;

    private readonly static string AttackLeftAnim = "AttackLeft";
    private readonly static string AttackRightAnim = "AttackRight";

    private void Start()
    {
        currentAttackDelay = 0;
        playerMovementController = GetComponent<PlayerMovementController>();
        playerDashingController = GetComponent<PlayerDashingController>();
        playerDivingController = GetComponent<PlayerDivingController>();
        playerVFXController = GetComponent<PlayerVFXController>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        currentAttackDelay -= Time.deltaTime;
        CheckAttackButton();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EnemyAttack"))
        {
            PlayerManager.Instance.TakeDamage(10f);
        }
    }

    private void CheckAttackButton()
    {
        if (currentAttackDelay > 0) return;
        if (playerMovementController.state == PlayerMovementController.MovementState.drilling) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            currentAttackDelay = attackDelay;
            playerVFXController.EnableBooster();
            playerVFXController.DisableBoosterDelayed(0.2f);
            if(isLeftAttack)
            {
                playerAnimator.SetTrigger(AttackLeftAnim);
            } else
            {
                playerAnimator.SetTrigger(AttackRightAnim);
            }
            isLeftAttack = !isLeftAttack;
            SoundManager.Instance.PlayAttackSound();
            playerVFXController.TriggerSlashVFXDelayed(transform.position, transform.rotation, isLeftAttack, 0.1f);
        }
    }

    public void StartDrilling(DriveGroundTrigger driveGroundTrigger)
    {
        IsDrilling = true;
        if (driveGroundTrigger.GetWeakPoint() && driveGroundTrigger.GetFirstTime())
        {
            BossManager.Instance.TakeDamage(25f);
            ScreenShakeManager.Instance.ShakeScreen();
            driveGroundTrigger.SetFirstTime(false);
        }
        driveGroundTrigger.DisableCollider();
        lastDriveGroundTrigger = driveGroundTrigger;
        playerMovementController.StartDrilling(driveGroundTrigger.transform);
    }

    public void StopDrilling(DriveGroundTrigger driveGroundTrigger)
    {
        IsDrilling = false;
        driveGroundTrigger.EnableColllider();
        playerMovementController.StopDrilling();
    }

    public void ForcedSafeStopDrilling()
    {
        StopDrilling(lastDriveGroundTrigger);
    }

    public void EnableLanceCollider()
    {
        lanceCollider.enabled = true;
        StartCoroutine(SafeDisableCollider());
    }

    public void UnsafeEnableLanceCollider()
    {
        lanceCollider.enabled = true;
    }

    public void DisableLanceCollider()
    {
        lanceCollider.enabled = false;
    }

    private IEnumerator SafeDisableCollider()
    {
        yield return new WaitForSeconds(0.3f);
        DisableLanceCollider();
    }
}
