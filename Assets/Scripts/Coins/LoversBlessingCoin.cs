using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ’ил или неу€звимость
public class LoversBlessingCoin : MonoBehaviour, ICoin
{
    private float cooldownDuration = 5f;
    private float lastUsedTime = -Mathf.Infinity;

    public void UseAbility()
    {
        if (IsCooldown())
        {
            Debug.Log("Ability is on cooldown.");
            return;
        }

        if (ICoin.GetRandomChoice())
            Heal();
        else
            Invincibility();

        lastUsedTime = Time.time;
    }

    public bool IsCooldown()
    {
        return Time.time - lastUsedTime < cooldownDuration;
    }

    public void ShowAnimation(GameObject target)
    {
        throw new System.NotImplementedException();
    }

    private void Heal()
    {
        PlayerStats.Instance.Heal(15);
    }

    private void Invincibility()
    {
        PlayerStats.Instance.ActivateShield();
    }
}
