using UnityEngine;

public interface IPutObject
{
    public Vector3 InstantiateObject(Vector3 lastFaceCordinate
        , float gapInObjects, Transform parentTransforms);
}