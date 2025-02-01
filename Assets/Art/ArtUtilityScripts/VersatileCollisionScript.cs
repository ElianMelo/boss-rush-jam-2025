using UnityEngine;
using UnityEngine.Events;

public class VersatileCollisionScript : MonoBehaviour
{
    [SerializeField] private string[] targetTags = { "Player" };

    [System.Serializable]
    public class CollisionEvent : UnityEvent<Collision> { }

    [System.Serializable]
    public class TriggerEvent : UnityEvent<Collider> { }

    [SerializeField] private CollisionEvent onCollisionEnterEvent;
    [SerializeField] private CollisionEvent onCollisionStayEvent;
    [SerializeField] private CollisionEvent onCollisionExitEvent;

    [SerializeField] private TriggerEvent onTriggerEnterEvent;
    [SerializeField] private TriggerEvent onTriggerStayEvent;
    [SerializeField] private TriggerEvent onTriggerExitEvent;

    private void OnCollisionEnter(Collision collision)
    {
        if (CheckCollisionTag(collision.gameObject.tag))
        {
            onCollisionEnterEvent.Invoke(collision);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (CheckCollisionTag(collision.gameObject.tag))
        {
            onCollisionStayEvent.Invoke(collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (CheckCollisionTag(collision.gameObject.tag))
        {
            onCollisionExitEvent.Invoke(collision);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CheckTriggerTag(other.tag))
        {
            onTriggerEnterEvent.Invoke(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (CheckTriggerTag(other.tag))
        {
            onTriggerStayEvent.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (CheckTriggerTag(other.tag))
        {
            onTriggerExitEvent.Invoke(other);
        }
    }

    private bool CheckCollisionTag(string tag)
    {
        foreach (string targetTag in targetTags)
        {
            if (tag == targetTag)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckTriggerTag(string tag)
    {
        foreach (string targetTag in targetTags)
        {
            if (tag == targetTag)
            {
                return true;
            }
        }
        return false;
    }
	
	public void DestroySelfAfterDelay(float delay)
    {
        Destroy(gameObject, delay);
    }
}

