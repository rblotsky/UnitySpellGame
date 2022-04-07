using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class DialogueDisplayer : MonoBehaviour
{
    // Data Structures //
    public struct QueuedDialogue
    {
        public DialogueBase dialogue;
        public GameObject displayingObj;

        public QueuedDialogue(DialogueBase text, GameObject obj)
        {
            dialogue = text;
            displayingObj = obj;
        }
    }


    // DATA //
    // References
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueTextBox;
    public Button nextLineButton;

    // Constants
    private static WaitForSeconds letterDelay = new WaitForSeconds(0.03f);

    // Cached Data
    private int displayLength = 0;
    private int currentLine = 0;
    private Queue<QueuedDialogue> queuedDialogues = new Queue<QueuedDialogue>();
    private bool isPaused = false;
    private RectTransform panelRectTransform;
    private Camera sceneCamera;
    [HideInInspector] public Coroutine currentDialogueCoroutine;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        // Gets references
        panelRectTransform = dialoguePanel.GetComponent<RectTransform>();
        sceneCamera = Camera.main;

        // Sets button to advance line when clicked
        nextLineButton.onClick.AddListener(AdvanceLine);

        // Disables dialogue panel
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        // Moves UI panel to dialogue position
        if(queuedDialogues.Count > 0)
        {
            QueuedDialogue queued = queuedDialogues.Peek();

            if(queued.displayingObj != null)
            {
                // Moves to position of object on screen
                Vector3 newPos = sceneCamera.WorldToScreenPoint(queued.displayingObj.transform.position);
                dialoguePanel.transform.position = GameUtility.ClampUIElementToCanvas(panelRectTransform, dialogueTextBox.canvas, newPos);
            }
        }
    }


    // Managing UI
    public void PauseDisplayer()
    {
        // Closes dialogue panel and pauses the displayer
        dialoguePanel.SetActive(false);
        StopAllCoroutines();
        currentDialogueCoroutine = null;
        isPaused = true;
    }

    public void UnPauseDisplayer(OverlayUIManager uiManager)
    {
        isPaused = false;
        currentDialogueCoroutine = StartCoroutine(RunDialogueDisplayer());
    }

    public void QueueDialogue(DialogueBase dialogue, GameObject objectInWorld)
    {
        // Enqueues the dialogue
        queuedDialogues.Enqueue(new QueuedDialogue(dialogue, objectInWorld));

        if (!isPaused && currentDialogueCoroutine == null)
        {
            currentDialogueCoroutine = StartCoroutine(RunDialogueDisplayer());
        }
    }


    // Skipping Dialogue
    public void SkipLine()
    {
        if (queuedDialogues.Count > 0)
        {
            if (queuedDialogues.Peek().dialogue != null)
            {
                displayLength = queuedDialogues.Peek().dialogue.lines[currentLine].Length - 1;
            }
        }
    }

    public void AdvanceLine()
    {
        if (queuedDialogues.Count > 0)
        {
            currentLine++;
            displayLength = 0;
        }
    }


    // Dialogue Displaying Coroutine
    public IEnumerator RunDialogueDisplayer()
    {
        // Starts dialogues, pauses game
        PlayerStats.isRunningDialogue = true;
        dialoguePanel.SetActive(true);
        nextLineButton.gameObject.SetActive(false);

        Time.timeScale = 0;

        while (queuedDialogues.Count > 0)
        {
            // Gets dialogue and display text
            DialogueBase displayingDialogue = queuedDialogues.Peek().dialogue;
            StringBuilder displayText = new StringBuilder();

            // If the line is within range, displays it. Otherwise, dequeues this dialogue.
            if(currentLine < displayingDialogue.lines.Length)
            {
                // Gets text
                displayText.Append("[" + displayingDialogue.speakingNPC + "]: ");
                displayText.Append(displayingDialogue.lines[currentLine].Substring(0, displayLength));

                // Updates UI
                dialogueTextBox.SetText(displayText.ToString());
                nextLineButton.gameObject.SetActive(displayLength == displayingDialogue.lines[currentLine].Length);
                dialogueTextBox.SetLayoutDirty();

                // Yields for reading speed
                yield return new WaitForSecondsRealtime(GameSettings.settings.charReadSpeed);

                // If current length isn't max, increases it
                if (currentLine < displayingDialogue.lines.Length)
                {
                    if (displayLength < displayingDialogue.lines[currentLine].Length)
                    {
                        displayLength++;
                    }
                }
            }

            else
            {
                // Finishes current dialogue
                displayingDialogue.RunDialogueFinish();
                queuedDialogues.Dequeue();
                currentLine = 0;
                displayLength = 0;

                // Resets text and next line button
                dialogueTextBox.SetText(displayText.ToString());
                dialogueTextBox.SetLayoutDirty();

                nextLineButton.gameObject.SetActive(false);
            }
        }

        // Finishes dialogues
        dialoguePanel.SetActive(false);
        currentDialogueCoroutine = null;
        currentLine = 0;
        displayLength = 0;

        // Allows new dialogues to play, unpauses game
        PlayerStats.isRunningDialogue = false;
        Time.timeScale = 1;
    }
}
