using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttackScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager.Instance.TakeDamage(10f);
        }
    }
}
