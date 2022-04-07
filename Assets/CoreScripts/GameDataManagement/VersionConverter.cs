using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Version Converter", menuName = "Version Converter")]
public class VersionConverter : ScriptableObject
{
    // Data Structures //
    public class FileKey
    {
        public string originalName;
        public string newName;
    }


    // DATA //
    // Setup Data
    public string previousVersionFolderPath;
    // Conversion Data
    public float gameVersion;
    public List<FileKey> fileConversions;

    // Cached Data
    public Dictionary<string, FileKey> fileKeyTable = new Dictionary<string, FileKey>();


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        gameVersion = float.Parse(Application.version);
    }


    // Management
    public void SetupDictionary()
    {
        fileKeyTable.Clear();

        foreach(FileKey conversion in fileConversions)
        {
            if (!fileKeyTable.ContainsKey(conversion.originalName))
            {
                fileKeyTable.Add(conversion.originalName, conversion);
            }

            else
            {
                Debug.LogError("Found duplicate file name: " + conversion.originalName);
            }
        }
    }

    public void SetupFileConversions()
    {
        
    }
    
    // Usage Functions
    public string GetConvertedName(string initialName)
    {
        FileKey conversion = null;
        if (fileKeyTable.TryGetValue(initialName, out conversion))
        {
            return conversion.newName;
        }

        else
        {
            return initialName;
        }
    }
}
