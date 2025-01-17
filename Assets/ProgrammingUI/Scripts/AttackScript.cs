using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public ObstacleControl obstacleScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }

        int obstacleLayer = LayerMask.NameToLayer("ObstacleBroken");
        if (other.gameObject.layer == obstacleLayer)
        {
            obstacleScript.HandleObstacle(other); // Call the function
        }
    }
}
