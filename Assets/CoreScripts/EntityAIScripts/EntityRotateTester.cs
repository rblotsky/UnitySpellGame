using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRotateTester : MonoBehaviour
{
    public float rotationAmount1 = 20;
    public Transform watched;
    public EntityRotate rotator;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RotateTo());
    }


    IEnumerator RotateTo()
    {
        while (enabled)
        {
            rotator.StartNewLookAtPointRotation(watched.position);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
