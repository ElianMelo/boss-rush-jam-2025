using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurtleWeekSpot : MonoBehaviour
{
    public Vector3 turtlePosition;
    public Vector3 turtleRotation;
    public Vector3 playerPosition;
    public List<GameObject> toActivate;
    public List<GameObject> toDeactivate;

    public List<UnityEvent> unityEvents;

    public Transform turtleTransform;
    public Transform playerTransform;

    public void SetupScene()
    {
        playerTransform.position = playerPosition;

        turtleTransform.position = turtlePosition;
        turtleTransform.rotation = Quaternion.Euler(turtleRotation);

        foreach (var item in toActivate)
        {
            item.SetActive(true);
        }
        foreach (var item in unityEvents)
        {
            item?.Invoke();
        }
        foreach (var item in toDeactivate)
        {
            item.SetActive(false);
        }
    }
}
