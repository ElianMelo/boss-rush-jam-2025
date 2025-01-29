using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControl : MonoBehaviour
{
    public Transform top;
    public Transform mid;
    public Transform bot;
    public float moveDistance = 5f;
    public float interval_1 = 30f;
    public float interval_2 = 2f;

    public GameObject obstaclePrefab;
    private GameObject newObstacle;

    //private List<Transform> movedChildren = new List<Transform>();
    private List<Transform> selectedChildren = new List<Transform>();

    void Start()
    {
        MoveChildrenDown(top);
        MoveChildrenDown(mid);
        MoveChildrenDown(bot);

        StartCoroutine(MoveAndRestoreCycle());
    }
    private IEnumerator MoveAndRestoreCycle()
    {
        while (true)
        {
            MoveRandomChildrenToList(top, 3);
            MoveRandomChildrenToList(mid, 3);
            MoveRandomChildrenToList(bot, 3);

            CallFunctionWithRandomChild();
            MoveSelectedChildrenBack();

            yield return new WaitForSeconds(interval_1);
            
            RestoreMovedChildren();

            yield return new WaitForSeconds(interval_2);
        }
    }

    private void RestoreMovedChildren()
    {
        foreach (Transform child in selectedChildren)
        {
            // Move the child back to its original position
            if (child.childCount >= 3)
            {
                StartCoroutine(SmoothMove(child.GetChild(2), -moveDistance, 1));
                Destroy(newObstacle, 2f);
            }
            else
            {
                StartCoroutine(SmoothMove(child.GetChild(0), -moveDistance, 1));
            }
        }
        selectedChildren.Clear();
    }

    private void MoveChildrenDown(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.GetChild(0).Translate(-Vector3.up * moveDistance, Space.Self);
        }
    }

    private void MoveSelectedChildrenBack()
    {
        foreach (Transform children in selectedChildren)
        {
            children.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(DeactivateSecondChild(children.GetChild(1), 2));

            if (children.childCount >= 3)
            {
                StartCoroutine(SmoothMove(children.GetChild(2), moveDistance, 3));
            }
            else
            {
                StartCoroutine(SmoothMove(children.GetChild(0), moveDistance, 3));
            }
        }
    }

    private void MoveRandomChildrenToList(Transform parent, int count)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in parent)
        {
            children.Add(child);
        }

        int childrenCount = Mathf.Min(count, children.Count);

        for (int i = 0; i < childrenCount; i++)
        {
            int randomIndex = Random.Range(0, children.Count);
            Transform randomChild = children[randomIndex];

            selectedChildren.Add(randomChild);
            children.RemoveAt(randomIndex);
        }
    }

    private void CallFunctionWithRandomChild()
    {
        if (selectedChildren.Count > 0)
        {
            int randomIndex = Random.Range(0, selectedChildren.Count);
            Transform randomChild = selectedChildren[randomIndex];

            InvokeBrokenObstacle(randomChild.GetChild(0));
        }
        else
        {
            Debug.LogWarning("Nenhum elemento disponível na lista para chamar a função.");
        }
    }

    private void InvokeBrokenObstacle(Transform child)
    {
        int layerId = LayerMask.NameToLayer("ObstacleBroken");

        if (obstaclePrefab != null)
        {
            newObstacle = Instantiate(obstaclePrefab, child.position, child.rotation);
            //newObstacle.transform.localScale = child.localScale;
            newObstacle.transform.SetParent(child.parent);
            newObstacle.layer = layerId;
        }
    }

    private IEnumerator SmoothMove(Transform obj, float distance, float duration)
    {
        Vector3 start = obj.localPosition;
        Vector3 end = start + obj.localRotation * (Vector3.up * distance);

        float elapsed = 0f;
        float shakeAmount = 0.3f; // Initial shake strength
        float shakeDecay = 0.05f;

        while (elapsed < duration)
        {
            obj.localPosition = Vector3.Lerp(start, end, elapsed / duration);

            // Add a shake effect to the object
            Vector3 shake = new Vector3(
                Random.Range(-shakeAmount, shakeAmount),
                Random.Range(-shakeAmount, shakeAmount),
                Random.Range(-shakeAmount, shakeAmount)
            );

            // Apply the shake effect
            obj.localPosition += shake;

            shakeAmount = Mathf.Max(shakeAmount - shakeDecay * Time.deltaTime, 0.02f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.localPosition = end;
    }

    private IEnumerator DeactivateSecondChild(Transform danger, float delay)
    {
        yield return new WaitForSeconds(delay);
        danger.gameObject.SetActive(false);
    }

    public void HandleObstacle(Collider obj)
    {
        if (selectedChildren.Contains(obj.transform.parent.transform))
        {
            StartCoroutine(SmoothMove(obj.transform, -moveDistance, 1));
            selectedChildren.Remove(obj.transform.parent.transform);
        }
        else
        {
            Debug.Log("Object is not in the movedChildren list.");
        }
    }
}
