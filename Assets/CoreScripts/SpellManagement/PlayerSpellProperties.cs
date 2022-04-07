using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerSpellProperties
{
    // PROPERTY DATA //
    public float duration;
    public float radius;
    public float amount;
    public float speed;
    public CombatEffects elements;
    public DamageType[] damagedEntities;
    public SpellKeyword[] keywords;


    // CONSTRUCTORS //
    public PlayerSpellProperties(float dur, float rad, float amt, DamageType[] damageTypes, CombatEffects newElements, SpellKeyword[] newKeywords, float newSpeed)
    {
        duration = dur;
        radius = rad;
        amount = amt;
        speed = newSpeed;
        damagedEntities = damageTypes;
        elements = newElements;
        keywords = newKeywords;
    }


    // FUNCTIONS //
    // Combining Functions
    public PlayerSpellProperties Add(PlayerSpellProperties other)
    {
        // Adds basic properties
        duration += other.duration;
        radius += other.radius;
        amount += other.amount;
        speed += other.speed;
        elements.Add(other.elements);

        // Adds damagetype property
        List<DamageType> newDamageTypes = new List<DamageType>();
        foreach (DamageType type in damagedEntities)
        {
            newDamageTypes.Add(type);
        }

        if (other.damagedEntities != null)
        {
            foreach (DamageType type in other.damagedEntities)
            {
                if (!newDamageTypes.Contains(type))
                {
                    newDamageTypes.Add(type);
                }
            }
        }

        damagedEntities = newDamageTypes.ToArray();

        // Adds keyword property
        List<SpellKeyword> newKeywords = new List<SpellKeyword>();
        foreach (SpellKeyword type in keywords)
        {
            newKeywords.Add(type);
        }

        if (other.keywords != null)
        {
            foreach (SpellKeyword type in other.keywords)
            {
                if (!newKeywords.Contains(type))
                {
                    newKeywords.Add(type);
                }
            }
        }

        keywords = newKeywords.ToArray();

        return this;
    }

    public static PlayerSpellProperties AddProperties(PlayerSpellProperties obj1, PlayerSpellProperties obj2)
    {
        PlayerSpellProperties returnedProperties = new PlayerSpellProperties();

        // Adds basic properties
        returnedProperties.duration = obj1.duration + obj2.duration;
        returnedProperties.radius = obj1.radius + obj2.radius;
        returnedProperties.amount = obj1.amount + obj2.amount;
        returnedProperties.speed = obj1.speed + obj2.speed;
        returnedProperties.elements = CombatEffects.AddElements(obj1.elements, obj2.elements);

        // Adds damagetype property
        List<DamageType> newDamageTypes = new List<DamageType>();
        foreach(DamageType type in obj1.damagedEntities)
        {
            newDamageTypes.Add(type);
        }

        foreach (DamageType type in obj1.damagedEntities)
        {
            if (!newDamageTypes.Contains(type))
            {
                newDamageTypes.Add(type);
            }
        }

        returnedProperties.damagedEntities = newDamageTypes.ToArray();

        // Adds keyword property
        List<SpellKeyword> newKeywords = new List<SpellKeyword>();
        foreach (SpellKeyword type in obj1.keywords)
        {
            newKeywords.Add(type);
        }

        foreach (SpellKeyword type in obj2.keywords)
        {
            if (!newKeywords.Contains(type))
            {
                newKeywords.Add(type);
            }
        }

        returnedProperties.keywords = newKeywords.ToArray();

        // Returns
        return returnedProperties;
    }


    // Utility Functions
    public PlayerSpellProperties ClampToAllowedRanges()
    {
        //TODO: Have this clamp most properties to postive values and some other clamping for other properties
        return this;
    }

    public string GetIngamePropertyDescription()
    {
        string returnValue = "";

        if(duration != 0)
        {
            returnValue += "Duration: " + duration + "\n";
        }

        if (radius != 0)
        {
            returnValue += "Radius: " + radius + "\n";
        }

        if (amount != 0)
        {
            returnValue += "Amount: " + amount + "\n";
        }

        if (speed != 0)
        {
            returnValue += "Speed: " + speed + "\n";
        }

        if (elements.TotalElements != 0)
        {
            returnValue += "On Hit: " + "\n" + elements.GetDisplayText(true) + "\n";
        }

        if(damagedEntities.Length > 0)
        {
            returnValue += "Damage Types: ";
            foreach(DamageType type in damagedEntities)
            {
                returnValue += type.ToString() + ", ";
            }

            // Removes the last 2 letters (the comma and space at the end) before adding a newline
            returnValue.Remove(returnValue.Length - 2);
            returnValue += "\n";
        }

        if (keywords.Length > 0)
        {
            returnValue += "Keywords: ";
            foreach (SpellKeyword keyword in keywords)
            {
                returnValue += keyword.displayName + ", ";
            }

            // Removes the last 2 letters (the comma and space at the end) before adding a newline
            returnValue.Remove(returnValue.Length - 2);
            returnValue += "\n";
        }

        return returnValue;
    }
}
