using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Image healthSlider;

    public static PlayerManager Instance;

    public UnityEvent OnPlayerTakeDamage;
    public UnityEvent OnPlayerDeath;

    public UnityEvent OnPlayerAttack;
    public UnityEvent OnPlayerJump;

    public UnityEvent OnStartDrilling;
    public UnityEvent OnStopDrilling;

    private float maxHealth = 100;
    private float health;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        health = maxHealth;
    }

    public void StartDrilling()
    {
        OnStartDrilling?.Invoke();
    }

    public void StopDrilling()
    {
        OnStopDrilling?.Invoke();
    }

    public void PlayerAttackEvent()
    {
        OnPlayerAttack?.Invoke();
    }

    public void PlayerJumpEvent()
    {
        OnPlayerJump?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            OnPlayerDeath?.Invoke();
            StartCoroutine(DelayedDeath());
        }
        else
        {
            OnPlayerTakeDamage?.Invoke();
        }
        StartCoroutine(SmoothChangeHealth());
        // healthSlider.value = health / maxHealth;
    }
    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(3f);
        LevelManager.Instance.ResetCurrentLevel();
    }

    private IEnumerator SmoothChangeHealth()
    {
        float currentTimer = 0;
        float timer = 4f;
        float target = health / maxHealth;
        while (currentTimer < timer)
        {
            healthSlider.fillAmount = Mathf.Lerp(healthSlider.fillAmount, target, currentTimer / timer);
            currentTimer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
