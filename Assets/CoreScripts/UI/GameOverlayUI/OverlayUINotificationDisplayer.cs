using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class OverlayUINotificationDisplayer : MonoBehaviour
{
    // Data Structures
    public class OverlayNotification
    {
        // Data
        public string notificationText;
        public float displayStartTime;

        // Constructor
        public OverlayNotification(string text, float startTime)
        {
            notificationText = text;
            displayStartTime = startTime;
        }

        public OverlayNotification(string text)
        {
            notificationText = text;
            displayStartTime = Time.time;
        }
    }


    // DATA //
    // References
    public TextMeshProUGUI displayTextBox;

    // Constants
    public static readonly float NOTIFICATION_DISPLAY_TIME = 4.0f;
    public static readonly float REMOVAL_CHECK_INTERVAL = 0.25f;

    // Cached Data
    private Queue<OverlayNotification> notifications = new Queue<OverlayNotification>();


    // FUNCTIONS //
    // Pausing/Unpausing
    public void PauseDisplay()
    {
        // Stops all coroutines and empties text box
        StopAllCoroutines();
        displayTextBox.SetText("");
    }

    public void UnPauseDisplay()
    {
        // Starts notification removal check and updates display
        StartCoroutine(NotificationRemovalCheck());
        UpdateNotificationDisplay();
    }


    // UI Updating Functions
    public void UpdateNotificationDisplay()
    {
        // Creates empty string of text to put in text box
        string notificationText = "";

        // Adds each notification to main string
        foreach(OverlayNotification notification in notifications)
        {
            notificationText += ("\n" + notification.notificationText);
        }

        // Adds notifications to the display
        displayTextBox.SetText(notificationText);
    }


    // External Updating Functions
    public void AddNotification(string text)
    {
        // Enqueues new notification
        notifications.Enqueue(new OverlayNotification(text, Time.time));

        // Updates UI
        UpdateNotificationDisplay();
    }


    // Notification Removal Coroutine
    public IEnumerator NotificationRemovalCheck()
    {
        // While this is enabled, checks every second whether a notification has displayed for long enough and removes it.
        while (enabled)
        {
            // Iterates through each notification to check if it should be removed.
            foreach(OverlayNotification notification in notifications.ToArray())
            {
                // If the time since the start time is greater than display time, dequeues.
                if(Time.time-notification.displayStartTime >= NOTIFICATION_DISPLAY_TIME)
                {
                    notifications.Dequeue();
                }

                // Otherwise, breaks loop (as they're in chronological order, it is useless to check the rest)
                else
                {
                    break;
                }
            }

            // Updates UI
            UpdateNotificationDisplay();

            // Waits check interval before looping again
            yield return new WaitForSeconds(REMOVAL_CHECK_INTERVAL);
        }

        yield return null;
    }

}
