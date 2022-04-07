using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class MainMenuNewSave : MonoBehaviour
{
    // DATA //
    // References
    public SaveListWindow saveManagerWindow;
    public TMP_InputField nameInputField;


    // FUNCTIONS //
    public void CreateSave()
    {
        // Creates a new save using text from name input field
        if (!saveManagerWindow.CreateNewSave(nameInputField.text))
        {
            nameInputField.SetTextWithoutNotify("Filename is invalid or already exists!");
        }
    }
}
