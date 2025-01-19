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
    private float repelForce = 5f;

    private void Awake()
    {
        targetLayer = LayerMask.NameToLayer("Enemy");
    }

    public void UseAbility()
    {
        if (IsCooldown())
        {
            Debug.Log("Ability is on cooldown.");
            return;
        }

        if (ICoin.GetRandomChoice())
            Repel();
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

        if (Physics.Raycast(origin + Vector3.up * 1f, direction, out hit, rayDistance, ~targetLayer))
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

        Vector3 finalPosition = PlayerMovement.Instance.transform.position + PlayerMovement.Instance.transform.TransformDirection(Vector3.back * 2f);
        StartCoroutine(KickbackPlayer(origin, finalPosition, 0.2f)); // Полет по диагонали
    }

    private IEnumerator KickbackPlayer(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 midPoint = (startPosition + endPosition) / 2 + Vector3.up * 0.5f;

        float collisionRadius = 0.5f; // Радиус проверки коллайдераыы
        LayerMask obstacleLayer = LayerMask.GetMask("Enemy"); // Убедитесь, что слой корректен

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = t * t * t * (5 - 2 * t); // Делает движение более естественным

            // Интерполяция с учётом средней точки
            Vector3 currentPosition = Vector3.Lerp(Vector3.Lerp(startPosition, midPoint, t), Vector3.Lerp(midPoint, endPosition, t), t);

            // Проверка на столкновение
            if (Physics.CheckSphere(currentPosition, collisionRadius, obstacleLayer))
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

    private IEnumerator Kickback(GameObject obj, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 midPoint = (startPosition + endPosition) / 2 + Vector3.up * 0.5f;

        float collisionRadius = 0.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = t * t * t * (5 - 2 * t); // Делает движение более естественным

            // Интерполяция с учётом средней точки
            Vector3 currentPosition = Vector3.Lerp(Vector3.Lerp(startPosition, midPoint, t), Vector3.Lerp(midPoint, endPosition, t), t);

            // Проверка на столкновение
            if (Physics.CheckSphere(currentPosition, collisionRadius, ~LayerMask.GetMask("Enemy")))
            {
                Debug.Log($"Obstacle detected at position {currentPosition}, stopping kickback.");
                Debug.DrawLine(currentPosition, currentPosition + Vector3.up * 15, Color.red, 100f);
                endPosition = currentPosition;
                yield break;
            }

            obj.transform.position = currentPosition;

            // Отладочная линия
            Debug.DrawLine(currentPosition, currentPosition + Vector3.up * 15, Color.green, 0.1f);

            yield return null;
        }

        obj.transform.position = endPosition;
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
            Vector3 directionToTarget = collider.transform.position - PlayerMovement.Instance.transform.position;
            float distance = directionToTarget.magnitude;

            if (distance > 0)
            {
                Vector3 repelDisplacement = directionToTarget.normalized * (repelForce / distance);

                Debug.Log($"Repelled {collider.name} with displacement {repelDisplacement}.");

                // Вызываем Kickback для каждого врага
                StartCoroutine(Kickback(collider.gameObject, collider.transform.position, collider.transform.position + repelDisplacement, 0.5f));
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
