using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttackScript : MonoBehaviour
{
    private PlayerAttack playerAttack;
    private enemyControl curEnemyControl;

    void Start()
    {
        curEnemyControl = GetComponentInParent<enemyControl>();
        playerAttack = FindObjectOfType<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HeadquartersMananger.Instance != null)
            {
                if (HeadquartersMananger.Instance.CurrentState != HeadquartersState.Walking) return;
            }
            curEnemyControl.DealDamage();
            Debug.Log("Player tomou 10 de dano (inimigos)");
            PlayerManager.Instance.TakeDamage(10f);
            playerAttack.receiveDamage();
        }
    }
}
