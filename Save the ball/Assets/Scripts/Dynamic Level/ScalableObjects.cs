using Scripts.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableObjects : MonoBehaviour, IPutObject
{
    [SerializeField] private int _gameLevel; // get this data from the LevelManager
    [SerializeField] private GameObject _simpleCube;
    [SerializeField] public static ScalableObjects Instance;

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

        var scriptRef = instance.AddComponent<ScaleOnAnyAxis>() as ScaleOnAnyAxis; // adding the scaling component
        var randValue = Random.Range(1, 4); // scale to this value .

        var position = MyCordinate(extent, lastFaceCordinate, gapInObjects, randValue);
        instance.transform.position = position;
        var MylastFaceCoordinate = position + Vector3.forward * randValue / 2;
        if (_gameLevel > 25)
        {
            var axisToApplyOn = (AxisOnApplied)randValue;
            scriptRef.SetMyValues(axisToApplyOn, _gameLevel * .1f, randValue);
        }
        else
        {
            scriptRef.SetMyValues(AxisOnApplied.Y_AXIS, _gameLevel * .1f, randValue);
        }
        return MylastFaceCoordinate;
    }

    private Vector3 MyCordinate(float extent, Vector3 lastCordinate, float gapBetweenObjects, float myScale)
    {
        var myPosition = lastCordinate + new Vector3(0, 0, gapBetweenObjects + myScale / 2);
        return myPosition;
    }
}