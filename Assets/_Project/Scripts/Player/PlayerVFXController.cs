using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXController : MonoBehaviour
{
    [SerializeField] private ParticleSystem LeftBooster;
    [SerializeField] private ParticleSystem RightBooster;
    [SerializeField] private ParticleSystem DrillingVfx;

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

    public void DisableDrilling()
    {
        DrillingVfx.Stop();
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
