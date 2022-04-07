using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;

public static class GameUtility 
{
    // STATIC DATA //
    // Properties
    public static float groundLevel { get; set; }
    public static Vector3 worldMousePosition { get; set; }

    // Constants
    public static readonly Vector3 nullVector3 = new Vector3(0f, 999f, 0f);
    public static readonly float SPELL_ELEMENT_MODIFIER = 0.05f;
    public static readonly float ELEMENTAL_KNOCKBACK_FORCE_MULTIPLIER = 2;
    public static readonly float COLLISION_DAMAGE_IMPULSE_DIVISOR = 10;


    // STATIC FUNCTIONS //
    // Damage Calculations
    public static CombatEffects CalculateDefense(CombatEffects damages, CombatEffects defenses)
    {
        // Calculates the damage dealt per element and returns it
        CombatEffects returnElements = new CombatEffects(
            CalculateSingleEffectDefense(damages.healthInstant, defenses.healthInstant),
            CalculateSingleEffectDefense(damages.healthChange, defenses.healthChange),
            CalculateSingleEffectDefense(damages.speedForDuration, defenses.speedForDuration),
            CalculateSingleEffectDefense(damages.effectDuration, defenses.effectDuration));
        
        return returnElements;
    }

    public static int CalculateSingleEffectDefense(int effectValue, int defenseAmount)
    {
        // If negative ('damage') then adds defense amount and clamps so it doesnt 'heal' or deal more than 2x original 'damage'
        if(effectValue < 0)
        {
            return Mathf.Clamp(effectValue - defenseAmount, effectValue * 2, 0);
        }

        // If positive, shouldn't add defense to it and just returns default.
        return effectValue;
    }


    // Location Calculations
    public static float CalculateSinglePlaneDistance(Vector3 firstPos, Vector3 secondPos)
    {
        float startx = firstPos.x;
        float startz = firstPos.z;

        float endx = secondPos.x;
        float endz = secondPos.z;

        float result = Mathf.Pow((endx - startx), 2f);
        result += Mathf.Pow((endz - startz), 2f);

        //Debug.DrawLine(firstPos, new Vector3(secondPos.x, firstPos.y, secondPos.z), Color.green, 0.1f);

        return Mathf.Sqrt(result);
    }   
        
    public static Vector3 ExtendLineToLocationSinglePlane(Vector3 startPos, Vector3 endPos, float amountToExtend)
    {
        // Extends a line between two points either forward or backwards.
        // Can be used for AI to decide destinations.
        // Line is extended along X/Z Axis, directed away from endPos.

        float lengthAB = CalculateSinglePlaneDistance(startPos, endPos);
        float lengthBC = amountToExtend;
        float lengthAD = endPos.x - startPos.x;
        float lengthBE = (lengthBC * lengthAD) / lengthAB;

        float lengthBD = endPos.z - startPos.z;
        float lengthCE = (lengthBC * lengthBD) / lengthAB;

        float xCoordinate = endPos.x + lengthBE;
        float zCoordinate = endPos.z + lengthCE;


        Vector3 returnValue = new Vector3(xCoordinate, endPos.y, zCoordinate);
        //Debug.DrawLine(startPos, returnValue, Color.red, 0.1f);

        return returnValue;
    }

    public static Vector3 ExtendLineToLocation(Vector3 startPos, Vector3 endPos, float amountToExtend)
    {
        Vector3 direction = endPos-startPos;
        direction.Normalize();
        Debug.DrawLine(startPos, (startPos + (direction * amountToExtend)), Color.magenta, Time.deltaTime);
        return (endPos + (direction * amountToExtend));
    }

    public static float DirAttractToPoint(Vector3 dir, Vector3 pos, Vector3 point, float optimalDistance, float maxValue)
    {
        float dot = Vector3.Dot(dir, (point - pos).normalized);
        float distance = Vector3.Distance(pos, point);
        float distanceMultiplier = Mathf.Clamp(distance - optimalDistance, -1, maxValue);
        return dot * distanceMultiplier;
    }

    public static float DirAttractToPoint(Vector3 dir, Vector3 pos, Vector3 point, float optimalDistance, float maxValue, float damping)
    {
        float dot = Vector3.Dot(dir, (point - pos).normalized);
        float distanceDamped = Vector3.Distance(pos, point)/damping;
        float distanceMultiplier = Mathf.Clamp(distanceDamped - optimalDistance, -1, maxValue);
        return dot * distanceMultiplier;
    }

