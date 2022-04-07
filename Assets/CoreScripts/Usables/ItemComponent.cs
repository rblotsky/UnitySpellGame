using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[DisallowMultipleComponent]
public class ItemComponent : Interactable
{
    // DATA //
    // References
    public Usable item;
    public TextMeshProUGUI nameTextUI;
    public GameObject nameTextPrefab;
    private Collider objectCollider;

    // Data
    public float nameTextVertOffset = 1f;
    public string onPickUpQuestUpdate;

    // UnityEvents
    public UnityEvent OnPickUp;


    // OVERRIDES //
    protected override void Interact()
    {
        PickUp();
    }


    // FUNCTIONS //
    // Basic Functions
    private void Start()
    {
        objectCollider = GetComponentInChildren<Collider>();

        // Instantiates the name text prefab
        if (nameTextPrefab != null)
        {
            // Calculates the uppermost part of the collider
            Vector3 highAbove = new Vector3(transform.position.x, transform.position.y + 50, transform.position.z);
            Vector3 topOfCollider = objectCollider.ClosestPointOnBounds(highAbove);

            // Instantiates name text above that point
            GameObject nameTextObject = Instantiate(nameTextPrefab, topOfCollider, nameTextPrefab.transform.rotation, transform);
            Vector3 nameTextPosition = nameTextObject.transform.position;
            nameTextPosition.y += nameTextVertOffset;
            nameTextObject.transform.position = nameTextPosition;
            Debug.Log("Successfully instantiated nameTextPrefab for " + name + "!", gameObject);
        }

        // Tries getting name text UI
        if (nameTextUI == null)
        {
            nameTextUI = GetComponentInChildren<TextMeshProUGUI>();
        }

        // Sets name text UI to item name
        if (nameTextUI != null)
        {
            nameTextUI.SetText(item.GetItemName());
        }

        // If the item is an Artifact, adds the artifact adding event to OnPickUp
        if (item is Artifact)
        {
            Artifact artifactItem = (Artifact)item;
            OnPickUp.AddListener(artifactItem.DiscoverArtifact);
        }
    }


    // Main Functionality
    public void PickUp()
    {
        // Tries adding item to inventory
        if (PlayerEquipment.main.AddItemToFirstEmptySlot(item, true))
        {
            // Runs event
            if (OnPickUp != null)
            {
                OnPickUp.Invoke();
            }

            Quests.UpdateStages(onPickUpQuestUpdate, true, gameObject);
            Destroy(this.gameObject);
        }
    }
}
