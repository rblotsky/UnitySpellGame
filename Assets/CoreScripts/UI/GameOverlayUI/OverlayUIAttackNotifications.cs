using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlayUIAttackNotifications : MonoBehaviour
{
    // Data Structures //
    class PanelData
    {
        public RectTransform rect;
        public TextMeshProUGUI text;
        public float displayStartTime;
        public Vector3 sourcePos;

        public PanelData(RectTransform newRect, TextMeshProUGUI textBox)
        {
            rect = newRect;
            text = textBox;
            displayStartTime = 0;
            sourcePos = Vector3.zero;
        }
    }


    // DATA //
    // References
    public RectTransform[] allRects;
    public Canvas mainCanvas;

    // Constants
    public static readonly float PANEL_DISPLAY_TIME = 2f;

    // Cached Data
    private List<PanelData> panels;
    private Camera mainCamera;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        panels = new List<PanelData>();

        // Creates panel list
        foreach (RectTransform rect in allRects)
        {
            if (rect != null)
            {
                TextMeshProUGUI text = rect.GetComponentInChildren<TextMeshProUGUI>();

                if (text != null)
                {
                    panels.Add(new PanelData(rect, text));
                }

                else
                {
                    Debug.LogWarning("[CombatEntityAnimator] attackApplyPanel item \"" + rect.name + "\" does not have a TextMeshProUGUI Component!");
                }
            }
        }
    }

    private void Start()
    {
        mainCanvas = GetComponentInParent<Canvas>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Iterates through all panels to move and/or them
        foreach (PanelData panel in panels)
        {
            float timeElapsed = Time.time - panel.displayStartTime;

            // Updates text if below display time
            if (timeElapsed > PANEL_DISPLAY_TIME)
            {
                panel.text.SetText("");
            }

            // Otherwise, moves to correct position
            else
            {
                //TODO: Add an animation so it moves a little bit and looks cool!
                Vector3 screenPoint = mainCamera.WorldToScreenPoint(panel.sourcePos);
                panel.rect.transform.position = GameUtility.ClampUIElementToCanvas(panel.rect, mainCanvas, screenPoint);
            }
        }
    }


    // External Management Functions
    public void NewPanel(CombatEffects attackInfo, Vector3 sourcePosition, GameObject hitObject)
    {
        // Uses longest-ago used panel
        PanelData newPanel = panels[0];

        newPanel.displayStartTime = Time.time;

        // Moves panel to location (world to screen point of hit object)
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(hitObject.transform.position);

        newPanel.rect.transform.position = GameUtility.ClampUIElementToCanvas(newPanel.rect, mainCanvas, screenPoint);
        newPanel.text.SetText(attackInfo.GetDisplayText(false));
        newPanel.sourcePos = hitObject.transform.position;

        // Moves to end of list
        panels.Remove(newPanel);
        panels.Add(newPanel);
    }
}
