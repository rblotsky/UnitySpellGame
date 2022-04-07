using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellComponentType
{
    Shape, // Shape component - Modifies the overall shape and type of spell (eg. projectile, ward, etc.)
    Modifier, // Modifier component - Modifies any spell properties contained in PlayerSpellProperties
}
