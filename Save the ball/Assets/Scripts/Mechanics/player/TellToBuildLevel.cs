using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It's job to tell the dynamic level builder to build the level when it approaches to end
/// </summary>
public class TellToBuildLevel : MonoBehaviour
{
    [SerializeField] private int _distanceCheck; // distance near to end after rebuilding the next distance platforms
    private Transform _myTransforms;

    // first build the start some distance
    // when we are few meters behind tell the level builder to build the level to an updated end point

    private void Start()
    {
        _myTransforms = GetComponent<Transform>();
    }

    private void Update()
    {
        if ((_myTransforms.position.z + _distanceCheck) > SpawnObjectSystem.Instance.DistanceTillToBuild)
        {
            SpawnObjectSystem.Instance.ChangeEndDistance();
            SpawnObjectSystem.Instance.SpawnObjectHelper();
        }
    }
}