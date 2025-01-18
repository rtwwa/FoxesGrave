using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // Максимальное здоровье
    [SerializeField] private int damage = 10;    // Урон, который может нанести враг
    [SerializeField] private float movementSpeed = 3.0f; // Скорость передвижения

    private int currentHealth; // Текущее здоровье

    private void Start()
    {
        // Устанавливаем начальное здоровье равным максимальному
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void DealDamage(PlayerStats target)
    {
        if (target != null)
        {
            target.TakeDamage(damage);
            Debug.Log($"{gameObject.name} dealt {damage} damage to {target.name}");
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // Удаляем объект из сцены
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }
}
