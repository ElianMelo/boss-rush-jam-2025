using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossEventListener : MonoBehaviour
{
    public UnityEvent OnBossTakeDamage;
    public UnityEvent OnBossDeath;
    public UnityEvent OnBossAttack;

    void Start()
    {
        BossManager.Instance.OnBossTakeDamage.AddListener(BossTakeDamageEvent);
        BossManager.Instance.OnBossDeath.AddListener(BossDeathEvent);
        BossManager.Instance.OnBossAttack.AddListener(BossAttackEvent);
    }

    private void OnDestroy()
    {
        BossManager.Instance.OnBossTakeDamage.RemoveListener(BossTakeDamageEvent);
        BossManager.Instance.OnBossDeath.RemoveListener(BossDeathEvent);
        BossManager.Instance.OnBossAttack.RemoveListener(BossAttackEvent);
    }

    private void BossTakeDamageEvent()
    {
        OnBossTakeDamage?.Invoke();
    }

    private void BossDeathEvent()
    {
        OnBossDeath?.Invoke();
    }

    private void BossAttackEvent()
    {
        OnBossAttack?.Invoke();
    }
}
