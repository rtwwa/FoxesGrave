using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Player Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Abilities")]
    private Dictionary<CoinType, bool> abilities = new Dictionary<CoinType, bool>();


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one PlayerStats instance");
        }

        Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    #region Health Management
    // Метод для нанесения урона
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Метод для лечения игрока
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log($"Player healed by {amount}. Current Health: {currentHealth}");
    }

    // Метод смерти игрока
    private void Die()
    {
        Debug.Log("Player has died!");
        // Добавьте здесь логику для смерти игрока (например, рестарт уровня)
    }
    #endregion
}
