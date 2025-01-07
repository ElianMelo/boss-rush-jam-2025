using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashingController : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovementController pm;
    private PlayerAttackController pa;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
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
    }

    private void Update()
    {
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
        pm.CallDashAnimation();
        pm.drillingVfx.Play();
        pa.UnsafeEnableLanceCollider();
        Vector3 forceToApply = orientation.forward * dashForce + orientation.up * dashUpwardForce;

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
            pm.drillingVfx.Stop();
            stopVfx = false;
        }
        pm.dashing = false;
        rb.useGravity = true;
        pa.DisableLanceCollider();
    }
}
