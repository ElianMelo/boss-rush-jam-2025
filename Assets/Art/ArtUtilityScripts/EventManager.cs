using UnityEngine;
using UnityEngine.Events;
using System;

public class EventManager : MonoBehaviour
{
    [Serializable]
    public class EventData
    {
        public string eventName;
        public UnityEvent eventAction;
    }

    [Serializable]
    public class UnityEvent : UnityEngine.Events.UnityEvent { }

    public EventData[] events;

    private void Start()
    {
        // You can call this function at any point in your game to trigger events
        // For example, TriggerEvent("EventName");
    }

    public void TriggerEvent(string eventName)
    {
        EventData eventData = Array.Find(events, e => e.eventName == eventName);

        if (eventData != null)
        {
            Debug.Log("Event triggered: " + eventName);
            eventData.eventAction.Invoke();
        }
        else
        {
            Debug.LogWarning($"Event '{eventName}' not found!");
        }
    }
}
