using Scripts.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjects : MonoBehaviour, IPutObject
{
    [SerializeField] private int _gameLevel; // get this data from the LevelManager
    [SerializeField] private GameObject _simpleCube;
    [SerializeField] public static MovableObjects Instance;
    [SerializeField] private float _myMovingSpeed;

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

        _gameLevel = GameData.Instance.GetGameLevel;
    }

    public Vector3 InstantiateObject(Vector3 lastFaceCordinate, float gapInObjects, Transform parentTransforms)
    {
        var refPoint = Constants.consts.refPoint;
        var instance = Instantiate(_simpleCube, refPoint, Quaternion.identity, parentTransforms) as GameObject;
        instance.tag = Constants.consts.environmentTag;
        var meshRenderer = instance.GetComponent<MeshRenderer>();
        var extent =
            meshRenderer.bounds.extents.z; //-- extreme z coordinate, since we are filling object in the z-direction
        var maxDistance_in_eitherDireciton = Random.Range(1, 5); //now we have to give it some room to travel too.
        var axisToApplyOn = (AxisOnApplied) Random.Range(1, 3); // apply on y and z only
        var position = MyCordinate(extent, lastFaceCordinate, gapInObjects, maxDistance_in_eitherDireciton,
            axisToApplyOn);
        instance.transform.position = position;
        var MylastFaceCoordinate = position + Vector3.forward *
            (extent + (axisToApplyOn == AxisOnApplied.Z_AXIS ? maxDistance_in_eitherDireciton : gapInObjects));
        var scriptRef =
            instance.AddComponent<MoveInAnyDirection>() as MoveInAnyDirection; // adding the rotating component

        //---G

        scriptRef.SetMyValues(axisToApplyOn, _gameLevel * _myMovingSpeed, maxDistance_in_eitherDireciton);

        return MylastFaceCoordinate;
    }

    private Vector3 MyCordinate(float extent, Vector3 lastCordinate, float gapBetweenObjects, float distanceToTravel,
        AxisOnApplied appliedAxis)
    {
        Vector3 myPosition = default;
        if (appliedAxis == AxisOnApplied.Z_AXIS)
        {
            myPosition = lastCordinate + new Vector3(0, 0, extent + distanceToTravel);
        }
        else
        {
            myPosition = lastCordinate + Vector3.forward * (extent + gapBetweenObjects);
        }

        return myPosition;
    }
}