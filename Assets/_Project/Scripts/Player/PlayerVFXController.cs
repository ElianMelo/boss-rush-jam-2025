using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerVFXController : MonoBehaviour
{
    [SerializeField] private ParticleSystem LeftBooster;
    [SerializeField] private ParticleSystem RightBooster;
    [SerializeField] private ParticleSystem DrillingVfx;
    [SerializeField] private ParticleSystem GroundDrillingVfx;
    [SerializeField] private GameObject SlashVfx;
    [SerializeField] private GameObject ShockVfx;
    [SerializeField] private Vector3 SlashVFXRotation;

    public void EnableBooster(bool keepBoosterActive = false)
    {
        if(keepBoosterActive)
        {
            StopAllCoroutines();
        }
        LeftBooster.gameObject.SetActive(true);
        RightBooster.gameObject.SetActive(true);
        LeftBooster.Play();
        RightBooster.Play();
    }

    public void DisableBooster()
    {
        LeftBooster.Stop();
        RightBooster.Stop();
    }

    public void EnableDrilling()
    {
        DrillingVfx.Play();
    }

    public void EnableGroundDrillingVfx()
    {
        GroundDrillingVfx.Play();
    }

    public void DisableDrilling()
    {
        DrillingVfx.Stop();
    }

    public void DisableGroundDrillingVfx()
    {
        GroundDrillingVfx.Stop();
    }

    public void TriggerShockVFX(Transform parent)
    {
        GameObject vfx = Instantiate(ShockVfx, parent);
        vfx.transform.SetParent(parent);
        Destroy(vfx, 1f);
    }

    public void TriggerSlashVFXDelayed(Vector3 position, Quaternion rotation, bool isRight, float delay)
    {
        StartCoroutine(TriggerSlashVfxCoroutine(position, rotation, isRight, delay));
    }

    private IEnumerator TriggerSlashVfxCoroutine(Vector3 position, Quaternion rotation, bool isRight, float delay)
    {
        yield return new WaitForSeconds(delay);
        position += new Vector3(0f, 0.6f, 0f);
        Vector3 currentSlashVFXRotation = SlashVFXRotation;
        if (isRight)
        {
            currentSlashVFXRotation.z = 230;
        } else
        {
            currentSlashVFXRotation.z = 190;
        }
        GameObject vfx = Instantiate(SlashVfx, position, rotation * Quaternion.Euler(currentSlashVFXRotation));
        Destroy(vfx, 1f);
    }

    public void DisableBoosterDelayed(float time)
    {
        StartCoroutine(DisableBoosterDelayedCoroutine(time));
    }

    private IEnumerator DisableBoosterDelayedCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        DisableBooster();
    }

}
