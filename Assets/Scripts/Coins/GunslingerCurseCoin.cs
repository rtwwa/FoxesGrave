using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunslingerCurseCoin : MonoBehaviour, ICoin
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
            ICoin.GetRandomChoice();
        else
            ICoin.GetRandomChoice();

        Debug.Log("GunslingerCurseCoin used.");
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
}
