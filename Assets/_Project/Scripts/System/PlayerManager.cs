using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Slider healthSlider;

    public static PlayerManager Instance;

    public UnityEvent OnPlayerTakeDamage;
    public UnityEvent OnPlayerDeath;

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
        Debug.Log("TakeDamage?");
        health -= damage;
        if(health <= 0)
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
            OnPlayerDeath?.Invoke();
        }
        else
        {
            OnPlayerTakeDamage?.Invoke();
        }
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
