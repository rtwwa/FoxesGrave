using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunslingerCurseCoin : MonoBehaviour, ICoin
{
    private float cooldownDuration = 0f;
    private float lastUsedTime = -Mathf.Infinity;

    private float rayDistance = 100f;
    private int damage = 50;
    private LayerMask targetLayer;
    private float repelRadius = 5f;
    private float repelForce = 15f;

    private void Awake()
    {
        targetLayer = LayerMask.GetMask("Enemy");
    }

    public void UseAbility()
    {
        if (IsCooldown())
        {
            Debug.Log("Ability is on cooldown.");
            return;
        }

        if (ICoin.GetRandomChoice())
            Shoot();
        else
            Repel();

        Debug.Log("GunslingerCurseCoin used.");
        lastUsedTime = Time.time;
    }

    private void Shoot()
    {
        Vector3 origin = PlayerMovement.Instance.transform.position;
        Vector3 direction = PlayerMovement.Instance.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(origin + Vector3.up * 1f, direction, out hit, rayDistance, targetLayer))
        {
            Debug.Log($"Hit {hit.collider.name} at distance {hit.distance}");

            // Попадание в объект, нанесение урона
            EnemyStats targetHealth = hit.collider.GetComponent<EnemyStats>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
                Debug.Log($"Dealt {damage} damage to {hit.collider.name}");
            }
        }
        else
        {
            Debug.Log("Missed. No target hit.");
        }

        // Перемещение игрока с использованием Rigidbody
        Rigidbody rb = PlayerMovement.Instance.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Направление отталкивания (в противоположную сторону от выстрела)
            Vector3 knockbackDirection = PlayerMovement.Instance.transform.TransformDirection(Vector3.back);

            // Применение силы отталкивания к Rigidbody игрока
            rb.AddForce(knockbackDirection * 10f, ForceMode.Impulse); // 10f - сила отталкивания, можно подстроить
        }
        else
        {
            Debug.LogWarning("Player does not have a Rigidbody component!");
        }
    }

    private IEnumerator KickbackPlayer(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 midPoint = (startPosition + endPosition) / 2 + Vector3.up * 1f;

        float collisionRadius = 0.5f; // Радиус проверки коллайдераыы

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = t * t * t * (5 - 2 * t); // Делает движение более естественным

            // Интерполяция с учётом средней точки
            Vector3 currentPosition = Vector3.Lerp(Vector3.Lerp(startPosition, midPoint, t), Vector3.Lerp(midPoint, endPosition, t), t);

            // Проверка на столкновение
            if (Physics.CheckSphere(currentPosition, collisionRadius, ~LayerMask.GetMask("Player")))
            {
                Debug.Log($"Obstacle detected at position {currentPosition}, stopping kickback.");
                Debug.DrawLine(currentPosition, currentPosition + Vector3.up * 2, Color.red, 100f);
                endPosition = currentPosition;
                yield break;
            }

            PlayerMovement.Instance.transform.position = currentPosition;

            // Отладочная линия
            Debug.DrawLine(currentPosition, currentPosition + Vector3.up * 2, Color.green, 0.1f);

            yield return null;
        }

        PlayerMovement.Instance.transform.position = endPosition;
    }
    private void Repel()
    {
        Debug.Log("Repelling enemies within a cylinder...");

        Collider[] hitColliders = Physics.OverlapCapsule(
            PlayerMovement.Instance.transform.position + Vector3.down * repelRadius / 2,
            PlayerMovement.Instance.transform.position + Vector3.up * repelRadius / 2,
            repelRadius,
            LayerMask.GetMask("Enemy"));

        foreach (Collider collider in hitColliders)
        {
            Debug.Log($"Repelled {collider.name}");
            Rigidbody enemyRigidbody = collider.GetComponent<Rigidbody>();

            if (enemyRigidbody != null)
            {
                Vector3 directionToTarget = collider.transform.position - PlayerMovement.Instance.transform.position;
                float distance = directionToTarget.magnitude;

                if (distance > 0)
                {
                    Vector3 repelForceVector = directionToTarget.normalized * (repelForce / distance);

                    Debug.Log($"Applying force {repelForceVector} to {collider.name}.");
                    enemyRigidbody.AddForce(repelForceVector, ForceMode.Impulse);
                }
            }
            else
            {
                Debug.LogWarning($"No Rigidbody found on {collider.name}. Unable to repel.");
            }
        }
    }

    public bool IsCooldown()
    {
        return Time.time - lastUsedTime < cooldownDuration;
    }
    public void ShowAnimation(GameObject target)
    {
        throw new System.NotImplementedException();
    }
}
