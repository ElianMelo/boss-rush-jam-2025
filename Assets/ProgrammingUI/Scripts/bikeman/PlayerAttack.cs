using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //currentAttackDelay = attackDelay;
            //playerVFXController.EnableBooster();
            //playerVFXController.DisableBoosterDelayed(0.2f);
            //playerAnimator.SetTrigger(AttackLeftAnim);
            //playerVFXController.TriggerSlashVFXDelayed(transform.position, transform.rotation, false, 0.1f);
            animator.SetTrigger("attackLeft");

            SoundManager.Instance.PlayAttackSound();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //currentAttackDelay = attackDelay;
            //playerVFXController.EnableBooster();
            //playerVFXController.DisableBoosterDelayed(0.2f);
            //playerAnimator.SetTrigger(AttackRightAnim);
            //playerVFXController.TriggerSlashVFXDelayed(transform.position, transform.rotation, true, 0.1f);
            SoundManager.Instance.PlayAttackSound();
            animator.SetTrigger("attackRight");
        }
    }
}
