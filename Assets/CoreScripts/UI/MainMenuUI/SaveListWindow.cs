using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class SaveListWindow : MonoBehaviour
{ 
    // DATA //
    // References
    public GameDataManager dataManager;
    public TextMeshProUGUI[] saveLabels;

    // Cache Data
    private List<string> allSaves = new List<string>();
    private PageBasedObjectCollection<string> savePages;
    private int pageSize = 1;
    private int currentPage = 0;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        // Updates the saves folder path in GameDataManager
        dataManager.UpdateSavesPath();

        // Loads settings
        GameSettings.LoadSettings();
    }


    // Startup Function (runs on UI open)
    public void StartUI()
    {
        // Gets the number of save slots per page from amount of save labels
        pageSize = saveLabels.Length;

        // Resets currentPage
        currentPage = 0;

        // Updates saves list
        UpdateSavesList();

        // Updates UI
        UpdatePageUI();
    }


    // Save List Management
    public void UpdateSavesList()
    {
        // Resets saveNames
        allSaves.Clear();

        // Gets the list of save files
        string[] saveFiles = Directory.GetDirectories(GameDataManager.saveFolderPath);

        // Iterates through list of save files, and adds save names to list
        foreach(string saveFile in saveFiles)
        {
            Debug.Log(saveFile);
            allSaves.Add(saveFile.Replace(GameDataManager.saveFolderPath+"\\", ""));
        }

        // Updates page based save list
        savePages = new PageBasedObjectCollection<string>(allSaves, pageSize);
    }


    // Updating UI
    void UpdatePageUI()
    {
        // Gets current page saves
        string[] savesOnPage = savePages.GetPageObjects(currentPage);

        // For each saveLabel, sets its text to the correct save name from the current page
        for(int labelIndex = 0; labelIndex < saveLabels.Length; labelIndex++)
        {
            // If the index is higher than the furthest indexed save, sets text to none
            if (labelIndex > savesOnPage.Length - 1)
            {
                saveLabels[labelIndex].SetText("[Empty Slot]");
                continue;
            }

            // Otherwise, sets text to the current page
            saveLabels[labelIndex].SetText(savesOnPage[labelIndex]);
        }
    }


    // UI Events
    public void OpenSave(int saveIndex)
    {
        // Gets current page saves
        string[] savesOnPage = savePages.GetPageObjects(currentPage);

        // Does nothing if the save index is higher than the final index on the page
        if (saveIndex > savesOnPage.Length - 1)
        {
            return;
        }

        // Otherwise, loads game progress for name at given index and loads that scene
        dataManager.LoadGameProgress(savesOnPage[saveIndex]);
        
    }
    
    public void DeleteSave(int saveIndex)
    {
        // Gets current page saves
        string[] savesOnPage = savePages.GetPageObjects(currentPage);

        // If the given index is higher than the final index on page, does nothing
        if (saveIndex > savesOnPage.Length - 1)
        {
            return;
        }

        // Deletes save at given index
        dataManager.DeleteSave(savesOnPage[saveIndex]);

        // Gets existing saves again and updates UI
        UpdateSavesList();
        UpdatePageUI();
    }

    public bool CreateNewSave(string name)
    {
        // If there are any invalid characters in name, returns false
        if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            return false;
        }

        // Tries creating file path - if any characters are invalid, returns false. 
        // Uses try/catch in case Path.Combine throws any errors.
        try
        {
            // Checks if file path exists, returns false if it does.
            if(File.Exists(Path.Combine(GameDataManager.saveFolderPath, name)))
            {
                return false;
            }
        }
        catch(ArgumentException exception)
        {
            Debug.Log(exception.Message);
            return false;
        }

        // Creates a new save using given name if all checks pass
        dataManager.StartNewSave(name);
        return true;
    }

    public void ChangePage(int pageChange)
    {
        // Increments/Decrements current page
        currentPage += pageChange;

        // Gets current page saves
        string[] savesOnPage = savePages.GetPageObjects(currentPage);

        // If there are no saves on this page, reverts change
        if (savesOnPage.Length < 1)
        {
            // Reverts pageChange
            currentPage -= pageChange;
        }

        // Updates UI
        UpdatePageUI();
    }
}
