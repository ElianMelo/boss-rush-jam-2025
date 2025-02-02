using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    private bool isMoving = false;
    private Vector3 start;
    private Vector3 end;
    private float duration;
    private float elapsed;
    private float shakeAmount;
    private float shakeDecay = 0.05f;

    private PlayerAttack playerAttack;

    void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();
    }

    public void MoveObstacle(float distance, float duration)
    {
        if (isMoving)
            start = end;
        else
            start = transform.localPosition;

        this.duration = duration;
        end = start + transform.localRotation * (Vector3.up * distance);
        elapsed = 0f;
        shakeAmount = 0.3f;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / duration);

        transform.localPosition = Vector3.Lerp(start, end, t);

        // Apply shake effect
        Vector3 shake = new Vector3(
            Random.Range(-shakeAmount, shakeAmount),
            Random.Range(-shakeAmount, shakeAmount),
            Random.Range(-shakeAmount, shakeAmount)
        );

        transform.localPosition += shake;
        shakeAmount = Mathf.Max(shakeAmount - shakeDecay * Time.deltaTime, 0.02f);

        if (t >= 1f)
        {
            transform.localPosition = end;
            isMoving = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (HeadquartersMananger.Instance != null)
            {
                if (HeadquartersMananger.Instance.CurrentState != HeadquartersState.Walking) return;
            }
            Debug.Log("Player tomou 10 de dano (obstaculo)");
            PlayerManager.Instance.TakeDamage(5f);
            playerAttack.receiveDamage();
        }
    }
}
