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

    private int defaultLayer;
    private int invulnerableLayer;

    void Start()
    {
        defaultLayer = transform.parent.gameObject.layer;
        invulnerableLayer = LayerMask.NameToLayer("Invulnerable");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isDrilling)
        {
            //currentAttackDelay = attackDelay;
            //playerVFXController.EnableBooster();
            //playerVFXController.DisableBoosterDelayed(0.2f);
            //playerAnimator.SetTrigger(AttackLeftAnim);
            //playerVFXController.TriggerSlashVFXDelayed(transform.position, transform.rotation, false, 0.1f);
            isAttacking = true;
            animator.SetTrigger("isAttacking");
            SoundManager.Instance.PlayAttackSound();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !isAttacking)
        {
            isDrilling = true;
            spinEffect.Play();
            speedEffect.Play();
            animator.SetTrigger("isDrilling");
            motorbikeControl.IncreaseMovementSpeed();

            transform.parent.gameObject.layer = invulnerableLayer;
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
