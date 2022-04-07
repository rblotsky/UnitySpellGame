using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.SceneManagement;

#endif

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class DuplicatePersistentCatcher : MonoBehaviour
{
    // DATA //
    [SerializeField] public int cachedInstanceID = 0;


    // FUNCTIONS //
    // Basic Functions
#if UNITY_EDITOR
    void Awake()
    {
        // If in prefab editor, does nothing
        if (UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
        {
            cachedInstanceID = 0;
        }

        // If the instance ID cached in the object is not equal to Unity's generated ID,
        // and it isn't zero, that means the object must be a copy of an existing object.
        else if (cachedInstanceID != GetInstanceID())
        {
            // If zero, caches the instance ID (this means the object is newly created)
            if (cachedInstanceID == 0)
            {
                cachedInstanceID = GetInstanceID();
            }

            // Otherwise, caches the instance ID but also resets saveable components.
            else
            {
                cachedInstanceID = GetInstanceID();

                // Ensures the object is valid? not sure this is copy pasted and I don't get what this does
                if (cachedInstanceID < 0)
                {
                    // Resets the DataPersistentObject component attached to this object
                    DataPersistentObject persistentComponent = GetComponent<DataPersistentObject>();

                    if(persistentComponent != null)
                    {
                        persistentComponent.objectID = 0;
                    }
                }
            }
        }
    }
#endif
}
