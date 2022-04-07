using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class BaseGameData
{
    // UNIVERSAL DATA //
    public string saveName;
    public string currentSceneName;
    public string savedVersion = "0.0";
    //TODO: All settings here

    public Queue<string> SaveData()
    {
        Queue<string> queue = new Queue<string>();
        queue.Enqueue(JsonConvert.SerializeObject(this));
        return queue;
    }
}
