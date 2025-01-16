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

    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private Animator animator;

    private bool isFollowingPlayer = false;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerMovementController>().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(CheckDistancePlayer());
    }

    private void FixedUpdate()
    {
        if(isFollowingPlayer)
            navMeshAgent.SetDestination(playerTransform.position);
    }

    private IEnumerator CheckDistancePlayer()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            if(Vector3.Distance(transform.position, playerTransform.position) < 20f)
            {
                isFollowingPlayer = true;
            }
            if (Vector3.Distance(transform.position, playerTransform.position) < 5f)
            {
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
        if (other.CompareTag("Lance"))
        {
            var hitVFXObject = Instantiate(hitVFX, transform.position + new Vector3(0f,2f,0f), Quaternion.identity);
            hitVFXObject.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f));
            var explosionVFXObject = Instantiate(explosionVFX, transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
            explosionVFXObject.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f));
            ScreenShakeManager.Instance.ShakeScreen();
            StopAllCoroutines();
            Destroy(gameObject, 0.5f);
        }
    }
}
