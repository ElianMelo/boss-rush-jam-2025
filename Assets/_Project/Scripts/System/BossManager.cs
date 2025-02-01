using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public Image healthSlider;

    public static BossManager Instance;

    [HideInInspector]
    public bool IsDead = false;

    public UnityEvent OnBossTakeDamage;
    public UnityEvent OnBossDeath;
    public UnityEvent OnBossAttack;

    private float maxHealth = 100;
    private float health;

    private IEnumerator SmoothChangeHealthCoroutine;
    private bool coroutineIsRunning = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        health = maxHealth;
    }

    public void Attack()
    {
        OnBossAttack?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        if (coroutineIsRunning && SmoothChangeHealthCoroutine != null)
        {
            healthSlider.fillAmount = health / maxHealth;
            StopCoroutine(SmoothChangeHealthCoroutine);
        }
        health -= damage;
        if (health <= 0)
        {
            OnBossDeath?.Invoke();
            IsDead = true;
            StartCoroutine(DelayedDeath());
        } else
        {
            OnBossTakeDamage?.Invoke();
        }
        SmoothChangeHealthCoroutine = SmoothChangeHealth();
        StartCoroutine(SmoothChangeHealthCoroutine);
    }

    private IEnumerator DelayedDeath()
    {
        yield return new WaitForSeconds(5f);
        IsDead = false;
        LevelManager.Instance.GoNextLevel();
    }

    private IEnumerator SmoothChangeHealth()
    {
        float currentTimer = 0;
        coroutineIsRunning = true;
        float timer = 4f;
        float target = health / maxHealth;
        while (currentTimer < timer)
        {
            healthSlider.fillAmount = Mathf.Lerp(healthSlider.fillAmount, target, currentTimer / timer);
            currentTimer += Time.deltaTime;
            yield return null;
        }
        coroutineIsRunning = false;
        yield return null;
    }
}
