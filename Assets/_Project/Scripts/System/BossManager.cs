using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public Slider healthSlider;

    public static BossManager Instance;

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

    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(SmoothChangeHealth());
        // healthSlider.value = health / maxHealth;
    }

    private IEnumerator SmoothChangeHealth()
    {
        float currentTimer = 0;
        float timer = 4f;
        float target = health / maxHealth;
        while (currentTimer < timer)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, target, currentTimer / timer);
            currentTimer += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}
