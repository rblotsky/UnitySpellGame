using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EventViewVCam : MonoBehaviour
{
    // DATA //
    // Scene/Object References
    public CinemachineVirtualCamera vcam;

    // Basic Data
    public float effectDuration;
    public float minZoom = 10;
    public float maxZoom = 40;

    // Constants
    public static readonly float ZOOM_MULT = 30.0f;
    public static readonly int ON_SWITCH_PRIORITY = 100;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (vcam.Priority == ON_SWITCH_PRIORITY)
        {
            // Zooms in/out depending on whether player uses zoom key
            float zoomAxis = Input.GetAxis("Mouse ScrollWheel");

            CinemachineComponentBase cameraTransposerComponent = vcam.GetCinemachineComponent(CinemachineCore.Stage.Body);
            float currentZoom;

            if (cameraTransposerComponent is CinemachineFramingTransposer)
            {
                currentZoom = (cameraTransposerComponent as CinemachineFramingTransposer).m_CameraDistance;
                (cameraTransposerComponent as CinemachineFramingTransposer).m_CameraDistance = Mathf.Clamp(currentZoom + zoomAxis * ZOOM_MULT, minZoom, maxZoom);
            }
        }
    }


    // Main Functions
    public void RunEventViewer()
    {
        StartCoroutine(EventViewer());
    }


    // Coroutines
    public IEnumerator EventViewer()
    {
        // Caches original priority, sets this to max, waits effect duration and resets to original priority
        int originalPriority = vcam.Priority;
        vcam.Priority = ON_SWITCH_PRIORITY;
        yield return new WaitForSeconds(effectDuration);
        vcam.Priority = originalPriority;
    }
}
