using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    // Public UnityEvent that can be assigned in the inspector
    public UnityEvent onAnimationEvent;

    // Method to be called from the animation event
    public void TriggerEvent()
    {
        // Invoke the UnityEvent
        onAnimationEvent.Invoke();
    }
}