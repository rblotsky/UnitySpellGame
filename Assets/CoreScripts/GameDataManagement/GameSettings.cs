using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class GameSettings
{
    // SETTINGS DATA //
    public static SettingsObject settings;


    // FUNCTIONS //
    // Saving/Loading
    public static void SaveSettings()
    {
        StreamWriter fileWriter = new StreamWriter(Path.Combine(Application.persistentDataPath, "Settings.json"));
        fileWriter.WriteLine(JsonConvert.SerializeObject(settings));
        fileWriter.Close();
    }

    public static void LoadSettings()
    {
        string settingsPath = Path.Combine(Application.persistentDataPath, "Settings.json");

        // Creates new settings if there are none yet
        if (!File.Exists(settingsPath))
        {
            settings = new SettingsObject();
            SaveSettings();
        }

        // Otherwise loads settings
        else
        {
            StreamReader fileReader = new StreamReader(settingsPath);
            settings = JsonConvert.DeserializeObject<SettingsObject>(fileReader.ReadToEnd());
            fileReader.Close();
        }
    }
}

[System.Serializable]
public class SettingsObject
{
    public float charReadSpeed = 0.05f;
}