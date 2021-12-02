using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscellaneousObjects : MonoBehaviour, IPutObject
{
    public static MiscellaneousObjects Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public Vector3 InstantiateObject(Vector3 lastFaceCordinate, float gapInObjects, Transform parentTransform)
    {
        print("Miscellaneous object placed");
        return lastFaceCordinate;
    }
}