using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveGroundTrigger : MonoBehaviour
{
    public Collider driveGroundSolid;
    public GameObject vfxDiggedVFX;
    private PlayerAttackController attackController;
    private Collider lastCheckPlayerCollider;

    public void SetAttackController(PlayerAttackController otherAttackController)
    {
        attackController = otherAttackController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lastCheckPlayerCollider = other;
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
        if(lastCheckPlayerCollider != null)
        {
            InstantiateVFX(lastCheckPlayerCollider);
        }
        driveGroundSolid.enabled = false;
    }

}
