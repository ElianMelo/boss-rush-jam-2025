using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Collider attackCollider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //currentAttackDelay = attackDelay;
            //playerVFXController.EnableBooster();
            //playerVFXController.DisableBoosterDelayed(0.2f);
            //playerAnimator.SetTrigger(AttackLeftAnim);
            //playerVFXController.TriggerSlashVFXDelayed(transform.position, transform.rotation, false, 0.1f);
            animator.SetTrigger("isAttacking");
            SoundManager.Instance.PlayAttackSound();
        }
    }

    void EnableColliderForAttack()
    {
        attackCollider.enabled = true;
        Debug.Log("ligou");
    }

    void DisableColliderForAttack()
    {
        attackCollider.enabled = false;
        Debug.Log("Desligou");
    }
}
