using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestNotificationDisplayer : MonoBehaviour
{
    // Data Structures
    struct QuestNotification
    {
        public string title;
        public string questName;

        // Constructor
        public QuestNotification(string newTitle, string newQuestName)
        {
            title = newTitle;
            questName = newQuestName;
        }
    }


    // STORED DATA //
    // Basic Data
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI questNameText;
    public float remainDelay = 2f;
    public float fadeTime = 1f;

    // Cache Data
    Queue<QuestNotification> notifications = new Queue<QuestNotification>();
    private bool isPaused = false;


    // FUNCTIONS //
    // Managing Display
    public void RunNotification(string title, string questName)
    {
        notifications.Enqueue(new QuestNotification(title, questName));

        // If this is the first item in notifications, starts the coroutine.
        if(notifications.Count == 1 && !isPaused)
        {
            StartCoroutine(DisplayNotification());
        }
    }

    public void PauseNotifications()
    {
        // Stops coroutines and resets title/questName text boxes
        StopAllCoroutines();

        titleText.SetText("");
        questNameText.SetText("");

        isPaused = true;
    }

    public void ResumeNotifications()
    {
        StartCoroutine(DisplayNotification());
        isPaused = false;
    }


    // Notification Displayer Coroutine
    private IEnumerator DisplayNotification()
    {
        // Continues running while there are notifications to display
        while (notifications.Count != 0)
        {
            // Sets title and questName text to default values
            titleText.SetText("");
            questNameText.SetText("");

            // Sets alpha to default values
            titleText.alpha = 0f;
            questNameText.alpha = 1f;

            // Sets title text
            titleText.SetText(notifications.Peek().title);

            // Fades in title text
            for (float i = 0; i < fadeTime; i += fadeTime / 10)
            {
                yield return new WaitForSeconds(fadeTime / 10);
                titleText.alpha += 0.1f;
            }

            // Writes quest name letter by letter
            for (int substringLength = 0; substringLength <= notifications.Peek().questName.Length; substringLength++)
            {
                questNameText.SetText(notifications.Peek().questName.Substring(0, substringLength));
                yield return new WaitForSeconds(0.05f);
            }

            // Waits to allow player to read what it says
            yield return new WaitForSeconds(remainDelay);

            // Fades out both at once
            for (float i = 0; i < fadeTime; i += fadeTime / 20)
            {
                yield return new WaitForSeconds(fadeTime / 20);
                titleText.alpha -= 0.05f;
                questNameText.alpha -= 0.05f;
            }

            // Dequeues the last notification used
            notifications.Dequeue();
        }

        yield return null;
    }
}
