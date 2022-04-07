using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

[DisallowMultipleComponent]
public class EntityRotate : MonoBehaviour
{
    // DATA //
    // References

    // Rotation Data
    [Tooltip("Degrees rotated per per increment")]
    public float rotationSpeed = 2;
    [Tooltip("Assign a value of 1 to the axis entity should rotate around, and 0 to all others.")]
    public Vector3 rotationAxis = new Vector3(1, 0, 1);

    // Constants
    public static float ROTATION_COROUTINE_INCREMENT_DELAY = 0.03f;
    public static readonly float NEAR_ENOUGH_TO_GOAL_DEGREES = 2.0f;

    // Cached Data
    protected Queue<Quaternion> goalRotations = new Queue<Quaternion>();
    protected Coroutine currentRotator = null;

    // Properties
    public bool IsRotating { get { return goalRotations.Count == 0; } }
    public Quaternion GoalRotation 
    { 
        get 
        {
            if (goalRotations.Count > 0)
            {
                return goalRotations.Peek();
            }
            else
            {
                return Quaternion.identity;
            }
        } 
    }


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        currentRotator = null;
    }

    private void OnEnable()
    {
        // Clears all rotations
        ClearRotation();

        if(currentRotator == null)
        {
            currentRotator = StartCoroutine(MainRotator());
        }
    }

    private void OnDisable()
    {
        // Clears all rotation data
        ClearRotation();

        if(currentRotator != null)
        {
            StopCoroutine(currentRotator);
            currentRotator = null;
        }
    }

    private void Update()
    {
        if (currentRotator == null)
        {
            Debug.LogWarning("WARNING: NULL CURRENTROTATOR ON " + name);
        }
    }


    // External Rotation Starting Functions
    public void StartNewLookAtPointRotation(Vector3 point)
    {
        // Sets the goal rotation according to the look at position
        goalRotations.Clear();
        goalRotations.Enqueue(GetRotation(point));
    }

    public void StartNewBasicRotation(float rotationAroundAxis)
    {
        // Sets the goal rotation according to the look at position
        goalRotations.Clear();
        goalRotations.Enqueue(GetRotation(rotationAroundAxis));
    }

    public void StartNewLookAroundRotation(float clockwiseRotation, float counterClockwiseRotation)
    {
        // Clears and sets new goal rotations
        goalRotations.Clear();
        goalRotations.Enqueue(GetRotation(clockwiseRotation));
        goalRotations.Enqueue(GetRotation(counterClockwiseRotation));
    }

    public void AddRotation(Vector3 point)
    {
        goalRotations.Enqueue(GetRotation(point));
    }

    public void AddRotation(float degrees)
    {
        goalRotations.Enqueue(GetRotation(degrees));
    }


    // Rotation Management Functions
    public void ClearRotation()
    {
        // Clears all cached rotation data
        goalRotations.Clear();
    }


    // Internal Functions
    protected Quaternion GetRotation(float axisRotation)
    {
        return Quaternion.AngleAxis(axisRotation, rotationAxis);
    }

    protected Quaternion GetRotation(Vector3 locationToLookAt)
    {
        locationToLookAt.y = transform.position.y;
        return Quaternion.LookRotation(locationToLookAt - transform.position, Vector3.up);
    }

    protected virtual IEnumerator MainRotator()
    {
        while (enabled)
        {
            // Keeps rotating while it isn't at goal rotation
            if (goalRotations.Count > 0)
            {
                // Rotates using RotateTowards
                transform.rotation = Quaternion.RotateTowards(transform.rotation, GoalRotation, rotationSpeed);

                // If at goal rotation, dequeues it
                if (Quaternion.Angle(transform.rotation, GoalRotation) <= NEAR_ENOUGH_TO_GOAL_DEGREES)
                {
                    goalRotations.Dequeue();
                }

                // Waits for increment delay
                yield return new WaitForSeconds(ROTATION_COROUTINE_INCREMENT_DELAY);
            }
            
            yield return new WaitForSeconds(ROTATION_COROUTINE_INCREMENT_DELAY);
        }

        currentRotator = null;
    }

}
