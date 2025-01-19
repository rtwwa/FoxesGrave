
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritFlipCoin : MonoBehaviour, ICoin
{
    public bool IsCooldown()
    {
        return false;
    }
    public void UseAbility()
    {
        // ƒа тут пусто, потому что флип затрагивает половину класса PlayerMovement, € бы переписал, но это надолго
    }

    public void ShowAnimation(GameObject target)
    {
        throw new System.NotImplementedException();
    }
}
