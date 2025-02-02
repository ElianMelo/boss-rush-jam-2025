using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BossProjectile : MonoBehaviour
{
    public float speed;
    private Transform player;

    public UnityEvent OnProjectileSpawn;
    public UnityEvent OnProjectileExpire;
    public UnityEvent OnProjectileHit;

    void Start()
    {
        player = FindObjectOfType<PlayerMovementController>().transform;
        OnProjectileSpawn?.Invoke();
        StartCoroutine(ProjectileExpire());
    }

    void Update()
    {
        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Lance") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("DiveGround") || collision.gameObject.CompareTag("DiveGroundTrigger"))
        {
            if(collision.gameObject.CompareTag("Lance"))
            {
                GetComponent<Collider>().enabled = false;
            }
            OnProjectileHit?.Invoke();
            Destroy(gameObject, 0.5f);
        }
    }

    private IEnumerator ProjectileExpire()
    {
        yield return new WaitForSeconds(9.5f);
        OnProjectileExpire?.Invoke();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
