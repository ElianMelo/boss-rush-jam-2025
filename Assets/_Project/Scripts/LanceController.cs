using UnityEngine;

public class LanceController : MonoBehaviour
{
    public LayerMask whatIsGround;
    public float rayRange;
    private PlayerAttackController attackController;
    private PlayerMovementController movementController;
    private Transform playerBaseTransform;

    private void Start()
    {
        attackController = GetComponentInParent<PlayerAttackController>();
        movementController = GetComponentInParent<PlayerMovementController>();
        playerBaseTransform = attackController.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DiveGroundTrigger"))
        {
            bool groundBelow = Physics.Raycast(playerBaseTransform.position, Vector3.down, rayRange, whatIsGround);
            if (groundBelow && movementController.state != PlayerMovementController.MovementState.diving)
            {
                return;
            }
            DriveGroundTrigger driveGroundTrigger = other.GetComponent<DriveGroundTrigger>();
            driveGroundTrigger.SetAttackController(attackController);
            attackController.StartDrilling(driveGroundTrigger);
        }
    }

}
