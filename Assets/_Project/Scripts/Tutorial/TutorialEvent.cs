using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialEvent : MonoBehaviour
{
    public List<UnityEvent> events;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            foreach(UnityEvent e in events)
            {
                e?.Invoke();
            }
        }
    }

    public void GoNextLevel()
    {
        LevelManager.Instance.GoNextLevel();
    }
}
