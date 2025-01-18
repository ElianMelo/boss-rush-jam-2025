using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreemanBoss : MonoBehaviour
{
    public GameObject projectile;
    public Transform attackStartPosition;
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(RandomAttack());
    }

    private IEnumerator RandomAttack()
    {
        yield return new WaitForSeconds(Random.Range(5f, 10f));

        var coin = Random.Range(0, 2);
        if(coin == 0)
        {
            animator.SetTrigger("CastA");
        } else
        {
            animator.SetTrigger("CastB");
        }
        Instantiate(projectile, attackStartPosition);
        StartCoroutine(RandomAttack());
    }
}
