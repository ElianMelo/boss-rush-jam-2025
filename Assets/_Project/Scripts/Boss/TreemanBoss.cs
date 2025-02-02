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
        BossManager.Instance.OnBossTakeDamage.AddListener(TakeDamage);
        BossManager.Instance.OnBossDeath.AddListener(Death);
        StartCoroutine(CallProvocationsLines());
    }

    private IEnumerator CallProvocationsLines()
    {
        var randomIntervalProvocation = Random.Range(20, 30);
        yield return new WaitForSeconds(randomIntervalProvocation);
        SoundManager.Instance.PlayTreemanProvocationSound();
        StartCoroutine(CallProvocationsLines());
    }

    private void OnDestroy()
    {
        BossManager.Instance.OnBossTakeDamage.RemoveListener(TakeDamage);
        BossManager.Instance.OnBossDeath.RemoveListener(Death);
    }

    private IEnumerator RandomAttack()
    {
        yield return new WaitForSeconds(Random.Range(10f, 20f));

        if (HeadquartersMananger.Instance != null)
        {
            if (HeadquartersMananger.Instance.CurrentState == HeadquartersState.Walking)
            {
                var coin = Random.Range(0, 2);
                if (coin == 0)
                {
                    animator.SetTrigger("CastA");
                }
                else
                {
                    animator.SetTrigger("CastB");
                }
                Instantiate(projectile, attackStartPosition);
            }
        }
        StartCoroutine(RandomAttack());
    }

    private void TakeDamage()
    {
        animator.SetTrigger("Hurt");
    }

    private void Death()
    {
        animator.SetTrigger("Death");
    }
}
