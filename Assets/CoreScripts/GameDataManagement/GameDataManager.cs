using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "Game Data Manager", menuName = "GameData/Game Data Manager")]
public class GameDataManager : ScriptableObject
{
    // GLOBAL GAME DATA //
    // Other data
    public string startSceneName = "TestingScene";

    // Static Data
    public static string saveFolderPath { get; private set; }

    // Constants
    public static readonly string FILE_EXTENSION = ".json";
    public static readonly string MAIN_FILE_NAME = "Main";
    public static readonly string PLAYER_STATS_FILE_NAME = "PlayerStats";
    public static readonly string QUEST_FILE_NAME = "Quests";
    public static readonly string SPELLS_FILE_NAME = "PlayerSpells";
    public static readonly string SCENE_DATA_FILE_NAME = "ScenePersistentData";
    public static readonly string MERCHANT_DATA_FILE_NAME = "Merchants";
    public static readonly string MAP_DATA_FILE_NAME = "Maps";
    public static readonly string INVENTORY_FILE_NAME = "Inventory";
    public static readonly string ARTIFACT_FILE_NAME = "Artifacts";


    // FUNCTIONS //
    // Main Functions
    public bool SaveGameProgress(string saveName, string sceneName)
    {
        // Checks if saves folder exists, if not creates one and returns false.
        if (CheckIfSaveFolder(saveFolderPath))
        {
            // Trims saveName
            saveName = saveName.Trim();

            // Gets the path to this save folder
            string pathToNewSave = Path.Combine(saveFolderPath, saveName);

            // If the directory doesn't exist, makes it
            if (!Directory.Exists(pathToNewSave))
            {
                Directory.CreateDirectory(pathToNewSave);
            }

            // Saving Main Save File
            DataRef.RefreshBaseGameData(sceneName);
            StreamWriter newSaveMainFile = new StreamWriter(Path.Combine(pathToNewSave, MAIN_FILE_NAME + FILE_EXTENSION));
            AddLinesToStreamWriter(newSaveMainFile, DataRef.baseData.SaveData());
            newSaveMainFile.Close();


            // Saving PlayerStats File
            DataRef.playerReference.RefreshPlayerStats();
            StreamWriter newSavePlayerStatsFile = new StreamWriter(Path.Combine(pathToNewSave, PLAYER_STATS_FILE_NAME + FILE_EXTENSION));
            AddLinesToStreamWriter(newSavePlayerStatsFile, PlayerStats.SaveDataToStringQueue());
            newSavePlayerStatsFile.Close();

            // Saving Inventory File
            if (PlayerEquipment.main == null)
            {
                Debug.LogError("[GameDataManager] Saving: Cannot find PlayerEquipment Main!");
                return false;
            }

            StreamWriter newSaveInventoryFile = new StreamWriter(Path.Combine(pathToNewSave, INVENTORY_FILE_NAME + FILE_EXTENSION));
            AddLinesToStreamWriter(newSaveInventoryFile, PlayerEquipment.main.SaveDataToStringQueue());
            newSaveInventoryFile.Close();

            // Saving Quests File
            StreamWriter newSaveQuestsFile = new StreamWriter(Path.Combine(pathToNewSave, QUEST_FILE_NAME + FILE_EXTENSION));
            AddLinesToStreamWriter(newSaveQuestsFile, Quests.SaveDataList());
            newSaveQuestsFile.Close();

            // Saving Spells File
            if (PlayerSpells.main == null)
            {
                Debug.LogError("[GameDataManager] Saving: Cannot find PlayerSpells Main!");
                return false;
            }

            StreamWriter newSaveSpellsFile = new StreamWriter(Path.Combine(pathToNewSave, SPELLS_FILE_NAME + FILE_EXTENSION));
            AddLinesToStreamWriter(newSaveSpellsFile, PlayerSpells.main.SaveDataToStringQueue());
            newSaveSpellsFile.Close();

            // Saving SceneData File
            StreamWriter newSaveSceneDataFile = new StreamWriter(Path.Combine(pathToNewSave, SCENE_DATA_FILE_NAME + FILE_EXTENSION));
            AddLinesToStreamWriter(newSaveSceneDataFile, ScenePersistentDataList.SaveDataList());
            newSaveSceneDataFile.Close();

            // Saving MerchantData File
            StreamWriter newMerchantDataSaveFile = new StreamWriter(Path.Combine(pathToNewSave, MERCHANT_DATA_FILE_NAME + FILE_EXTENSION));
            AddLinesToStreamWriter(newMerchantDataSaveFile, MerchantDataList.SaveDataList());
            newMerchantDataSaveFile.Close();

            // Saving MapData File
            StreamWriter newMapDataSaveFile = new StreamWriter(Path.Combine(pathToNewSave, MAP_DATA_FILE_NAME + FILE_EXTENSION));
            AddLinesToStreamWriter(newMapDataSaveFile, MapDataList.SaveDataList());
            newMapDataSaveFile.Close();

            // Saving ArtifactData File
            StreamWriter newArtifactDataSaveFile = new StreamWriter(Path.Combine(pathToNewSave, ARTIFACT_FILE_NAME + FILE_EXTENSION));
            AddLinesToStreamWriter(newArtifactDataSaveFile, ArtifactDataList.SaveDataList());
            newArtifactDataSaveFile.Close();

            // Returns true when saving completes successfully
            Debug.Log("[GameDataManager] New Save Completed: " + pathToNewSave);
            return true;
        }

        else
        {
            // Creates new Saves folder
            Directory.CreateDirectory(saveFolderPath);
            Debug.LogError("[GameDataManager] Saving: Could not find Saves folder, new folder created: " + saveFolderPath);
            return false;
        }
    }

