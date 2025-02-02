using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthInterfaceManager : MonoBehaviour
{
    public static HealthInterfaceManager Instance;

    public UnityEvent OnPlayerTakeDamage;
    public UnityEvent OnBossTakeDamage;

    public Image jumpChargeLeft;
    public Image jumpChargeRight;
    public Image dashCooldownIcon;
    public TMP_Text dashCooldownText;
    public TMP_Text bossName;
    public TMP_Text bossTitle;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerManager.Instance.OnPlayerTakeDamage.AddListener(PlayerTakeDamage);
        BossManager.Instance.OnBossTakeDamage.AddListener(BossTakeDamage);
        switch (LevelManager.Instance.CurrentLevel) {
            case LevelManager.Level.Treeman:
                bossName.text = "Treeman";
                bossTitle.text = "The Rotten Elder";
                break;
            case LevelManager.Level.Bikerman:
                bossName.text = "Bikerman";
                bossTitle.text = "The Madness Showman";
                break;
            case LevelManager.Level.Turtleman:
                bossName.text = "Turtleman";
                bossTitle.text = "The Metallic Beast";
                break;
        }
    }

    public void SetAmountOfJumps(int jumps)
    {
        if(jumps == 0) {
            jumpChargeLeft.gameObject.SetActive(false);
            jumpChargeRight.gameObject.SetActive(false);
        } else if(jumps == 1) {
            jumpChargeLeft.gameObject.SetActive(true);
            jumpChargeRight.gameObject.SetActive(false);
        } else if (jumps == 2) {
            jumpChargeLeft.gameObject.SetActive(true);
            jumpChargeRight.gameObject.SetActive(true);
        }
    }

    public void DashCooldown(float cooldownTimer)
    {
        StartCoroutine(DashRecoverCoroutine(cooldownTimer));
    }

    private void PlayerTakeDamage()
    {
        OnPlayerTakeDamage?.Invoke();
    }

    private void BossTakeDamage()
    {
        OnBossTakeDamage?.Invoke();
    }

    private IEnumerator DashRecoverCoroutine(float cooldownTimer)
    {
        float currentTime = 0;
        dashCooldownText.gameObject.SetActive(true);
        while (currentTime < cooldownTimer)
        {
            dashCooldownIcon.fillAmount = currentTime / cooldownTimer;
            dashCooldownText.text = (1 - (currentTime / cooldownTimer)).ToString("0.0");
            currentTime += Time.deltaTime;
            yield return null;
        }
        dashCooldownIcon.fillAmount = 1f;
        dashCooldownText.gameObject.SetActive(false);
    }
}
