using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDivingController : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;
    private PlayerMovementController pm;
    private PlayerAttackController pa;

    [Header("Diving")]
    public float diveForce;
    public float diveDuration;

    [Header("Cooldown")]
    public float diveCd;
    private float diveCdTimer;

    private bool stopVfx = false;

    [Header("Input")]
    public KeyCode diveKey = KeyCode.LeftControl;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementController>();
        pa = GetComponent<PlayerAttackController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(diveKey))
            Dive();
        if (diveCdTimer > 0)
            diveCdTimer -= Time.deltaTime;
    }

    private void Dive()
    {
        if (diveCdTimer > 0) return;
        else diveCdTimer = diveCd;
        if (pm.state == PlayerMovementController.MovementState.drilling) return;
        pm.diving = true;
        pm.CallDiveAnimation();
        pm.drillingVfx.Play();
        pa.UnsafeEnableLanceCollider();
        Vector3 forceToApply = Vector3.down * diveForce;

        delayedForceToApply = forceToApply;

        Invoke(nameof(DelayedDiveForce), 0.025f);
        Invoke(nameof(ResetDive), diveDuration);
    }

    private Vector3 delayedForceToApply;

    private void DelayedDiveForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    public void ForcedResetDive()
    {
        stopVfx = true;
        ResetDive();
    }

    private void ResetDive()
    {
        if (stopVfx || pm.state != PlayerMovementController.MovementState.drilling)
        {
            pm.drillingVfx.Stop();
            stopVfx = false;
        }
        pm.diving = false;
        pa.DisableLanceCollider();
    }
}