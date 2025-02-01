using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerBikeVFXController : MonoBehaviour
{
    [SerializeField] private GameObject SlashVfx;
    [SerializeField] private GameObject ShockVfx;
    [SerializeField] private Vector3 SlashVFXRotation;

    public void TriggerShockVFX(Transform parent)
    {
        Vector3 offset = parent.up * 2.0f;
        GameObject vfx = Instantiate(ShockVfx, parent.position + offset, parent.rotation, parent);
        Destroy(vfx, 0.5f);
    }

    public void TriggerSlashVFXDelayed(Vector3 position, Quaternion rotation, bool isRight, float delay)
    {
        StartCoroutine(TriggerSlashVfxCoroutine(position, rotation, isRight, delay));
    }

    private IEnumerator TriggerSlashVfxCoroutine(Vector3 position, Quaternion rotation, bool isRight, float delay)
    {
        yield return new WaitForSeconds(delay);
        //position += new Vector3(0f, 2f, 0f);
        Vector3 currentSlashVFXRotation = SlashVFXRotation;
        if (isRight)
        {
            currentSlashVFXRotation.z = 0;
        } else
        {
            currentSlashVFXRotation.z = 190;
        }
        GameObject vfx = Instantiate(SlashVfx, position, rotation * Quaternion.Euler(currentSlashVFXRotation));
        vfx.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        Destroy(vfx, 1f);
    }

}
