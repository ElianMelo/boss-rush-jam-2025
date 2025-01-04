using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LanceController : MonoBehaviour
{
    private PlayerAttackController attackController;

    private void Start()
    {
        attackController = GetComponentInParent<PlayerAttackController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DiveGroundTrigger"))
        {
            DriveGroundTrigger driveGroundTrigger = other.GetComponent<DriveGroundTrigger>();
            driveGroundTrigger.SetAttackController(attackController);
            attackController.StartDrilling(driveGroundTrigger);
        }
    }

}
