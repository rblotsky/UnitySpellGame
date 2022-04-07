using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MerchantTradeUpdate
{
    // Data Structures //
    public enum MerchantUpdateType
    {
        Shuffle_Trades,
        Change_Sell_Data,
        Change_Buy_Data,
    }


    // DATA //
    public string merchantIdentifier;
    public MerchantUpdateType updateType;
    public MerchantTradeData tradeData;
    

    // FUNCTIONS //
    public void RunTradeUpdate()
    {
        if (updateType == MerchantUpdateType.Shuffle_Trades)
        {
            MerchantDataList.ResetTradeKey(merchantIdentifier);
        }

        else if (updateType == MerchantUpdateType.Change_Buy_Data)
        {
            if (tradeData == null)
            {
                Debug.LogError("[MerchantTradeUpdate] ChangeData update given null tradeData!");
            }

            else
            {
                MerchantDataList.GetMerchantData(merchantIdentifier).merchantBuyTrades = tradeData.id.ToString();
            }
        }

        else if (updateType == MerchantUpdateType.Change_Sell_Data)
        {
            if (tradeData == null)
            {
                Debug.LogError("[MerchantTradeUpdate] ChangeData update given null tradeData!");
            }

            else
            {
                MerchantDataList.GetMerchantData(merchantIdentifier).merchantSellTrades = tradeData.id.ToString();
            }
        }
    }
}
