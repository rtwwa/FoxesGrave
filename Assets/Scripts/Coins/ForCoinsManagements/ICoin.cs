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
}
