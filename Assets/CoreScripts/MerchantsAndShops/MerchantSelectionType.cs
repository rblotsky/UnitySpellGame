using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MerchantSelectionType
{
    /// <summary>
    /// Random from a given selection of items, using given chances for each.
    /// </summary>
    Select_From_Selection,
    /// <summary>
    /// Random item of given type, using given chances for each type.
    /// </summary>
    Select_From_Type,
}
