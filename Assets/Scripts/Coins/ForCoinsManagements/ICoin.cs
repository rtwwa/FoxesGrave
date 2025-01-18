using System;
using Unity.VisualScripting;
using UnityEngine;

public enum CoinType
{
    SpiritFlip,
    LoversBlessing,
    SwordsMan,
    GunslingerCurse,
    AngelsState,
}

public interface ICoin
{
    void UseAbility();
    void ShowAnimation(GameObject target);
    bool IsCooldown();

    static bool GetRandomChoice()
    {
        return UnityEngine.Random.Range(0f, 1f) < 0.5f;
    }
}