    public bool LoadGameProgress(string saveName)
    {
        Debug.Log("[GameDataManager] LOADING GAME PROGRESS BEGINNING!");

        // Sets up all data managers
        SetupAllDataManagers();

        Debug.Log(saveFolderPath);

        // Checks if saves folder exists. If not, returns false.
        if (CheckIfSaveFolder(saveFolderPath))
        {
            string pathToThisSave = Path.Combine(saveFolderPath, saveName);

            // Checks if save folder exists. If not, returns false.
            if (!Directory.Exists(pathToThisSave))
            {
                Debug.LogError("[GameDataManager] Loading: Save folder does not exist: " + pathToThisSave);
                return false;
            }

            // Checks if save text files exist. If not, returns false.
            if (!File.Exists(Path.Combine(pathToThisSave, MAIN_FILE_NAME + FILE_EXTENSION)))
            {
                Debug.LogError("[GameDataManager] Loading: MainSaveFile not found: " + pathToThisSave);
                return false;
            }

            if (!File.Exists(Path.Combine(pathToThisSave, PLAYER_STATS_FILE_NAME + FILE_EXTENSION)))
            {
                Debug.LogError("[GameDataManager] Loading: PlayerStatsFile not found: " + pathToThisSave);
                return false;
            }

            if (!File.Exists(Path.Combine(pathToThisSave, QUEST_FILE_NAME + FILE_EXTENSION)))
            {
                Debug.LogError("[GameDataManager] Loading: QuestsFile not found: " + pathToThisSave);
                return false;
            }

            if (!File.Exists(Path.Combine(pathToThisSave, SPELLS_FILE_NAME + FILE_EXTENSION)))
            {
                Debug.LogError("[GameDataManager] Loading: PlayerSpellsFile not found: " + pathToThisSave);
                return false;
            }

            if (!File.Exists(Path.Combine(pathToThisSave, SCENE_DATA_FILE_NAME + FILE_EXTENSION)))
            {
                Debug.LogError("[GameDataManager] Loading: SceneDataFile not found: " + pathToThisSave);
                return false;
            }

            if (!File.Exists(Path.Combine(pathToThisSave, MERCHANT_DATA_FILE_NAME + FILE_EXTENSION)))
            {
                Debug.LogError("[GameDataManager] Loading: MerchantDataFile not found: " + pathToThisSave + "\n Continuing with empty MerchantDataFile - merchant data isn't necessary for save functionality. Backup of current save status has been made.");

                // Creates backup folder
                string pathToBackup = pathToThisSave + "Backup/";
                Directory.CreateDirectory(pathToBackup);

                // Copies everything in save folder to backup folder
                foreach(string fileName in Directory.GetFiles(pathToThisSave))
                {
                    File.Copy(fileName, fileName.Replace(pathToThisSave, pathToBackup), true);
                }

                // Creates empty merchant data file
                StreamWriter newFile = new StreamWriter(Path.Combine(pathToThisSave, MERCHANT_DATA_FILE_NAME + FILE_EXTENSION));
                newFile.Close();
            }

            if (!File.Exists(Path.Combine(pathToThisSave, MAP_DATA_FILE_NAME + FILE_EXTENSION)))
            {
                Debug.LogError("[GameDataManager] Loading: MapDataFile not found: " + pathToThisSave);
                return false;
            }

            if (!File.Exists(Path.Combine(pathToThisSave, ARTIFACT_FILE_NAME + FILE_EXTENSION)))
            {
                Debug.LogError("[GameDataManager] Loading: ArtifactDataFile not found: " + pathToThisSave);
                return false;
            }

            // Loading PlayerStats
            StreamReader playerStatsFile = new StreamReader(Path.Combine(pathToThisSave, PLAYER_STATS_FILE_NAME + FILE_EXTENSION));
            PlayerStats.LoadDataFromText(playerStatsFile.ReadToEnd());
            playerStatsFile.Close();

            // Loading PlayerEquipment
            StreamReader inventoryFile = new StreamReader(Path.Combine(pathToThisSave, INVENTORY_FILE_NAME + FILE_EXTENSION));
            PlayerEquipment.main.LoadDataFromText(inventoryFile.ReadToEnd());
            inventoryFile.Close();

            // Loading Quests
            StreamReader questsFile = new StreamReader(Path.Combine(pathToThisSave, QUEST_FILE_NAME + FILE_EXTENSION));
            Quests.LoadDataList(questsFile.ReadToEnd());
            questsFile.Close();

            // Loading Spells
            StreamReader spellsFile = new StreamReader(Path.Combine(pathToThisSave, SPELLS_FILE_NAME + FILE_EXTENSION));
            PlayerSpells.main.LoadDataFromText(spellsFile.ReadToEnd());
            spellsFile.Close();

            // Loading Scene Data
            StreamReader sceneDataFile = new StreamReader(Path.Combine(pathToThisSave, SCENE_DATA_FILE_NAME + FILE_EXTENSION));
            ScenePersistentDataList.LoadDataList( sceneDataFile.ReadToEnd());
            sceneDataFile.Close();

            // Loading Merchant Data
            StreamReader merchantDataFile = new StreamReader(Path.Combine(pathToThisSave, MERCHANT_DATA_FILE_NAME + FILE_EXTENSION));
            MerchantDataList.LoadDataList(merchantDataFile.ReadToEnd());
            merchantDataFile.Close();

            // Loading Map Data
            StreamReader mapDataFile = new StreamReader(Path.Combine(pathToThisSave, MAP_DATA_FILE_NAME + FILE_EXTENSION));
            MapDataList.LoadDataList(mapDataFile.ReadToEnd());
            mapDataFile.Close();

            // Loading Map Data
            StreamReader artifactDataFile = new StreamReader(Path.Combine(pathToThisSave, ARTIFACT_FILE_NAME + FILE_EXTENSION));
            ArtifactDataList.LoadDataList(artifactDataFile.ReadToEnd());
            artifactDataFile.Close();

            // Loading scene and starting game
            StreamReader mainFile = new StreamReader(Path.Combine(pathToThisSave, MAIN_FILE_NAME + FILE_EXTENSION));
            DataRef.LoadBaseGameData(mainFile.ReadToEnd());

            // Resetting static variables
            PlayerStats.isRunningDialogue = false;
            PlayerStats.canPlayerCastSpell = true;

            // Updates game version
            DataRef.baseData.savedVersion = Application.version;

            // Loading Scene
            SceneManager.LoadScene(DataRef.baseData.currentSceneName, LoadSceneMode.Single);
            Time.timeScale = 1;

            mainFile.Close();
        }

        else
        {
            // Creates new Saves folder
            Directory.CreateDirectory(saveFolderPath);
            Debug.LogError("[GameDataManager] Loading: Could not find Saves folder, new folder created: " + saveFolderPath);
            return false;
        }

        // Returns true if everything passes
        return true;
    
    }


