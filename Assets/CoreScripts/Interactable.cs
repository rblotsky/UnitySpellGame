using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class Interactable : MonoBehaviour, IDataPersistentComponent
{
    // BASE DATA //
    // Basic Info
    [Header("Base Interactable Data")]
    public Texture2D hoverCursor;
    public Color hoverColour = Color.green;
    public int maxUses = 99999;
    public UnityEvent OnInteract;

    // Constants
    public static readonly float MAX_INTERACT_DISTANCE = 100f;
    public static readonly string INTERACT_PLAYER_TAG = "Player";

    // Cache Data
    private int numUses = 0;
    private MeshRenderer[] meshRenderers;
    private Dictionary<MeshRenderer, Color> rendererColours = new Dictionary<MeshRenderer, Color>();


    // FUNCTIONS //
    // Basic Functions
    protected virtual void Awake()
    {
        // Gets all mesh renderers in children
        meshRenderers = GetComponentsInChildren<MeshRenderer>();

        // Stores default colour for each renderer
        foreach(MeshRenderer renderer in meshRenderers)
        {
            // If the renderer is not already added, adds it
            if (!rendererColours.ContainsKey(renderer))
            {
                rendererColours.Add(renderer, renderer.material.color);
            }
        }
    }

    private void OnMouseOver()
    {
        // Runs base code if the player isn't viewing a menu and isnt paused
        if (PlayerStats.isPlayerPaused)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            RemoveHoverColour();
            return;
        }

        if (!DataRef.sceneMenuManagerReference.IsViewingMenu)
        {
            // Sets hover effects
            Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
            AddHoverColour();

            // Checks if the player right clicks
            if (Input.GetMouseButtonDown(1))
            {
                // Does nothing if the numUses goes over maxUses
                if(numUses >= maxUses)
                {
                    return;
                }

                // Raycasts from player to object, interacts if the object isn't blocked
                RaycastHit hit;
                Debug.DrawRay(DataRef.playerReference.transform.position, transform.position - DataRef.playerReference.transform.position, Color.red, 2f);
                if (Physics.Raycast(DataRef.playerReference.transform.position, transform.position - DataRef.playerReference.transform.position, out hit, MAX_INTERACT_DISTANCE))
                {
                    Debug.Log(hit.collider.name);

                    // Interacts if the collided object is this object or one of its children
                    if (hit.collider.transform.IsChildOf(transform))
                    {
                        Interact();
                        OnInteract.Invoke();
                        numUses++;

                        //TODO: Add a cool particle animation that plays, maybe a line of particles toward the player
                    }
                }
            }
        }

        // Otherwise, removes hover effects
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            RemoveHoverColour();
        }
    }

    private void OnMouseExit()
    {
        // Resets hover effects
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        RemoveHoverColour();
    }

    private void OnDisable()
    {
        // Resets cursor when destroyed
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }


    // Virtual Functions
    protected virtual void Interact()
    {
        // Base Interact Class does nothing
        return;
    }


    // Interface Functions
    public string SaveDataToString()
    {
        string savedData = "";
        savedData += numUses.ToString();
        return savedData;
    }

    public void LoadDataFromString(string data)
    {
        int.TryParse(data.Trim(), out numUses);
    }

    // Hover Effect Functions
    protected virtual void AddHoverColour()
    {
        // Sets the colour of each renderer to the hover colour
        foreach(MeshRenderer renderer in meshRenderers)
        {
            renderer.material.color = hoverColour;
        }
    }

    protected virtual void RemoveHoverColour()
    {
        // Resets each renderer's colour to default stored in rendererColours
        foreach(MeshRenderer renderer in meshRenderers)
        {
            // If the renderer is in the dictionary, resets its colour
            if (rendererColours.ContainsKey(renderer))
            {
                renderer.material.color = rendererColours[renderer];
            }
        }
    }

}
