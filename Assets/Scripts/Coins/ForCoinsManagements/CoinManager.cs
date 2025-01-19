using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    private Dictionary<CoinType, CoinInfo> coins = new Dictionary<CoinType, CoinInfo>();

    [Header("Coin Models")]
    [SerializeField] private List<CoinModels> coinModels = new List<CoinModels>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one PlayerStats instance");
        }

        Instance = this;

        foreach (CoinType coinType in Enum.GetValues(typeof(CoinType)))
        {
            GameObject coinObject = new GameObject(coinType.ToString());
            coinObject.transform.parent = this.transform;

            GameObject modelPrefab = GetModelForCoin(coinType);
            if (modelPrefab != null)
            {
                GameObject modelInstance = Instantiate(modelPrefab, coinObject.transform);
                modelInstance.name = $"{coinType}_Model";
            }

            ICoin coinComponent = (ICoin)coinObject.AddComponent(GetCoinComponentForType(coinType));
            coins.Add(coinType, new CoinInfo(coinType, coinComponent, false)); // TODO поменять на false
        }
    }

    private System.Type GetCoinComponentForType(CoinType coinType)
    {
        switch (coinType)
        {
            case CoinType.SpiritFlip:
                return typeof(SpiritFlipCoin);
            case CoinType.LoversBlessing:
                return typeof(LoversBlessingCoin);
            case CoinType.SwordsMan:
                return typeof(SwordsManCoin);
            case CoinType.GunslingerCurse:
                return typeof(GunslingerCurseCoin);
            case CoinType.AngelsState:
                return typeof(AngelsStateCoin);
            default:
                throw new System.ArgumentOutOfRangeException($"No component assigned for {coinType}");
        }
    }

    public GameObject GetModelForCoin(CoinType coinType)
    {
        foreach (var coinModel in coinModels)
        {
            if (coinModel.coinType == coinType)
            {
                return coinModel.model;
            }
        }

        Debug.LogError($"No model found for {coinType}");
        return null;
    }

    public CoinInfo GetCoin(CoinType type)
    {
        return coins.GetValueOrDefault(type);
    }

    public void UnlockAbility(CoinType type)
    {
        var coin = GetCoin(type);
        if (coin != null)
        {
            coin.IsUnlocked = true;
            // Event по открытию монетки?
            Console.WriteLine($"Skill {type} unlocked!");
        }
    }

    public void UseAbility(CoinType type)
    {
        var coin = GetCoin(type);
        if (coin != null && coin.IsUnlocked)
        {
            coin.CoinAbility.UseAbility();
            //coin.CoinAbility.ShowAnimation(GetModelForCoin(type));
        }
        else
        {
            Console.WriteLine($"Skill {type} is not unlocked yet!");
        }
    }
}
