using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurtlemanTerminal : MonoBehaviour
{
    public List<UnityEvent> startEvents;
    public List<UnityEvent> deathEvents;

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
            foreach (var item in deathEvents)
            {
                item?.Invoke();
            }
        }
    }
}
