using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

[CreateAssetMenu(fileName = "New Charm", menuName = "Usables/Charm")]
public class Charm : Usable
{
    // DATA //
    // References
    public GameObject charmPrefab;

    // Data
    public bool attractor = true;
    [Tooltip("Radius of effect")]
    public float distance = 1f;
    [Tooltip("The distance forwards where the charm is placed when used.")]
    public float placeDistance = 2f;
    public float duration = 5f;


    // FUNCTIONS //
    // Basic Functions
    protected override void Awake()
    {
        itemType = UsableType.Charm;
    }


    // Item Management
    public override bool UseItem(PlayerComponent playerToUse)
    {
        if (charmPrefab != null)
        {
            GameObject charmObj = Instantiate(charmPrefab, playerToUse.GetPlayerPosition() + playerToUse.transform.forward * placeDistance, charmPrefab.transform.rotation);
            CharmBehaviour charmComponent = charmObj.GetComponent<CharmBehaviour>();
            charmComponent.attractor = attractor;
            charmComponent.distance = distance;
            charmComponent.duration = duration;
            charmComponent.ActivateCharm();

            return true;
        }

        return false;
    }

    public override string GetItemName()
    {
        if (attractor)
        {
            return "Attraction Charm";
        }

        else
        {
            return "Repulsion Charm";
        }
    }

    public override string GetItemDescription()
    {
        StringBuilder description = new StringBuilder();

        // Attraction/Repulsion
        description.Append("Type: ");
        if (attractor)
        {
            description.Append("Attractor\n (Creatures are drawn toward)\n");
        }

        else
        {
            description.Append("Repulsor\n (Creatures are pushed away)\n");
        }

        // Distance/Duration
        description.Append("\nEffective Distance: " + distance);
        description.Append("\nDuration: " + duration);

        // Returns Description
        return description.ToString();

    }
}
