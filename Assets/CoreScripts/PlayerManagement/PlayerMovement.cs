using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class PlayerMovement : MonoBehaviour
{
    // MOVEMENT DATA //
    // References
    private Vector3 positionToLookAt;
    private CharacterController movementController;
    private Camera mainCamera;
    private NavMeshAgent playerAgent;

    // Basic Movement Data (Should not be modified except for permanent boosts)
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public float sprintMult;
    public bool useNavAgentMovement = false;
    public float footstepLength = 3f;

    // Sound Effects
    public SoundEffect footstepSound;

    // Cached Data
    private bool footstepCompletedThisFrame = false;
    private float currentStepDistance = 0;

    // Properties
    public bool FootstepCompletedInFrame { get { return footstepCompletedThisFrame; } }


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        // Gets references
        movementController = GetComponent<CharacterController>();
        playerAgent = GetComponent<NavMeshAgent>();

        // Manages nav agent movement (for debug purposes)
        if (useNavAgentMovement)
        {
            if (movementController != null)
            {
                movementController.enabled = false;
            }

            if (playerAgent != null)
            {
                playerAgent.enabled = true;
                playerAgent.speed = moveSpeed;
            }
        }

        else
        {
            if (playerAgent != null)
            {
                playerAgent.enabled = false;
            }

            if (movementController != null)
            {
                movementController.enabled = true;
            }
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!PlayerStats.isPlayerPaused)
        {
            // Rotates player to mouse position
            RotateToMouse();

            // Moves player
            MovePlayer();
        }
    }


    // Player Movement Functions
    private void MovePlayer()
    {
        // Caches data to avoid modifying nonlocal data
        Vector3 totalMovement = new Vector3(0, 0, 0);

        // Gets movement direction from input
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            totalMovement += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            totalMovement -= new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow))
        {
            totalMovement -= new Vector3(1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
        {
            totalMovement += new Vector3(1, 0, 0);
        }

        // Normalizes
        totalMovement = totalMovement.normalized * moveSpeed;

        // Applies sprint multiplier
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalMovement *= sprintMult;
        }

        // Updates current footstep - adds distance travelled modified by delta time
        UpdateCurrentFootstep(totalMovement.magnitude * Time.deltaTime);

        // Applying gravity to the controller
        totalMovement.y -= Physics.gravity.magnitude;

        // Moves the controller
        movementController.Move(totalMovement * Time.deltaTime);

        // Plays footstep sound if necessary
        PlayFootstepSound();

        // Calculates ground level
        GameUtility.SetGroundLevel(transform.position);
    }

    private void MovePlayerAuto()
    {
        if (Input.GetMouseButton(2))
        {
            playerAgent.SetDestination(GameUtility.worldMousePosition);
        }
    }


    // Player Rotation Functions
    private void RotateToMouse()
    {
        Ray castPoint = mainCamera.ScreenPointToRay(Input.mousePosition);

        castPoint.direction = castPoint.direction;
        RaycastHit hitInfo;

        // Sets layerMask to only MouseRaycastAccept
        LayerMask groundMask = 1 << 9;

        // Casts ray - value 200 is set just in case the ground is far below the camera
        if (Physics.Raycast(castPoint, out hitInfo, 200, groundMask))
        {
            // Gets the look position and sets world mouse position
            positionToLookAt = hitInfo.point;
            GameUtility.worldMousePosition = positionToLookAt;

            // Looks at mouse position
            transform.LookAt(new Vector3(positionToLookAt.x, transform.localPosition.y, positionToLookAt.z));
        }
    }


    // Footstep Functions
    private void UpdateCurrentFootstep(float frameDistance)
    {
        // Updates that footstep didn't complete
        footstepCompletedThisFrame = false;

        // Increments by the amount travelled this frame
        currentStepDistance += frameDistance;

        // If goes past step distance, resets to 0 and sets to having completed the footstep this frame
        if(currentStepDistance > footstepLength)
        {
            currentStepDistance = 0;
            footstepCompletedThisFrame = true;
        }
    }

    private void PlayFootstepSound()
    {
        if (FootstepCompletedInFrame)
        {
            SoundEffect.TryPlaySoundEffectFromPlayer(footstepSound);
        }
    }


    // Setup Functions
    public void SetupData(float newMoveSpeed, float newSprintMult)
    {
        moveSpeed = newMoveSpeed;
        sprintMult = newSprintMult;

        playerAgent.speed = moveSpeed;
    }
}
