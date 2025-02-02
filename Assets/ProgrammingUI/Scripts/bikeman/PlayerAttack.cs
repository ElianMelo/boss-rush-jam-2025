using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Collider attackCollider;
    public ParticleSystem spinEffect;
    public ParticleSystem speedEffect;
    public MotorbikeControl motorbikeControl;
    public bool isAttacking = false;
    public bool isDrilling = false;
    private float currentAttackDelay;
    public float attackDelay;
    public float dashCd;
    float currentDashCooldown;

    private PlayerBikeVFXController playerBikeVFXController;
    private int defaultLayer;
    private int invulnerableLayer;

    void Start()
    {
        currentDashCooldown = dashCd;
        currentAttackDelay = 0;
        defaultLayer = transform.parent.gameObject.layer;
        invulnerableLayer = LayerMask.NameToLayer("Invulnerable");
        playerBikeVFXController = GetComponent<PlayerBikeVFXController>();
    }

    void Update()
    {
        currentAttackDelay -= Time.deltaTime;
        currentDashCooldown -= Time.deltaTime;

        CheckAttackButton();

        if (Input.GetKeyUp(KeyCode.LeftShift) && !isAttacking  && currentDashCooldown <= 0)
        {
            currentDashCooldown = dashCd;
            HealthInterfaceManager.Instance.DashCooldown(dashCd);
            isDrilling = true;
            spinEffect.Play();
            speedEffect.Play();
            animator.SetTrigger("isDrilling");
            motorbikeControl.IncreaseMovementSpeed();
            SoundManager.Instance.PlayDashSound();
            PlayerManager.Instance.StartDrilling();

            transform.parent.gameObject.layer = invulnerableLayer;
        }
    }

    public void CheckAttackButton()
    {
        if (currentAttackDelay > 0) return;

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isDrilling)
        {
            currentAttackDelay = attackDelay;
            Vector3 offset = transform.up * 5.0f;
            playerBikeVFXController.TriggerSlashVFXDelayed(transform.position + offset, transform.rotation, true, 0.2f);
            isAttacking = true;
            animator.SetTrigger("isAttacking");
            SoundManager.Instance.PlayAttackSound();
            PlayerManager.Instance.PlayerAttackEvent();
        }
    }

    void EnableColliderForAttack()
    {
        attackCollider.enabled = true;
    }

    void DisableColliderForAttack()
    {
        attackCollider.enabled = false;
        isAttacking = false;
        transform.parent.gameObject.layer = defaultLayer;
    }

    void turnOffEffects()
    {
        speedEffect.Stop();
        spinEffect.Stop();
        motorbikeControl.DecreaseMovementSpeed();
        isDrilling = false;
        PlayerManager.Instance.StopDrilling();
    }

    public void receiveDamage()
    {
        animator.SetTrigger("isHurt");
        DisableColliderForAttack();
        turnOffEffects();
        transform.parent.gameObject.layer = invulnerableLayer;
    }

    public void stopInvulnerability()
    {
        transform.parent.gameObject.layer = defaultLayer;
    }
}
