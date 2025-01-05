using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private PlayerMovementController playerMovementController;
    public Collider lanceCollider;

    private void Start()
    {
        playerMovementController = GetComponent<PlayerMovementController>();
    }

    public void StartDrilling(DriveGroundTrigger driveGroundTrigger)
    {
        driveGroundTrigger.DisableCollider();
        playerMovementController.StartDrilling(driveGroundTrigger.transform);
    }

    public void StopDrilling(DriveGroundTrigger driveGroundTrigger)
    {
        driveGroundTrigger.EnableColllider();
        playerMovementController.StopDrilling();
    }

    public void EnableLanceCollider()
    {
        lanceCollider.enabled = true;
    }

    public void DisableLanceCollider()
    {
        lanceCollider.enabled = false;
    }
}