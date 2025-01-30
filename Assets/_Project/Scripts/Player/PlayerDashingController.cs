using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingController : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    private Rigidbody rb;
    private PlayerMovementController pm;
    private PlayerAttackController pa;
    private PlayerVFXController pv;

    [Header("Dashing")]
    public float dashForce;
    public float dashDuration;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    private bool stopVfx = false;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.LeftShift;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementController>();
        pa = GetComponent<PlayerAttackController>();
        pv = GetComponent<PlayerVFXController>();
    }

    private void Update()
    {
        if (HeadquartersMananger.Instance != null)
        {
            if (HeadquartersMananger.Instance.CurrentState != HeadquartersState.Walking) return;
        }
        if (Input.GetKeyDown(dashKey))
            Dash();
        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    private void Dash()
    {
        if (dashCdTimer > 0) return;
        else dashCdTimer = dashCd;
        if (pm.state == PlayerMovementController.MovementState.drilling) return;
        pm.dashing = true;
        rb.useGravity = false;
        SoundManager.Instance.PlayDashSound();
        pm.CallDashAnimation();
        pv.EnableBooster();
        pv.DisableBoosterDelayed(0.1f);
        pv.EnableDrilling();
        pa.UnsafeEnableLanceCollider();
        Vector3 forceToApply = orientation.forward * dashForce;

        delayedForceToApply = forceToApply;

        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(ResetDash), dashDuration);
    }

    private Vector3 delayedForceToApply;

    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    public void ForcedResetDash()
    {
        stopVfx = true;
        ResetDash();
    }

    private void ResetDash()
    {
        if (stopVfx || pm.state != PlayerMovementController.MovementState.drilling)
        {
            pv.DisableDrilling();
            pv.DisableBooster();
            stopVfx = false;
        }
        pm.dashing = false;
        rb.useGravity = true;
        pa.DisableLanceCollider();
    }
}
