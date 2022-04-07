using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BaseAIPerspective : MonoBehaviour
{
    // PERSPECTIVE DATA //    
    // Cached Data
    private bool isPlayerInPerspective = false;
    private PlayerComponent player;

    // Constants
    public static readonly string IGNORED_LAYER = "CombatEntity";


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        player = FindObjectOfType<PlayerComponent>();
    }

    private void OnDestroy()
    {
        isPlayerInPerspective = false;

    }

    private void OnDisable()
    {
        isPlayerInPerspective = false;
    }


    // Adding/Removing items from collider list
    public void OnTriggerEnter(Collider seenObject)
    {
        if (seenObject.CompareTag("Player"))
        {
            isPlayerInPerspective = true;
        }
    }

    private void OnTriggerExit(Collider seenObject)
    {
        if (seenObject.CompareTag("Player"))
        {
            isPlayerInPerspective = false;
        }
    }
 
    
    // Checking
    public bool CheckIfSeesPlayer(Vector3 entityPos)
    {
        if (isPlayerInPerspective)
        {
            // Checks if the perspective can see the player using raycast
            Vector3 directionToPlayer = player.GetPlayerPosition() - entityPos;
            int layerMask = ~LayerMask.GetMask(IGNORED_LAYER);
            RaycastHit raycastInfo;
            if (Physics.Raycast(entityPos, directionToPlayer, out raycastInfo, 99, layerMask))
            {
                //Debug.DrawLine(entityPos, raycastInfo.point, Color.red, 0.2f);
                // Runs see or lose player according to whether it sees it or not
                return raycastInfo.collider.CompareTag("Player");
            }
        }

        return false;
    }
}
