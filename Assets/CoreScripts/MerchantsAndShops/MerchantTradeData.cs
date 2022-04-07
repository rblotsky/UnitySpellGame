using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Merchant Trade Data", menuName = "Merchant Trade Data")]
public class MerchantTradeData : GameResource
{
    // DATA //
    // Trades
    public PercentItemTypePair[] itemTypePairs;
    public PercentUsablePair[] usablePairs;

    // Constants
    public static readonly float PERCENT_PAIR_VALUE_RANGE_TOTAL = 1000;


    // FUNCTIONS //
    // Getting Trades
    public Usable[] GetTrades(float key)
    {
        // Creates a list of buy trades
        List<Usable> trades = new List<Usable>();

        // Gets the items according to key and ranges
        foreach (PercentUsablePair usablePair in usablePairs)
        {
            // If the key is within the min/max values of the percentusablepair range, adds that item
            if (usablePair.rangeMinValue <= key && usablePair.rangeMaxValue >= key)
            {
                trades.Add(usablePair.pairedObject);
            }
        }


        foreach (PercentItemTypePair itemTypePair in itemTypePairs)
        {
            // If the key is within the min/max values of the percentitemtypepair range, adds a random item of that type (using key to select from list) to the list.
            if (itemTypePair.rangeMinValue <= key && itemTypePair.rangeMaxValue >= key)
            {
                // Gets items of the given type
                Usable[] itemsOfType = UsableList.GetItems(itemTypePair.pairedObject);

                // Algorithm to get random selection (converts key to range from 0-last index, rounds to int, and uses that as index of item to select)
                float keyRangeConversion = ((itemsOfType.Length - 1) / PERCENT_PAIR_VALUE_RANGE_TOTAL);
                trades.Add(itemsOfType[Mathf.RoundToInt(key * keyRangeConversion)]);
            }
        }

        // Returns current trades according to seed
        return trades.ToArray();
    }


    // Utility Functions
    private void SetPercentObjectPairRange(PercentItemTypePair percentTypePair)
    {
        // Gets the amount in each direction from the root value the range will go
        float separationFromRoot = percentTypePair.percentValue * (PERCENT_PAIR_VALUE_RANGE_TOTAL / 100) / 2;

        // Gets the root value (so separationFromRoot will never go above 1000 or below 1)
        float rootValue = UnityEngine.Random.Range(1.0f + separationFromRoot, PERCENT_PAIR_VALUE_RANGE_TOTAL - separationFromRoot);

        // Sets the min and max values
        percentTypePair.rangeMinValue = rootValue - separationFromRoot;
        percentTypePair.rangeMaxValue = rootValue + separationFromRoot;
    }

    private void SetPercentObjectPairRange(PercentUsablePair percentUsablePair)
    {
        // Gets the amount in each direction from the root value the range will go
        float separationFromRoot = percentUsablePair.percentValue * (PERCENT_PAIR_VALUE_RANGE_TOTAL / 100) / 2;

        // Gets the root value (so separationFromRoot will never go above 1000 or below 1)
        float rootValue = UnityEngine.Random.Range(1.0f + separationFromRoot, 1000.0f - separationFromRoot);

        // Sets the min and max values
        percentUsablePair.rangeMinValue = rootValue - separationFromRoot;
        percentUsablePair.rangeMaxValue = rootValue + separationFromRoot;
    }


    // Editor Functions
    [ContextMenu("Generate Selection Ranges")]
    public void GenerateSelectionRanges()
    {
        // Sets selection ranges for each buy listing, if it's still default value (equal to each other)
        foreach (PercentItemTypePair percentTypePair in itemTypePairs)
        {
            if (percentTypePair.rangeMaxValue == percentTypePair.rangeMinValue)
            {
                SetPercentObjectPairRange(percentTypePair);
            }
        }

        foreach (PercentUsablePair percentUsablePair in usablePairs)
        {
            if (percentUsablePair.rangeMaxValue == percentUsablePair.rangeMinValue)
            {
                SetPercentObjectPairRange(percentUsablePair);
            }
        }
    }
}
