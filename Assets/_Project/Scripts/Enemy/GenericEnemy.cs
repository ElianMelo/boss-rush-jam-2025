using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemy : MonoBehaviour
{
    public GameObject explosionVFX;
    public GameObject hitVFX;
    public Collider attackCollider;
    public float deathForce;

    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private Rigidbody enemyRigidbody;
    private Animator animator;
    private bool canAttack = true;

    private bool isFollowingPlayer = false;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerMovementController>().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyRigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(CheckDistancePlayer());
    }

    private void FixedUpdate()
    {
        if (HeadquartersMananger.Instance != null)
        {
            if (HeadquartersMananger.Instance.CurrentState != HeadquartersState.Walking)
            {
                navMeshAgent.SetDestination(transform.position);
                return;
            }
        }
        if (isFollowingPlayer)
            navMeshAgent.SetDestination(playerTransform.position);
    }

    private IEnumerator CheckDistancePlayer()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            if (HeadquartersMananger.Instance != null)
            {
                if (HeadquartersMananger.Instance.CurrentState != HeadquartersState.Walking) continue;
            }
            if (Vector3.Distance(transform.position, playerTransform.position) < 20f)
            {
                isFollowingPlayer = true;
            }
            if(!canAttack)
            {
                canAttack = true;
                continue;
            }
            if (Vector3.Distance(transform.position, playerTransform.position) < 5f && canAttack)
            {
                canAttack = false;
                Attack();
            }
        }
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            DisableAttackCollider();
        }
        if (other.CompareTag("Lance"))
        {
            DisableAttackCollider();
            StopAllCoroutines();
            enemyRigidbody.AddForce((transform.position - playerTransform.position).normalized * deathForce, ForceMode.Impulse);
            var hitVFXObject = Instantiate(hitVFX, transform.position + new Vector3(0f,2f,0f), Quaternion.identity);
            hitVFXObject.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f));
            var explosionVFXObject = Instantiate(explosionVFX, transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
            explosionVFXObject.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f));
            ScreenShakeManager.Instance.ShakeScreen();
            Destroy(gameObject, 0.5f);
        }
    }
}
