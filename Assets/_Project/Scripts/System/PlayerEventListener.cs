using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEventListener : MonoBehaviour
{
    public UnityEvent OnPlayerTakeDamage;
    public UnityEvent OnPlayerDeath;
    public UnityEvent OnPlayerAttack;
    public UnityEvent OnPlayerJump;
    public UnityEvent OnStartDrilling;
    public UnityEvent OnStopDrilling;

    void Start()
    {
        PlayerManager.Instance.OnPlayerTakeDamage.AddListener(PlayerTakeDamageEvent);
        PlayerManager.Instance.OnPlayerDeath.AddListener(PlayerDeathEvent);
        PlayerManager.Instance.OnPlayerAttack.AddListener(PlayerAttackEvent);
        PlayerManager.Instance.OnPlayerJump.AddListener(PlayerJumpEvent);
        PlayerManager.Instance.OnStartDrilling.AddListener(PlayerStartDrilling);
        PlayerManager.Instance.OnStopDrilling.AddListener(PlayerStopDrilling);
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.OnPlayerTakeDamage.RemoveListener(PlayerTakeDamageEvent);
        PlayerManager.Instance.OnPlayerDeath.RemoveListener(PlayerDeathEvent);
        PlayerManager.Instance.OnPlayerAttack.RemoveListener(PlayerAttackEvent);
        PlayerManager.Instance.OnPlayerJump.RemoveListener(PlayerJumpEvent);
        PlayerManager.Instance.OnStartDrilling.RemoveListener(PlayerStartDrilling);
        PlayerManager.Instance.OnStopDrilling.RemoveListener(PlayerStopDrilling);
    }

    private void PlayerTakeDamageEvent()
    {
        OnPlayerTakeDamage?.Invoke();
    }

    private void PlayerDeathEvent()
    {
        OnPlayerDeath?.Invoke();
    }

    private void PlayerAttackEvent()
    {
        OnPlayerAttack?.Invoke();
    }

    private void PlayerJumpEvent()
    {
        OnPlayerJump?.Invoke();
    }

    private void PlayerStartDrilling()
    {
        OnStartDrilling?.Invoke();
    }

    private void PlayerStopDrilling()
    {
        OnStopDrilling?.Invoke();
    }
}
