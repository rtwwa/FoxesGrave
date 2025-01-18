using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Player Stats")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    private bool canTakeDamage;

    [Header("Abilities")]
    private Dictionary<CoinType, bool> abilities = new Dictionary<CoinType, bool>();

    [Header("Shield Effect")]
    [SerializeField] private GameObject shieldEffect;

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

    public void ActivateShield()
    {
        if (canTakeDamage)
        {
            canTakeDamage = false;
            shieldEffect.SetActive(true);

            StartCoroutine(ShieldTimer());
        }
    }

    private IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(3f);

        shieldEffect.SetActive(false);

        canTakeDamage = true;
    }

    #region Health Management
    // Метод для нанесения урона
    public void TakeDamage(int amount)
    {
        if (canTakeDamage)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            Debug.Log("Player took damage. Current health: " + currentHealth);
        }
        else
        {
            Debug.Log("Player is shielded and can't take damage.");
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
