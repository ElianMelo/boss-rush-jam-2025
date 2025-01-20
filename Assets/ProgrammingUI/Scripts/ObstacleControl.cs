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

    public Mesh specialMesh;
    public Material specialMaterial;

    private Mesh originalMesh;
    private Material originalMaterial;
    private int originalLayer;

    private List<Transform> movedChildren = new List<Transform>();

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
            MoveRandomChildrenBack(top, 3);
            MoveRandomChildrenBack(mid, 3);
            MoveRandomChildrenBack(bot, 3);

            Transform specialChild = null;

            if (movedChildren.Count > 0)
            {
                specialChild = movedChildren[Random.Range(0, movedChildren.Count)];
                ChangeAppearance(specialChild.GetChild(0));
            }

            yield return new WaitForSeconds(interval_1);
                
            if (specialChild != null)
            {
                RestoreAppearance(specialChild.GetChild(0));
            }

            RestoreMovedChildren();

            yield return new WaitForSeconds(interval_2);
        }
    }

    private void RestoreMovedChildren()
    {
        foreach (Transform child in movedChildren)
        {
            // Move the child back to its original position
            StartCoroutine(SmoothMove(child.GetChild(0), -moveDistance, 1));
        }

        movedChildren.Clear();
    }

    private void MoveChildrenDown(Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.GetChild(0).Translate(-Vector3.forward * moveDistance, Space.Self);
        }
    }

    private void MoveRandomChildrenBack(Transform parent, int count)
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

            // Move the selected child forward along its local blue axis
            randomChild.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(DeactivateSecondChild(randomChild.GetChild(1), 2));
            StartCoroutine(SmoothMove(randomChild.GetChild(0), moveDistance, 3));

            movedChildren.Add(randomChild);

            children.RemoveAt(randomIndex);
        }
    }

    private IEnumerator SmoothMove(Transform obj, float distance, float duration)
    {
        Vector3 start = obj.localPosition;
        Vector3 end = start + obj.localRotation * (Vector3.forward * distance);

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

    private void ChangeAppearance(Transform obj)
    {
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        Renderer renderer = obj.GetComponent<Renderer>();
        int layerId = LayerMask.NameToLayer("ObstacleBroken");

        // Save original mesh and material
        if (meshFilter != null && renderer != null)
        {
            originalMesh = meshFilter.mesh;
            originalMaterial = renderer.material;
            originalLayer = obj.gameObject.layer;

            // Assign new mesh and material
            meshFilter.mesh = specialMesh;
            renderer.material = specialMaterial;
            obj.gameObject.layer = layerId;
        }
    }

    private void RestoreAppearance(Transform obj)
    {
        MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
        Renderer renderer = obj.GetComponent<Renderer>();

        if (meshFilter != null && renderer != null)
        {
            meshFilter.mesh = originalMesh;
            renderer.material = originalMaterial;
            obj.gameObject.layer = originalLayer;
        }
    }

    public void HandleObstacle(Collider obj)
    {
        if (movedChildren.Contains(obj.transform.parent.transform))
        {
            RestoreAppearance(obj.transform);
            StartCoroutine(SmoothMove(obj.transform, -moveDistance, 1));
            movedChildren.Remove(obj.transform.parent.transform);
        }
        else
        {
            Debug.Log("Object is not in the movedChildren list.");
        }
    }
}
