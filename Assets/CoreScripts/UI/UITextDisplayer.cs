using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITextDisplayer : MonoBehaviour
{
    // STORED DATA //
    public TextMeshProUGUI textBox;
    public float letterDelay = 0.05f;
    public float remainDelay = 2f;
    public float fadeTime = 1f;


    // FUNCTIONS //
    // Basic Text Displayer Function
    public virtual IEnumerator RunTextDisplayer(string text)
    {
        // Makes sure box has alpha of 1
        textBox.alpha = 1;

        // Displays text letter by letter
        for (int i = 0; i <= text.Length; i++)
        {
            // Displays all letters up to current letter, then waits a short delay
            textBox.SetText(text.Substring(0, i));
            yield return new WaitForSeconds(letterDelay);
        }

        // Waits to allow player to read what it says
        yield return new WaitForSeconds(remainDelay);

        // Fades out text
        for (int i = 0; i < fadeTime * 10; i++)
        {
            textBox.alpha -= 0.10f;
        }
        textBox.SetText("");
    }
}
