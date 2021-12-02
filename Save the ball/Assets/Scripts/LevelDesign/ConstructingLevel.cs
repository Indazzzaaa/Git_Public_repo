using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LevelDesign
{
    /// <summary>
    /// In this we are constructing the level of the game, by first randomly assign the scale of the object
    /// and then scaling it, and then calculating it's correct position and then placing it. it won't work when we
    /// have object's with the different scales or distorting on the changing scales , go for goto:#ConstructionLevel2 scripts.
    /// And only works best with the unity primitive shapes.
    /// </summary>
    public class ConstructingLevel : MonoBehaviour

    {
        #region variables

        [Space(10)]
        [SerializeField] private GameObject _constructionObject;

        [SerializeField] private float _gapBetweenObjects;

        [Space(10)]
        [SerializeField] private Vector3 _endPos;

        [SerializeField] private float _maxZExtent; // this is for testing how much distance will be enough to place

        //---R
        [SerializeField] private List<GameObject> _spawnedObjects;

        #endregion variables

        private void Start()
        {
            FillLevel();
        }

        [ContextMenu("Fill Level")]
        private void FillLevel()
        {
            #region Start block

            // this is position of the start object
            var posAtSpawning = Vector3.zero;
            var myZ_size = Random.Range(1.0f, _maxZExtent);//-- size of object in z_direction , using scale for extending it.
            // setting up the location and size of the object
            var instance = Instantiate(_constructionObject, posAtSpawning, Quaternion.identity) as GameObject;
            instance.gameObject.tag = Constants.consts.environmentTag;
            var instanceTransform = instance.transform;
            instanceTransform.localScale = new Vector3(instanceTransform.localScale.x, instanceTransform.localScale.y, myZ_size);

            var lastObjectLastCornerZ_pos = posAtSpawning.z + myZ_size / 2;//-- this is the last face position,use for placing the next object
            _spawnedObjects.Add(instance);//-- putting the spawned object in the list

            #endregion Start block

            //---Y

            #region Full Scene

            while (posAtSpawning.z < _endPos.z)
            {
                myZ_size = Random.Range(1, _maxZExtent);
                posAtSpawning = new Vector3(0, 0, lastObjectLastCornerZ_pos + _gapBetweenObjects + myZ_size / 2);
                lastObjectLastCornerZ_pos = posAtSpawning.z + myZ_size / 2;

                // setting up the location and size of the object
                instance = Instantiate(_constructionObject, posAtSpawning, Quaternion.identity) as GameObject;
                instance.gameObject.tag = Constants.consts.environmentTag;
                instanceTransform = instance.transform;
                // #hardcoded
                instanceTransform.localScale = new Vector3(1, 1, myZ_size);

                _spawnedObjects.Add(instance);
            }

            #endregion Full Scene
        }
    }
}