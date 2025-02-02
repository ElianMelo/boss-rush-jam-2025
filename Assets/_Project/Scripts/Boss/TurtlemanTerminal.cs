using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurtlemanTerminal : MonoBehaviour
{
    public List<UnityEvent> deathEvents;
    public List<UnityEvent> delayedDeathvents;

    public UnityEvent OnHitEvent;
    public UnityEvent OnDeathEvent;

    public GameObject deathVFX;

    private IEnumerator DelayedCall()
    {
        yield return new WaitForSeconds(2f);
        OnDeathEvent?.Invoke();
        foreach (var item in delayedDeathvents)
        {
            item?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lance"))
        {
            OnHitEvent?.Invoke();
            var deathVFXInstance = Instantiate(deathVFX, transform.position, Quaternion.identity);
            foreach (var item in deathEvents)
            {
                item?.Invoke();
            }
            Destroy(deathVFXInstance, 1.5f);
            Destroy(gameObject, 2.5f);
            StartCoroutine(DelayedCall());
        }
    }
}
