using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class StateBasedAI : AIBase
{
    // DATA //
    // States
    [Header("State Data")]
    public AIStateBase currentState;
    public AIStateBase[] possibleStates;

    // AI Data
    [Header("StateBasedAI Basic Data")]
    public float playerForgetTime = 2.0f;
    public float colliderAvoidanceDistance = 5f;
    public float movementDamping = 1.5f;

    // Constants
    public static readonly float STATE_CHANGE_CHECK_INTERVAL = 0.2f;
    public static readonly float DATA_CACHE_DELAY = 0.2f;

    // Cached Data
    [Header("Auto-Cached Data")]
    public float lastSeenPlayer = -10f;
    public bool canSeePlayerCached = false;
    public Vector3 lastKnownPlayerLocation = GameUtility.nullVector3;
    public List<Vector3> movementDirections = new List<Vector3>();
    public (Vector3 moveDirection, float cacheTime) lastMovementDirection = (GameUtility.nullVector3, 0);
    public PlayerComponent playerInScene;


    // OVERRIDES //
    // Basic Functions
    protected override void Awake()
    {
        // Gets attached components
        navAgent = GetComponent<NavMeshAgent>();
        combatEntity = GetComponent<CombatEntity>();
        entityRotator = GetComponent<EntityRotate>();
        movementController = GetComponent<CharacterController>();
        playerInScene = FindObjectOfType<PlayerComponent>();

        // Generates movement direction angles
        movementDirections.Clear();
        float directionAngleIncrement = 360f / totalMovementDirections;
        for (int i = 1; i <= totalMovementDirections; i++)
        {
            Vector3 direction = Quaternion.AngleAxis(i * directionAngleIncrement, Vector3.up) * Vector3.forward;
            Debug.DrawLine(transform.position, transform.position + direction, Color.cyan, 10);
            movementDirections.Add(direction);
        }

        // Base Awake   
        base.Awake();
    }

    protected override void OnEnable()
    {
        // Attempts state change
        AttemptChangeState();

        // Starts main functionality
        StartCoroutine(StateChangeChecker());
        base.OnEnable();
    }


    // FUNCTIONS //
    // Basic Functions
    private void FixedUpdate()
    {
        // Checks if can see player now
        canSeePlayerCached = perspective.CheckIfSeesPlayer(transform.position);

        // Updates player position and time seen
        if (canSeePlayerCached)
        {
            lastKnownPlayerLocation = playerInScene.GetPlayerPosition();
            lastSeenPlayer = Time.time;
        }

        // Runs the current state function
        if (currentState != null)
        {
            currentState.RunStateFunctionality(this);
        }
    }


    // Main AI Behaviourss
    public IEnumerator StateChangeChecker()
    {
        while (enabled)
        {
            // Attempts changing state
            AttemptChangeState();

            // Waits before running again
            yield return new WaitForSeconds(STATE_CHANGE_CHECK_INTERVAL);
        }
    }


    // Check Functions
    public bool ForgotPlayerSighting()
    {
        // Returns whether the time since seeing the player is higher than the time it takes to forget
        return (Time.time - lastSeenPlayer) > playerForgetTime;
    }

    public bool CanGoToLocation(Vector3 position)
    {
        // Returns whether the AI can go to the given position
        //TODO: Make this good
        return NavMesh.SamplePosition(position, out _, POSITION_REACHED_DISTANCE, 1);
    }


    // Movement
    public void MoveInDirection(Vector3 direction, bool lookInMovementDirection, float speedDamper)
    {
        // Speed = speedDamper*speed.
        Vector3 movement = direction * speedDamper * entityMoveSpeed;

        if (lookInMovementDirection)
        {
            entityRotator.StartNewLookAtPointRotation(transform.position + movement/speedDamper);
        }

        // Caches direction if necessary
        if(Time.time-lastMovementDirection.cacheTime > DATA_CACHE_DELAY)
        {
            lastMovementDirection.moveDirection = direction;
            lastMovementDirection.cacheTime = Time.time;
        }

        //Debug.DrawLine(transform.position, movement + transform.position, Color.magenta, 5);

        // Only applies gravity after to make sure it doesnt start rotating downwards.
        movement.y -= Physics.gravity.magnitude * speedDamper;
        movementController.Move(movement);
    }


    // Events
    protected virtual void AttemptChangeState()
    {
        // Runs through each state and attempts transitioning to it.
        // If it succeeds, transitions to it and breaks loop.
        // States are ordered by priority, so low priority states will only occur if higher priority ones don't.
        foreach(AIStateBase state in possibleStates)
        {
            if (state.CheckStateConditions(this))
            {
                // If it is not already current state, runs state switch code
                if(state != currentState)
                {
                    ChangeState(state);
                }

                // Even if it is current state, prevents from going to lower priority state
                break;
            }
        }
    }

    protected virtual void ChangeState(AIStateBase newState)
    {
        // Clears rotation and movement
        entityRotator.ClearRotation();

        // Sets current state to new state
        currentState = newState;
    }
}
