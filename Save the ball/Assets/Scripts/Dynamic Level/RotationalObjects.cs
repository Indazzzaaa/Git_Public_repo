using Scripts.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationalObjects : MonoBehaviour, IPutObject
{
    [SerializeField] private int _gameLevel; // get this data from the LevelManager
    [SerializeField] private GameObject _simpleCube;
    [SerializeField] public static RotationalObjects Instance;
    [SerializeField] private float _roationalSpeed;

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

    public Vector3 InstantiateObject(Vector3 lastFaceCordinate, float gapInObjects, Transform parentTransform)
    {
        var refPoint = Constants.consts.refPoint;
        var instance = Instantiate(_simpleCube, refPoint, Quaternion.identity, parentTransform) as GameObject;
        instance.tag = Constants.consts.environmentTag;
        var meshRenderer = instance.GetComponent<MeshRenderer>();
        var extent = meshRenderer.bounds.extents.z; //-- extreme z coordinate, since we are filling object in the z-direction
        var position = MyCordinate(extent, lastFaceCordinate, gapInObjects);
        instance.transform.position = position;
        var MylastFaceCoordinate = position + Vector3.forward * extent;
        var scriptRef = instance.AddComponent<RotateObject>() as RotateObject; // adding the rotating component

        if (_gameLevel > 15)
        {
            var axisToApplyOn = (AxisOnApplied)Random.Range(0, 3);
            scriptRef.SetMyValues(axisToApplyOn, _gameLevel * _roationalSpeed);
        }
        else
        {
            scriptRef.SetMyValues(AxisOnApplied.Y_AXIS, _gameLevel * 2);
        }
        return MylastFaceCoordinate;
    }

    private Vector3 MyCordinate(float extent, Vector3 lastCordinate, float gapBetweenObjects)
    {
        var myPosition = lastCordinate + new Vector3(0, 0, gapBetweenObjects + extent);
        return myPosition;
    }
}