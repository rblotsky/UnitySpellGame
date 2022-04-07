// Data Structures //
[System.Serializable]
public class MerchantData
{
    public string id;
    public float merchantKey;
    public string merchantBuyTrades;
    public string merchantSellTrades;

    public MerchantData(string identifier, float key, string buyTrades, string sellTrades)
    {
        id = identifier;
        merchantKey = key;
        merchantBuyTrades = buyTrades;
        merchantSellTrades = sellTrades;
    }
}