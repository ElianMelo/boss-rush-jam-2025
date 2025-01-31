using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DriveGroundTrigger : MonoBehaviour
{
    public Collider driveGroundSolid;
    public GameObject vfxDiggedVFX;
    
    private PlayerAttackController attackController;
    private Collider lastCheckPlayerCollider;
    public WeakSpotEventListener weakSpotEventListener;

    public bool WeakPoint;
    public float WeakPointDamage = 25f;

    private bool isFirstTime = true;
    private bool isWeakPoint = true;

    private float currentTimer = 0f;
    private float maxTimer;

    private TurtleWeekSpot turtleWeekSpot;
    private PlayerAttackController playerAttackController;

    private void Awake()
    {
        turtleWeekSpot = GetComponent<TurtleWeekSpot>();
        playerAttackController = FindObjectOfType<PlayerAttackController>();
        SetAttackController(playerAttackController);
    }

    private void Start()
    {
        isWeakPoint = WeakPoint;
    }

    public bool GetWeakPoint()
    {
        return isWeakPoint;
    }

    public bool GetFirstTime()
    {
        return isFirstTime;
    }

    public void SetFirstTime(bool newIsFirstTime)
    {
        isFirstTime = newIsFirstTime;
    }

    public void SetAttackController(PlayerAttackController otherAttackController)
    {
        attackController = otherAttackController;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Lance"))
        {
            if(weakSpotEventListener != null && WeakPoint)
            {
                weakSpotEventListener.OnWeakSpotEnter?.Invoke();
            }
            lastCheckPlayerCollider = other;
            currentTimer = 0f;
            StopAllCoroutines();
            StartCoroutine(SafeCheck());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Lance"))
        {
            currentTimer = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // todo: get attack controller no matter what
        if ((other.CompareTag("Player")) && attackController != null)
        {
            if (weakSpotEventListener != null && WeakPoint)
            {
                weakSpotEventListener.OnWeakSpotExit?.Invoke();
            }
            if (!attackController.IsDrilling) return;
            InstantiateVFX(other);
            attackController.StopDrilling(this);
            StopAllCoroutines();
            if (turtleWeekSpot != null)
            {
                turtleWeekSpot.ExpelPlayer();
            }
        }
    }

    private IEnumerator SafeCheck()
    {
        maxTimer = 1f;
        while (currentTimer < maxTimer)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        attackController.StopDrilling(this);
        yield return null;
    }

    public void InstantiateVFX(Collider other)
    {
        Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
        GameObject vfx = Instantiate(vfxDiggedVFX, contactPoint, Quaternion.identity);
        vfx.transform.LookAt(transform.parent.transform);
    }

    public void EnableColllider()
    {
        driveGroundSolid.enabled = true;
    }

    public void DisableCollider()
    {
        if(lastCheckPlayerCollider != null)
        {
            InstantiateVFX(lastCheckPlayerCollider);
        }
        driveGroundSolid.enabled = false;
    }

}
