using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMovePlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Transform targetPosition;

    public void MovePlayerToPosition()
    {
        playerTransform.position = targetPosition.position;
    }
}
