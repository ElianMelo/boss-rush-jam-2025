using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitbox : MonoBehaviour
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
            Debug.Log("Player tomou 10 de dano (boss hitbox e tiro)");
            PlayerManager.Instance.TakeDamage(10f);
            playerAttack.receiveDamage();
        }
    }
}
