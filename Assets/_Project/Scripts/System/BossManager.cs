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
        healthSlider.value = health / maxHealth;
    }
}
