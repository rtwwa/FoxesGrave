public class CoinInfo
{
    public CoinType Type { get; set; }
    public ICoin CoinAbility { get; set; }
    public bool IsUnlocked { get; set; }

    public CoinInfo(CoinType type, ICoin coinAbility, bool isUnlocked)
    {
        Type = type;
        CoinAbility = coinAbility;
        IsUnlocked = isUnlocked;
    }
}