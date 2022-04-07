using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerFollowVCam : MonoBehaviour
{
    // DATA //
    // Scene/Object References
    public CinemachineVirtualCamera playerFollowCam;
    public PlayerComponent playerReference;

    // Basic Data
    public float sceneOpenEffectDuration = 0.5f;
    public float minZoom = 10;
    public float maxZoom = 40;

    // Constants
    public static readonly float ZOOM_MULT = 30.0f;

    // Cached Data
    private float finalFOV = 0;

    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        playerFollowCam = GetComponent<CinemachineVirtualCamera>();
        playerReference = FindObjectOfType<PlayerComponent>();

        // Adds event listeners to startup event
        playerReference.onStartUp += StartZoom;
    }

    private void OnEnable()
    {
        //StartCoroutine(UpdateTranslucentObjects());
    }

    private void OnDisable()
    {
        playerReference.onStartUp -= StartZoom;
    }

    private void Update()
    {
        // Zooms in/out depending on whether player uses zoom key
        float zoomAxis = Input.GetAxis("Mouse ScrollWheel");

        CinemachineComponentBase cameraTransposerComponent = playerFollowCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        float currentZoom;

        if (cameraTransposerComponent is CinemachineFramingTransposer)
        {
            currentZoom = (cameraTransposerComponent as CinemachineFramingTransposer).m_CameraDistance;
            (cameraTransposerComponent as CinemachineFramingTransposer).m_CameraDistance = Mathf.Clamp(currentZoom + zoomAxis * ZOOM_MULT, minZoom, maxZoom);
        }
    }


    // Zoom Effect
    private void StartZoom()
    {
        finalFOV = playerFollowCam.m_Lens.FieldOfView;
        playerFollowCam.m_Lens.FieldOfView = 0;
        StartCoroutine(ZoomOut());
    }

    private IEnumerator ZoomOut()
    {
        for (int i = 0; i < finalFOV; i++)
        {
            playerFollowCam.m_Lens.FieldOfView = i;
            yield return new WaitForSeconds(sceneOpenEffectDuration/finalFOV);
        }

        playerFollowCam.m_Lens.FieldOfView = finalFOV;
    }
}
