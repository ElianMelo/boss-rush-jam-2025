using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public enemyControl enemyControlScript; // Reference to the enemyControl script on the parent

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyControlScript.playerNear = true;
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        enemyControlScript.playerNear = false;
    //    }
    //}
}
