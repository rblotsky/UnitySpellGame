using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverlayUITutorialPanels : MonoBehaviour
{
    // DATA //
    // References
    public TextMeshProUGUI panelText;

    // Cached Data
    private Camera mainCamera;
    private Canvas mainCanvas;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        mainCanvas = GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }


    // External Management Functions
    public void SetPanel(Vector3 pos, string text)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(pos);
        panelText.SetText(text);
        panelText.SetLayoutDirty();
        panelText.rectTransform.position = GameUtility.ClampUIElementToCanvas(panelText.rectTransform, mainCanvas, screenPos);
        panelText.SetLayoutDirty();
    }
}
