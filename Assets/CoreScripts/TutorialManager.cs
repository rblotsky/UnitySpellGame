using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour, IDataPersistentComponent
{
    // Data Structures //
    private enum TutorialStage
    {
        WASD_To_Move,
        Right_Click_NPC,
        Click_To_Use_Item,
        Pick_Up_Spell_Tome,
        Create_Spell_With_Components,
        Finished_Tutorial,
    }


    // DATA //
    // References
    [Header("References")]
    public OverlayUITutorialPanels textPanels;
    public GameObject secondPointerObject;
    public GameObject pickUpItemPointerObject;

    // Text
    [Header("Tutorial Text")]
    [TextArea(1, 4)] public string text_WASDToMove;
    [TextArea(1, 4)] public string text_RightClickNPC;
    [TextArea(1, 4)] public string text_PickUpSpellTome;
    [TextArea(1, 4)] public string text_ClickToUseItem;
    [TextArea(1, 4)] public string text_CreateSpellWithComponents;
    [TextArea(1, 4)] public string taskCompleteText = "Tutorial Task Complete!";

    // Other Data
    [Header("Other Data")]
    public float distanceFromStartToUpdate = 5f;
    public Vector3 hoverOffset = new Vector3(0, 2f, 0);

    // Cached Data
    private PlayerComponent player;
    private Vector3 playerStartLocation;
    private TutorialStage currentTutorialStage = 0;
    private SpellComponentItem pickedUpTome;


    // FUNCTIONS //
    // Basic Functions
    void Awake ()
    {
        player = FindObjectOfType<PlayerComponent>();
        playerStartLocation = player.GetPlayerPosition();
    }

    void Update()
    {
        // If the player needs to move, checks if they moved from initial position
        if (currentTutorialStage == TutorialStage.WASD_To_Move)
        {
            if (Vector3.Distance(player.GetPlayerPosition(), playerStartLocation) > distanceFromStartToUpdate)
            {
                currentTutorialStage = TutorialStage.Right_Click_NPC;

                if (DataRef.overlayManagerReference != null)
                {
                    DataRef.overlayManagerReference.DisplayNotification(taskCompleteText);
                }
            }
        }

        // If the player needs to pick up an item, checks if they picked it up
        else if(currentTutorialStage == TutorialStage.Pick_Up_Spell_Tome)
        {
            if (pickedUpTome == null) 
            {
                foreach (Usable item in PlayerEquipment.main.inventory)
                {
                    if (item is SpellComponentItem)
                    {
                        pickedUpTome = (SpellComponentItem)item;
                        currentTutorialStage = TutorialStage.Click_To_Use_Item;

                        if (DataRef.overlayManagerReference != null)
                        {
                            DataRef.overlayManagerReference.DisplayNotification(taskCompleteText);
                        }

                        break;
                    }
                }
            }
        }

        // If the player needs to use their item, checks if they used it
        else if(currentTutorialStage == TutorialStage.Click_To_Use_Item)
        {
            if (pickedUpTome != null)
            {
                if(!PlayerEquipment.main.InventoryContains(pickedUpTome, true))
                {
                    pickedUpTome = null;
                    currentTutorialStage = TutorialStage.Create_Spell_With_Components;

                    if (DataRef.overlayManagerReference != null)
                    {
                        DataRef.overlayManagerReference.DisplayNotification(taskCompleteText);
                    }
                }
            }
        }

        // If the player needs to make a spell, checks if they made it
        else if(currentTutorialStage == TutorialStage.Create_Spell_With_Components)
        {
            if (PlayerSpells.main.spell1.IsValid)
            {
                currentTutorialStage = TutorialStage.Finished_Tutorial;

                if (DataRef.overlayManagerReference != null)
                {
                    DataRef.overlayManagerReference.DisplayNotification(taskCompleteText);
                }
            }
        }

        // Updates the tutorial UI panel
        UpdateTutorialPanel();
    }


    // Interface Functions
    public string SaveDataToString()
    {
        string cachedTomeName = "null";

        if(pickedUpTome != null)
        {
            cachedTomeName = pickedUpTome.id.ToString();
        }

        return (int)currentTutorialStage + "," + cachedTomeName;
    }

    public void LoadDataFromString(string data)
    {
        string[] dataArray = data.Trim().Split(',');

        if(dataArray.Length != 2)
        {
            Debug.LogError("[TutorialManager] IDataPersistentComponent (" + name + ") has received incorrectly formatted data array: " + data);
            return;
        }

        currentTutorialStage = (TutorialStage)int.Parse(dataArray[0]);
        pickedUpTome = (SpellComponentItem)UsableList.FindItem(dataArray[1]);
    }


    // Other Functions
    public void OnNPCRightClick()
    {
        if (currentTutorialStage == TutorialStage.Right_Click_NPC || currentTutorialStage == TutorialStage.Right_Click_NPC)
        {
            currentTutorialStage = TutorialStage.Pick_Up_Spell_Tome;

            if(pickUpItemPointerObject == null)
            {
                currentTutorialStage = TutorialStage.Create_Spell_With_Components;
            }

            if (DataRef.overlayManagerReference != null)
            {
                DataRef.overlayManagerReference.DisplayNotification(taskCompleteText);
            }
        }
    }

    public void UpdateTutorialPanel()
    {
        // Sets position and text
        Vector3 position = hoverOffset;

        switch (currentTutorialStage)
        {
            case TutorialStage.WASD_To_Move:
                position += player.GetPlayerPosition();
                textPanels.SetPanel(position, text_WASDToMove);
                break;

            case TutorialStage.Right_Click_NPC:
                position += secondPointerObject.transform.position;
                textPanels.SetPanel(position, text_RightClickNPC);
                break;

            case TutorialStage.Pick_Up_Spell_Tome:
                position += pickUpItemPointerObject.transform.position;
                textPanels.SetPanel(position, text_PickUpSpellTome);
                break;

            case TutorialStage.Click_To_Use_Item:
                position += player.GetPlayerPosition();
                textPanels.SetPanel(position, text_ClickToUseItem);
                break;

            case TutorialStage.Create_Spell_With_Components:
                position += player.GetPlayerPosition();
                textPanels.SetPanel(position, text_CreateSpellWithComponents);
                break;

            default:
                textPanels.SetPanel(position, "");
                break;
        }
    }
}