    // External Save Management Functions
    public void UpdateSavesPath()
    {
        // Updates the global path to save folder
        saveFolderPath = Path.Combine(Application.persistentDataPath, "Saves");
    }

    public void DeleteSave(string saveName)
    {
        // Deletes directory with given name
        Directory.Delete(Path.Combine(saveFolderPath, saveName), true);
    }

    public void StartNewSave(string saveName)
    {
        // Starts a new game from scratch. Makes a new directory as well.
        // Creates directory
        Directory.CreateDirectory(Path.Combine(saveFolderPath, saveName));

        // Sets up all other data managers
        SetupAllDataManagers();

        // Updating SceneDataRef data
        BaseGameData newBaseData = new BaseGameData();
        newBaseData.saveName = saveName;
        newBaseData.currentSceneName = startSceneName;
        newBaseData.savedVersion = Application.version;
        DataRef.baseData = newBaseData;

        // Resetting static variables
        PlayerStats.isRunningDialogue = false;
        PlayerStats.canPlayerCastSpell = true;
        Time.timeScale = 1;

        // Sets player location
        MapDataList.playerMapLocation = MapDataList.GetLocationByCurrentTransferSceneName(startSceneName);

        // Loads default scene Scene
        SceneManager.LoadScene(startSceneName, LoadSceneMode.Single);

        // The game is saved immediately after the scene loads from the start method in PlayerComponent
    }


    // Internal Management Functions
    private void SetupAllDataManagers()
    {
        // Resets player inventory, spells, and stats
        PlayerEquipment.SetMainReference();
        PlayerSpells.SetMainReference();
        PlayerStats.ResetStats();

        // Sets up and clears all main lists (setup functions set up data to defaults)
        UsableList.SetupFunction();
        Quests.MainSetup();
        SpellComponentList.SetupFunction();
        ScenePersistentDataList.ResetSceneData();
        MapDataList.SetupFunction();
        MerchantDataList.SetupList();
        //TODO: SETUP ARTIFACTS!
    }

    // Utility Saving Functions
    private bool CheckIfSaveFolder(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void AddLinesToStreamWriter(StreamWriter writer, Queue<string> lines)
    {
        foreach(string line in lines)
        {
            writer.WriteLine(line);
        }
    }
}