    public static float DirAttractToPoint(Vector3 dir, Vector3 pos, Vector3 point, float distance, float optimalDistance, float maxValue, float damping)
    {
        float dot = Vector3.Dot(dir, (point - pos).normalized);
        float distanceDamped = (distance - dot) / damping;
        float distanceMultiplier = Mathf.Clamp(distanceDamped - optimalDistance, -1, maxValue);
        return dot * distanceMultiplier;
    }


    // Physics Calculations
    public static float CalculateKnockbackForce(CombatEffects damages)
    {
        return damages.healthInstant * ELEMENTAL_KNOCKBACK_FORCE_MULTIPLIER;
    }

    public static void BasicPhysicsExplosion(Vector3 position, float radius, CombatEffects elements, DamageType[] damageTypes, IAttacker explodingObject)
    {
        // Gets colliders within explosion radius
        Collider[] initialExplosionColliders = Physics.OverlapSphere(position, radius, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide);

        // Applies force and damage to each collider
        foreach (Collider collider in initialExplosionColliders)
        {
            if (collider.attachedRigidbody != null)
            {
                collider.attachedRigidbody.AddForce((collider.transform.position - position) * CalculateKnockbackForce(elements), ForceMode.Impulse);
            }

            CombatEntity combatEntity = collider.GetComponent<CombatEntity>();

            if (combatEntity != null)
            {
                combatEntity.ApplyEffects(elements, position, damageTypes, false, explodingObject);
            }
        }

        // After force and damage is applied, gets colliders within radius again (if an object shattered, this will apply force to its fragments)
        Collider[] secondExplosionColliders = Physics.OverlapSphere(position, radius);

        foreach (Collider collider in secondExplosionColliders)
        {
            // Applies explosion force only to objects not present in initial explosion
            if (!initialExplosionColliders.Contains(collider))
            {
                if (collider.attachedRigidbody != null)
                {
                    collider.attachedRigidbody.AddForce((collider.transform.position - position) * CalculateKnockbackForce(elements), ForceMode.Impulse);
                }
            }
        }
    }
    

    // Ground Level
    public static void SetGroundLevel(Vector3 referencePoint)
    {
        Ray rayToGround = new Ray(referencePoint, new Vector3(referencePoint.x, referencePoint.y-100, referencePoint.z));

        RaycastHit hitInfo;

        // Sets layerMask to only MouseRaycastAccept (So it only registers the ground)
        int maskToUse = 1 << 9;
        if (Physics.Raycast(rayToGround, out hitInfo, maskToUse))
        {
            // Sets the new ground level if the ray hits anything
            groundLevel = hitInfo.point.y;
        }
    }

    public static Vector3 SetVectorToGroundLevel(Vector3 vector)
    {
        return new Vector3(vector.x, groundLevel, vector.z);
    }

    public static Vector3 SetSpecificAxis(Vector3 baseVector, Vector3 axisMultiplier, Vector3 axisNewValues)
    {
        baseVector = Vector3.Scale(baseVector, axisMultiplier);
        return baseVector + axisNewValues;
    }


    // String Editing
    public static string GenerateHTMLColouredText(string text, Color colour)
    {
        // Generates HTML coloured text
        string colourHexValue = "#" + ColorUtility.ToHtmlStringRGBA(colour);
        return "<color=" + colourHexValue + ">" + text + "</color>";
    }


    // General Utility Functions
    public static Vector3 ParseVector3(string stringVector)
    {
        // Removes parentheses
        if (stringVector.StartsWith("(") && stringVector.EndsWith(")"))
        {
            stringVector = stringVector.Substring(1, stringVector.Length - 2);
        }

        // Parses data from inside parentheses
        string[] stringValues = stringVector.Split(',');

        Vector3 result = new Vector3(
            float.Parse(stringValues[0]), 
            float.Parse(stringValues[1]), 
            float.Parse(stringValues[2]));

        // Returns parsed value
        return result;
    }

    public static Vector3 ClampUIElementToCanvas(RectTransform element, Canvas canvas, Vector3 newPos)
    {
        float minX = (element.rect.size.x * canvas.scaleFactor * element.pivot.x);
        float maxX = (canvas.pixelRect.size.x - (element.rect.size.x * canvas.scaleFactor * element.pivot.x));
        float minY = (element.rect.size.y * canvas.scaleFactor * element.pivot.y);
        float maxY = (canvas.pixelRect.size.y - (element.rect.size.y * canvas.scaleFactor * Mathf.Abs(element.pivot.y-1)));

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        // Sets position and dirties layout
        return newPos;
    }
}
