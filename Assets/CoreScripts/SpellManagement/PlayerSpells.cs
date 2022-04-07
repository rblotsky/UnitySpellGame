using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

public class PlayerSpells 
{
    [Serializable]
    public class PlayerSpellsStringSerialized
    {
        public List<string> availableComponentNames = new List<string>();
        public PlayerSpellObjectStringSerialized spell1String;
        public PlayerSpellObjectStringSerialized spell2String;
    }

    [Serializable]
    public class PlayerSpellObjectStringSerialized
    {
        public string shapeComponentName;
        public List<string> modifierNames = new List<string>();
    }


    // SPELLS DATA //
    // Owned Component Lists
    public List<PlayerSpellComponent> availableComponents = new List<PlayerSpellComponent>();

    // Current Spells
    public PlayerSpellObject spell1 = new PlayerSpellObject();
    public PlayerSpellObject spell2 = new PlayerSpellObject();

    // Constants
    public static Color COMPONENT_LEARN_NOTIFICATION_COLOUR = Color.cyan;
    public static Color COMPONENT_LEARN_FAIL_NOTIFICATION_COLOUR = Color.red;

    // Singleton Pattern
    public static PlayerSpells main;


    // FUNCTIONS //
    // Component Editing
    public bool LearnNewComponent (PlayerSpellComponent componentToLearn)
    {
        // Adds the component to master list
        if (!HasComponent(componentToLearn))
        {
            // Adds to available components
            availableComponents.Add(componentToLearn);

            // Displays notification
            if (DataRef.overlayManagerReference != null)
            {
                DataRef.overlayManagerReference.DisplayNotification("You have learned a \"" + componentToLearn.DisplayName + "\" spell component!", COMPONENT_LEARN_NOTIFICATION_COLOUR);
            }

            return true;
        }

        else
        {
            if (DataRef.overlayManagerReference != null)
            {
                DataRef.overlayManagerReference.DisplayNotification("You already know this \"" + componentToLearn.DisplayName + "\" spell component!", COMPONENT_LEARN_FAIL_NOTIFICATION_COLOUR);
            }

            return false;
        }
    }

    public bool HasComponent(PlayerSpellComponent component)
    {
        return availableComponents.Contains(component);
    }

    public void AddComponent(PlayerSpellComponent newComponent, SpellComponentType type, int spellNum, int slotIndex)
    {
        // Creates a reference to a PlayerSpellObject to modify it
        PlayerSpellObject spellToEdit;

        // Chooses correct spell to reference
        if(spellNum == 1)
        {
            spellToEdit = spell1;
        }

        else
        {
            spellToEdit = spell2;
        }

        // Adds the component to the correct slot
        if(type == SpellComponentType.Shape)
        {
            Debug.Log("PlayerSpells added Shape Component!");
            spellToEdit.shapeComponent = (SpellShapeComponent)newComponent;
        }

        else
        {
            Debug.Log("PlayerSpells added Modifier Component!");

            // Makes sure slot where it wants to add the component exists before adding it
            if (slotIndex < spellToEdit.spellModifiers.Count)
            {
                spellToEdit.spellModifiers[slotIndex] = (SpellModifierComponent)newComponent;
            }

            else
            {
                spellToEdit.spellModifiers.Add((SpellModifierComponent)newComponent);
            }
        }
    }

    // For use in loading function, adds component to first null slot of its type
    public void LoadingAddComponent(PlayerSpellComponent component, SpellComponentType componentType, int spellNum)
    {
        // Checks type of component, adds to correct slot in spell 
        if (componentType == SpellComponentType.Shape)
        {
            if(spellNum == 1)
            {
                spell1.shapeComponent = (SpellShapeComponent)component;
                return;
            }

            else if (spellNum == 2)
            {
                spell2.shapeComponent = (SpellShapeComponent)component;
                return;
            }

            else
            {
                Debug.LogError("[PlayerSpells] LoadingAddComponent given invalid spellNum: " + spellNum);
                return;
            }
        }

        else
        {
            if (spellNum == 1)
            {
                spell1.spellModifiers.Add((SpellModifierComponent)component);
                return;
            }

            else if (spellNum == 2)
            {
                spell2.spellModifiers.Add((SpellModifierComponent)component);
                return;
            }

            else
            {
                Debug.LogError("[PlayerSpells] LoadingAddComponent given invalid spellNum: " + spellNum);
                return;
            }
        }
    }


