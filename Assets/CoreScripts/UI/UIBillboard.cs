using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    // DATA //
    // Cached Data
    private Transform mainCameraTransfrom;
    private PlayerComponent player;

    // FUNCTIONS //
    // Basic Functions
    void Start()
    {
        player = DataRef.playerReference;

        if (player == null)
        {
            player = FindObjectOfType<PlayerComponent>();

            if (player.sceneMainCamera == null)
            {
                mainCameraTransfrom = Camera.main.transform;
            }

            else
            {
                mainCameraTransfrom = player.sceneMainCamera.transform;
            }
        }

        else
        {
            if (player.sceneMainCamera == null)
            {
                mainCameraTransfrom = Camera.main.transform;
            }

            else
            {
                mainCameraTransfrom = player.sceneMainCamera.transform;
            }
        }

        transform.LookAt(transform.position + mainCameraTransfrom.forward);
    }

    void LateUpdate()
    {
        // Rotates to look at camera
        transform.LookAt(transform.position + mainCameraTransfrom.forward);
    }
}
