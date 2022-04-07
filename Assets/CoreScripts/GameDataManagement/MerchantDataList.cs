using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class MerchantDataList
{
    // DATA //
    // Main Lists
    public static Dictionary<string, MerchantData> merchantDataTable = new Dictionary<string, MerchantData>();
    public static Dictionary<string, MerchantTradeData> tradeDataTable = new Dictionary<string, MerchantTradeData>();
    public static MerchantTradeData[] tradeDataMasterList;


    // FUNCTIONS //    
    // Updating Trade Keys
    public static void SetTradeKey(string ID, float key)
    {
        // If the given ID is in the dictionary, sets its key to new value.
        if (merchantDataTable.ContainsKey(ID))
        {
            merchantDataTable[ID].merchantKey = key;
        }

        // Otherwise, logs error.
        else
        {
            Debug.LogError("[MerchantDataList] SetTradeKey given nonexistent Merchant ID! \"" + ID + "\"");
        }
    }

    public static void ResetTradeKey(string ID)
    {
        SetTradeKey(ID, 0);
    }


    // Data Request
    public static float GetTradeKey(string merchantID)
    {
        // If the merchant key is in the key dictionary, tries getting its current key
        if (merchantDataTable.ContainsKey(merchantID))
        {
            // If the key is 0, generates a new one.
            if(merchantDataTable[merchantID].merchantKey == 0)
            {
                merchantDataTable[merchantID].merchantKey = GenerateTradeKey();
            }
        }

        // If the merchant is not in the key dictionary, throws error
        else
        {
            Debug.LogError("[MerchantDataList] Nonexistent merchant ID requested from dictionary: " + merchantID);
            return 0;
        }

        // Returns the trade key for this merchant ID. The ID is guaranteed to be in the dictionary.
        return merchantDataTable[merchantID].merchantKey;
    }

    public static MerchantData GetMerchantData(string merchantID)
    {
        MerchantData returnData = null;
        merchantDataTable.TryGetValue(merchantID, out returnData);
        return returnData;
    }

    public static void SaveMerchantData(MerchantData data)
    {
        if (merchantDataTable.ContainsKey(data.id))
        {
            merchantDataTable[data.id] = data;
        }

        else
        {
            merchantDataTable.Add(data.id, data);
        }
    }

    public static MerchantTradeData GetTradeData(string dataName)
    {
        MerchantTradeData returnData = null;
        tradeDataTable.TryGetValue(dataName, out returnData);
        return returnData;
    }


    // Trade Key Generation
    public static float GenerateTradeKey()
    {
        // Generates new key - float value from 1 to static value in merchant class.
        return Random.Range(1.0f, MerchantTradeData.PERCENT_PAIR_VALUE_RANGE_TOTAL);
    }


    // Data Management
    public static Queue<string> SaveDataList(Formatting format = Formatting.None)
    {
        // Creates line queue
        Queue<string> merchantDataLines = new Queue<string>();

        // Saves ID and key for every merchant ingame, separated by a comma
        foreach (string merchantID in merchantDataTable.Keys)
        {
            // Enqueues merchant ID with its associated key.
            merchantDataLines.Enqueue(JsonConvert.SerializeObject(merchantDataTable[merchantID], format));
        }

        return merchantDataLines;
    }

    public static void LoadDataList(string saveFileText)
    {
        // Gets lines as array
        string[] merchantDataLines = saveFileText.Split('\n');

        // Reads lines and adds items to merchant data dictionary
        foreach (string line in merchantDataLines)
        {
            MerchantData data = JsonConvert.DeserializeObject<MerchantData>(line.Trim());

            if (data != null)
            {
                if (!merchantDataTable.ContainsKey(data.id))
                {
                    merchantDataTable.Add(data.id, data);
                }

                // Otherwise, logs error
                else
                {
                    Debug.LogError("[MerchantDataList] LoadData given duplicate Merchant ID! \"" + data.id + "\"");
                }
            }
        }
    }


    // Setup and Reset
    public static void SetupList()
    {
        tradeDataMasterList = Resources.LoadAll<MerchantTradeData>("MerchantTrades");
        tradeDataTable.Clear();
        merchantDataTable.Clear();

        foreach (MerchantTradeData tradeData in tradeDataMasterList)
        {
            tradeDataTable.Add(tradeData.id.ToString(), tradeData);
        }
    }
}