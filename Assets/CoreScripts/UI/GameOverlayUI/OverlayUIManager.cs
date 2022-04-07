using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[DisallowMultipleComponent]
public class OverlayUIManager : MonoBehaviour
{
    // STORED DATA //
    // UI References
    public OverlayUIItemSlot[] itemSlotList;
    public DialogueDisplayer dialogueDisplayer;
    public QuestNotificationDisplayer questNotificationDisplayer;
    public OverlayUINotificationDisplayer notificationDisplayer;
    public UIPlayerDataDisplayer playerInfoUI;

    // Cached Data
    private PlayerComponent playerInScene;
    private Queue<NPCDialogue> pauseDisplayDialogues = new Queue<NPCDialogue>();


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        // Gets a reference to the player
        playerInScene = FindObjectOfType<PlayerComponent>();

        // Gets other component references
        if (playerInfoUI == null)
        {
            playerInfoUI = GetComponentInChildren<UIPlayerDataDisplayer>();
        }

        if (dialogueDisplayer == null)
        {
            dialogueDisplayer = GetComponentInChildren<DialogueDisplayer>();
        }

        if (notificationDisplayer == null)
        {
            notificationDisplayer = GetComponentInChildren<OverlayUINotificationDisplayer>();
        }

        if (questNotificationDisplayer == null)
        {
            questNotificationDisplayer = GetComponentInChildren<QuestNotificationDisplayer>();
        }
    }

    private void OnEnable()
    {
        // Adds listeners to player startup and invventory update
        playerInScene.onStartUp += AddInventoryEvents;
        playerInScene.onStartUp += SetupItemSlots;
    }

    private void OnDisable()
    {
        // Removes listeners to player startup and inventory update
        PlayerEquipment.main.onInventoryUpdate -= UpdateInventoryDisplay;
        playerInScene.onStartUp -= SetupItemSlots;
        playerInScene.onStartUp -= AddInventoryEvents;
    }

    private void Start()
    {
        // Starts Notification Displayer
        notificationDisplayer.UnPauseDisplay();

        // Starts data displayer
        playerInfoUI.SetEntityReference(playerInScene.component_CombatEntity);
    }

    private void Update()
    {
        // When the player clicks, tells dialogue displayer to skip dialogue line. SkipLine makes sure a dialogue is running.
        if(Input.GetMouseButtonDown(0))
        {
            dialogueDisplayer.SkipLine();
        }
    }


    // Basic UI Management
    public void DisableOverlay()
    {
        // Pauses and removes events
        dialogueDisplayer.PauseDisplayer();
        questNotificationDisplayer.PauseNotifications();
        notificationDisplayer.PauseDisplay();
        gameObject.SetActive(false);
    }

    public void EnableOverlay()
    {
        // Unpauses and readds all events
        gameObject.SetActive(true);
        questNotificationDisplayer.ResumeNotifications();
        notificationDisplayer.UnPauseDisplay();
        dialogueDisplayer.UnPauseDisplayer(this);
        PlayerEquipment.main.onInventoryUpdate += UpdateInventoryDisplay;

        // Updates displays to show new possibly changed data
        playerInfoUI.UpdateHealth(0);
        playerInfoUI.UpdateHealthChangeEffect();
        playerInfoUI.UpdateSpeedEffect();
        UpdateInventoryDisplay();
    }

    private void AddInventoryEvents()
    {
        PlayerEquipment.main.onInventoryUpdate += UpdateInventoryDisplay;
    }


    // Inventory UI
    public void SetupItemSlots()
    {
        // Updates each slot, then updates display
        foreach (OverlayUIItemSlot slot in itemSlotList)
        {
            slot.Setup();
        }

        UpdateInventoryDisplay();
    }

    public void UpdateItemSlot(int slotIndex, Usable item, bool isSlotSelected)
    {
        // Gets the slot to update
        OverlayUIItemSlot slotToUpdate = itemSlotList[slotIndex];

        // Updates item and selected status
        slotToUpdate.UpdateItem(item);
        slotToUpdate.UpdateSelectedStatus(isSlotSelected);

    }

    public void UpdateInventoryDisplay()
    {
        // Loops through all item slots
        for (int i = 0; i < itemSlotList.Length; i++)
        {
            Usable itemToUpdate = PlayerEquipment.main.inventory[i];

            // If the item isn't null, updates slot and selected status
            if (itemToUpdate != null)
            {
                // If the slot is the selected slot, sets it as selected (selection status is given as true/false expression)
                UpdateItemSlot(i, itemToUpdate, i == PlayerEquipment.main.selectedSlot);
            }

            // If the slot is null, sets slot to empty and not selected
            else
            {
                UpdateItemSlot(i, null, false);
            }

        }
    }


    // Dialogue Displaying
    public void DisplayDialogue(NPCDialogue dialogue)
    {
        dialogueDisplayer.QueueDialogue(dialogue, null);
    }

    public void DisplayDialogue(NPCDialogue dialogue, GameObject objectInWorld)
    {
        dialogueDisplayer.QueueDialogue(dialogue, objectInWorld);
    }


    // Notifications and Popups
    public void DisplayQuestNotification(string questName, CompletionStatus questCompletion)
    {
        // Starts a coroutine in questNotificationDisplayer to display the quest notification.
        // The title of the notification depends on the quest completion.

        if(questCompletion == CompletionStatus.Started)
        {
            // Displays notification with "New Quest Started" title
            questNotificationDisplayer.RunNotification("New Quest Started:", questName);
        }

        else
        {
            // If the quest is finished, displays title includes the completionstatus
            questNotificationDisplayer.RunNotification("Quest " + questCompletion.ToString() +  ":", questName);
        }
    }

    public void DisplayNotification(string text)
    {
        // Updates notification displayer with new notification
        notificationDisplayer.AddNotification(text);
    }

    public void DisplayNotification(string text, Color textColour)
    {
        // Creates formatted string with colour
        string colouredText = GameUtility.GenerateHTMLColouredText(text, textColour);

        // Updates notification displayer with new notification
        notificationDisplayer.AddNotification(colouredText);
    }
}