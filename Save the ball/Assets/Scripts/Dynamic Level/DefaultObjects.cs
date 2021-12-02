using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultObjects : MonoBehaviour, IPutObject
{
    [SerializeField] private GameObject _simpleCube;
    public static DefaultObjects Instance;

    // Start is called before the first frame update
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
        var refPoint = Constants.consts.refPoint;
        var instance = Instantiate(_simpleCube, refPoint, Quaternion.identity, parentTransform) as GameObject;
        instance.tag = Constants.consts.environmentTag;
        var meshRenderer = instance.GetComponent<MeshRenderer>();
        var extent = meshRenderer.bounds.extents.z; //-- extreme z coordinate, since we are filling object in the z-direction
        var position = MyCordinate(extent, lastFaceCordinate, gapInObjects);
        instance.transform.position = position;
        var MylastFaceCoordinate = position + Vector3.forward * extent;

        return MylastFaceCoordinate;
    }

    private Vector3 MyCordinate(float extent, Vector3 lastCordinate, float gapBetweenObjects)
    {
        var myPosition = lastCordinate + new Vector3(0, Random.Range(-2, 2), gapBetweenObjects + extent);
        myPosition.y = Mathf.Clamp(myPosition.y, -15, 15);
        return myPosition;
    }
}