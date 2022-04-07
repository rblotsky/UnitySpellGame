using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableDoor : MonoBehaviour, IDataPersistentComponent
{
    // DATA //
    // Basic Data
    public Interactable[] locks;
    public Transform openDoorPosition;
    public int requiredUnlocks = 1;
    [HideInInspector]
    public int completedUnlocks = 0;
    public float openAnimationLength = 2;
    public float animationIterationLength = 0.02f;

    // Constants
    public static readonly float CLOSE_ENOUGH_TO_OPEN_POSITION = 0.1f;


    // FUNCTIONS //
    // Basic Functions
    private void Awake()
    {
        if(locks.Length != 0)
        {
            requiredUnlocks = locks.Length;
        }
    }

    private void Start()
    {
        foreach (Interactable item in locks)
        {
            item.OnInteract.AddListener(AddUnlock);
        }
    }


    // Main Functionality
    public void AddUnlock()
    {
        completedUnlocks++;

        if(completedUnlocks >= requiredUnlocks)
        {
            StartCoroutine(OpenDoorAnimation());
        }
    }

    public IEnumerator OpenDoorAnimation()
    {
        // Splits movement into manageable intervals
        Vector3 goalPosition = openDoorPosition.position;
        float numIterations = openAnimationLength / animationIterationLength;
        float distancePerIteration = (goalPosition - transform.position).magnitude / numIterations;
        Debug.Log("Distance per: " + distancePerIteration);
        Debug.Log("Num iterations: " + numIterations);

        // Opens slowly
        for(int i = 0; i < numIterations; i++)
        {
            transform.position = GameUtility.ExtendLineToLocation(transform.position, goalPosition, -1*(Vector3.Distance(transform.position, goalPosition)-distancePerIteration));
            yield return new WaitForSeconds(animationIterationLength);
        }

        gameObject.SetActive(false);
        yield return null;
    }


    // Interface Functions
    public string SaveDataToString()
    {
        return completedUnlocks.ToString();
    }

    public void LoadDataFromString(string data)
    {
        int.TryParse(data.Trim(), out completedUnlocks);
    }
}
