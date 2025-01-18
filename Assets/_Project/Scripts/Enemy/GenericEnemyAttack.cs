using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemyAttack : MonoBehaviour
{
    private GenericEnemy genericEnemy;

    private void Start()
    {
        genericEnemy = GetComponentInParent<GenericEnemy>();
    }

    public void EnableAttackCollider()
    {
        genericEnemy.EnableAttackCollider();
    }

    public void DisableAttackCollider()
    {
        genericEnemy.DisableAttackCollider();
    }
}
