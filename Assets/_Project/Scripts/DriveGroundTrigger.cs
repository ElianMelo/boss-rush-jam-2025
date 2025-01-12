using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DriveGroundTrigger : MonoBehaviour
{
    public Collider driveGroundSolid;
    public GameObject vfxDiggedVFX;
    
    private PlayerAttackController attackController;
    private Collider lastCheckPlayerCollider;

    public bool WeakPoint;

    private bool isFirstTime = true;
    private bool isWeakPoint = true;

    private float currentTimer = 0f;
    private float maxTimer;
    
    private void Start()
    {
        isWeakPoint = WeakPoint;
    }

    public bool GetWeakPoint()
    {
        return isWeakPoint;
    }

    public bool GetFirstTime()
    {
        return isFirstTime;
    }

    public void SetFirstTime(bool newIsFirstTime)
    {
        isFirstTime = newIsFirstTime;
    }

    public void SetAttackController(PlayerAttackController otherAttackController)
    {
        attackController = otherAttackController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Lance"))
        {
            lastCheckPlayerCollider = other;
            currentTimer = 0f;
            StartCoroutine(SafeCheck());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Lance"))
        {
            currentTimer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Player")) && attackController != null)
        {
            if (!attackController.IsDrilling) return;
            InstantiateVFX(other);
            attackController.StopDrilling(this);
            StopAllCoroutines();
        }
    }

    private IEnumerator SafeCheck()
    {
        maxTimer = 1f;
        while (currentTimer < maxTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        attackController.StopDrilling(this);
        yield return null;
    }

    public void InstantiateVFX(Collider other)
    {
        Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
        GameObject vfx = Instantiate(vfxDiggedVFX, contactPoint, Quaternion.identity);
        vfx.transform.LookAt(transform.parent.transform);
    }

    public void EnableColllider()
    {
        driveGroundSolid.enabled = true;
    }

    public void DisableCollider()
    {
        if(lastCheckPlayerCollider != null)
        {
            InstantiateVFX(lastCheckPlayerCollider);
        }
        driveGroundSolid.enabled = false;
    }

}