    // Data Management
    public PlayerSpellsStringSerialized SerializeToStringVersion()
    {
        PlayerSpellsStringSerialized stringData = new PlayerSpellsStringSerialized();

        // Saves available components
        foreach(PlayerSpellComponent component in availableComponents)
        {
            stringData.availableComponentNames.Add(component.id.ToString());
        }

        // Saves spell 1
        PlayerSpellObjectStringSerialized stringSpell1 = new PlayerSpellObjectStringSerialized();

        if (spell1.shapeComponent != null)
        {
            stringSpell1.shapeComponentName = spell1.shapeComponent.id.ToString();
        }

        else
        {
            stringSpell1.shapeComponentName = "none";
        }

        foreach(SpellModifierComponent component in spell1.spellModifiers)
        {
            if (component != null)
            {
                stringSpell1.modifierNames.Add(component.id.ToString());
            }

            else
            {
                stringSpell1.modifierNames.Add("none");
            }
        }

        stringData.spell1String = stringSpell1;

        // Saves spell 2
        PlayerSpellObjectStringSerialized stringSpell2 = new PlayerSpellObjectStringSerialized();

        if (spell2.shapeComponent != null)
        {
            stringSpell2.shapeComponentName = spell2.shapeComponent.id.ToString();
        }

        else
        {
            stringSpell2.shapeComponentName = "none";
        }

        foreach (SpellModifierComponent component in spell2.spellModifiers)
        {
            if (component != null)
            {
                stringSpell2.modifierNames.Add(component.id.ToString());
            }

            else
            {
                stringSpell2.modifierNames.Add("none");
            }
        }

        stringData.spell2String = stringSpell2;

        return stringData;
    }

    public void DeserializeFromStringVersion(PlayerSpellsStringSerialized stringData)
    {
        if (stringData.availableComponentNames != null)
        {
            foreach (string name in stringData.availableComponentNames)
            {
                availableComponents.Add(SpellComponentList.GetSpellComponent(name));
            }
        }

        spell1.shapeComponent = (SpellShapeComponent)SpellComponentList.GetSpellComponent(stringData.spell1String.shapeComponentName);
        if (stringData.spell1String.modifierNames != null)
        {
            foreach (string name in stringData.spell1String.modifierNames)
            {
                spell1.spellModifiers.Add((SpellModifierComponent)SpellComponentList.GetSpellComponent(name));
            }
        }

        spell2.shapeComponent = (SpellShapeComponent)SpellComponentList.GetSpellComponent(stringData.spell2String.shapeComponentName);
        if (stringData.spell2String.modifierNames != null)
        {
            foreach (string name in stringData.spell2String.modifierNames)
            {
                spell2.spellModifiers.Add((SpellModifierComponent)SpellComponentList.GetSpellComponent(name));
            }
        }
    }

    public Queue<string> SaveDataToStringQueue(Formatting format = Formatting.None)
    {
        Queue<string> playerSpellsLines = new Queue<string>();
        playerSpellsLines.Enqueue(JsonConvert.SerializeObject(SerializeToStringVersion(), format));
        return playerSpellsLines;
    }

    public void LoadDataFromText(string saveFileText)
    {
        // Since there is only one object being saved to the file, runs on entire file text
        DeserializeFromStringVersion(JsonConvert.DeserializeObject<PlayerSpellsStringSerialized>(saveFileText));
    }


    // Reset Function
    public void SetupFunction()
    {
        // Clears everything to default values
        availableComponents.Clear();
        spell1 = new PlayerSpellObject();
        spell2 = new PlayerSpellObject();
    }


    // Singleton Pattern
    public static void SetMainReference()
    {
        main = new PlayerSpells();
        main.SetupFunction();
    }
}
