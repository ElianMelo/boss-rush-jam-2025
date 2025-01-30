using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAttackScript : MonoBehaviour
{
    private PlayerAttack playerAttack;

    void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player tomou 10 de dano (inimigos)");
            PlayerManager.Instance.TakeDamage(10f);
            playerAttack.receiveDamage();
        }
    }
}
