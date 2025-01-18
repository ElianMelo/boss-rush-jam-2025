using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed;
    private Transform player;
    
    void Start()
    {
        player = FindObjectOfType<PlayerMovementController>().transform;
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lance") || other.CompareTag("Ground") 
            || other.CompareTag("DiveGround") || other.CompareTag("DiveGroundTrigger"))
        {
            Destroy(gameObject);
        }
    }
}
