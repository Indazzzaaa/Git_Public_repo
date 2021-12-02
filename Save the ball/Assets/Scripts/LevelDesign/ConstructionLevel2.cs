using System.Threading.Tasks;
using UnityEngine;

namespace LevelDesign
{
    /// <summary>
    /// This script will design the level on the basis of placing object at the reference point
    /// and then calculating the actual position of the object and then placing back to that position.
    /// Now we no need to scale object according to we want , any size ,scale object works as along as it has
    /// MeshRendere attached to it.
    /// </summary>

    public class ConstructionLevel2 : MonoBehaviour
    {
        // tag:#ConstructionLevel2
        [Space(20)]
        [SerializeField] private GameObject _levelBuildingObject;

        [Space(20)]
        [Header("nothing works without start:i.e. 0,0,0  and End : have to decide")]
        [SerializeField] private float _end;

        [SerializeField] private float _gapBetweenObjects;

        [Space(15)]
        [Header("-----Just for watching------------")]
        [SerializeField] private Vector3 _lastFaceCordinate;

        //---Y

        //---R

        private void Start()
        {
            BuildTheMeshCity();//-- since this is async and ti will wait for 100ms ,this becomes non continuous
        }

        private async void BuildTheMeshCity()
        {
            await Task.Delay(100);
            var instance = Instantiate(_levelBuildingObject, Constants.consts.refPoint, Quaternion.identity);
            instance.tag = Constants.consts.environmentTag;
            var myMeshRenderer = instance.GetComponent<MeshRenderer>();
            var myTranforms = instance.transform;
            myTranforms.position = Vector3.zero; // which will be Vector3.zero for the start object
            _lastFaceCordinate = new Vector3(0, 0, myMeshRenderer.bounds.extents.z); // this is the last face coordinate

            while (_lastFaceCordinate.z < _end)
            {
                await Task.Delay(100); // 100ms delay
                instance = Instantiate(_levelBuildingObject, Constants.consts.refPoint, Quaternion.identity);
                instance.tag = Constants.consts.environmentTag;
                myMeshRenderer = instance.GetComponent<MeshRenderer>();
                myTranforms = instance.transform;
                myTranforms.position = GiveMeNextCordinate(myMeshRenderer.bounds.extents.z);
            }
            //#print
            print("City has been build");
        }

        private Vector3 GiveMeNextCordinate(float z)
        {
            var nextCordinate = _lastFaceCordinate + new Vector3(0, 0, _gapBetweenObjects + z);
            _lastFaceCordinate = nextCordinate + new Vector3(0, 0, z);
            return nextCordinate;
        }
    }
}