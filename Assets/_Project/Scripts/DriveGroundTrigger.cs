using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveGroundTrigger : MonoBehaviour
{
    public Collider driveGroundSolid;
    private PlayerAttackController attackController;

    public void SetAttackController(PlayerAttackController otherAttackController)
    {
        attackController = otherAttackController;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && attackController != null)
        {
            attackController.StopDrilling(this);
        }
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
