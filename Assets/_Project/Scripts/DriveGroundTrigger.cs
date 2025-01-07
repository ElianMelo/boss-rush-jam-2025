using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveGroundTrigger : MonoBehaviour
{
    public Collider driveGroundSolid;
    public GameObject vfxDiggedVFX;
    private PlayerAttackController attackController;
    private float checkVfxTimer = .2f;

    public void SetAttackController(PlayerAttackController otherAttackController)
    {
        attackController = otherAttackController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CheckDrillingForVFX(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && attackController != null)
        {
            if (!attackController.IsDrilling) return;
            InstantiateVFX(other);
            attackController.StopDrilling(this);
        }
    }

    public IEnumerator CheckDrillingForVFX(Collider other)
    {
        float timer = 0;
        while(timer < checkVfxTimer)
        {
            timer += Time.deltaTime;
            if(attackController != null && attackController.IsDrilling)
            {
                InstantiateVFX(other);
                timer = checkVfxTimer;
            }
            yield return null;
        }
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
        StopAllCoroutines();
        driveGroundSolid.enabled = false;
    }

}
