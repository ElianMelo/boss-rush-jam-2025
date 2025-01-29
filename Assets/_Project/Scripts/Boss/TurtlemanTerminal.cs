using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurtlemanTerminal : MonoBehaviour
{
    public List<UnityEvent> startEvents;
    public List<UnityEvent> deathEvents;

    public GameObject deathVFX;

    private void Start()
    {
        StartCoroutine(DelayedCall());
    }

    private IEnumerator DelayedCall()
    {
        yield return new WaitForSeconds(2f);
        foreach (var item in startEvents)
        {
            item?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lance"))
        {
            var deathVFXInstance = Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(deathVFXInstance, 1.5f);
            Destroy(gameObject, 0.5f);
            foreach (var item in deathEvents)
            {
                item?.Invoke();
            }
        }
    }
}
