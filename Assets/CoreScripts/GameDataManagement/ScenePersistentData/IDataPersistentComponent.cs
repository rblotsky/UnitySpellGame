using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistentComponent
{
    public string SaveDataToString();
    public void LoadDataFromString(string data);
}
