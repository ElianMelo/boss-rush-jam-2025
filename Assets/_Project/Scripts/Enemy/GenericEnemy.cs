using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemy : MonoBehaviour
{
    public GameObject explosionVFX;
    public GameObject hitVFX;

    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerMovementController>().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        navMeshAgent.SetDestination(playerTransform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lance"))
        {
            var hitVFXObject = Instantiate(hitVFX, transform.position, Quaternion.identity);
            hitVFXObject.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f));
            var explosionVFXObject = Instantiate(explosionVFX, transform.position, Quaternion.identity);
            explosionVFXObject.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f));
            ScreenShakeManager.Instance.ShakeScreen();
            Destroy(gameObject, 0.5f);
        }
    }
}
