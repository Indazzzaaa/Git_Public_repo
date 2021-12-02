using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// This will spawn the objects and according to features on the basis of the probability
/// </summary>
public class SpawnObjectSystem : MonoBehaviour
{
    #region variables

    [Header("Probability of spawn type objects(Do not touch)")]
    [Space(20)]
    [SerializeField] private float _rotationProb;

    [SerializeField] private float _scaleProb;
    [SerializeField] private float _moveProb;
    [SerializeField] private float _otherProb;

    //---G
    [Space(10)]
    [Header("-------Just for testing stuff---------------")]
    [SerializeField] private int _gameLevel;

    [SerializeField] private int _levelDistanceFactor;// we multiply with level to get the distance at each level
    [SerializeField] private float _gap;

    [Space(10)]
    [SerializeField] private Vector3 _lastFaceCordinate;

    [SerializeField] private Vector3 _endCordinate;

    [Space(10)]
    [SerializeField] private GameObject _parent;

    [SerializeField] private GameObject _startCube;

    #endregion variables

    [SerializeField] private bool _shouldPlaceTheEndTile;
    public static SpawnObjectSystem Instance;
    public float DistanceTillToBuild;
    public float GetEndCordinate { get => _endCordinate.z; }

    //---Y

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        _gameLevel = GameData.Instance.GetGameLevel;
        _lastFaceCordinate = Vector3.zero;// initially our last co-ordinate will be at zero
        _endCordinate.z = _gameLevel * _levelDistanceFactor;
        DistanceTillToBuild = 20;
        // during start every variable will have default zero probability
        _shouldPlaceTheEndTile = true; // allow placing the end tile once in every level
        AssignProbability();
        SpawnObjects();
    }

    [ContextMenu("See Probability")]
    // this will help us to assign probability to the different behaviors
    private void AssignProbability()
    {
        if (_gameLevel <= 10)
        {
            _moveProb = _gameLevel * 2.5f;
            _rotationProb = 0;
            _scaleProb = 0;
            _otherProb = 0;
        }
        else if (_gameLevel <= 20)
        {
            _moveProb = 25;
            _rotationProb = _gameLevel % 10 * (2.5f);
            _scaleProb = 0;
            _otherProb = 0;
        }
        else if (_gameLevel <= 30)
        {
            _moveProb = 25;
            _rotationProb = 25;
            _scaleProb = _gameLevel % 10 * (2.5f);
            _otherProb = 0;
        }
        else if (_gameLevel <= 40)
        {
            _moveProb = 25;
            _rotationProb = 25;
            _scaleProb = 25;
            _otherProb = _gameLevel % 10 * (2.5f);
        }
        else
        {
            _moveProb = 25;
            _rotationProb = 25;
            _scaleProb = 25;
            _otherProb = 25;
        }
    }

    [ContextMenu(" Spawn the objects")]
    // this will instantiate the object on the basis of the probability
    private void SpawnObjects()
    {
        //code for placing the start cube in the scene
        var instance = Instantiate(_startCube, Vector3.zero, Quaternion.identity, _parent.transform);
        instance.tag = Constants.consts.environmentTag;

        var meshRenderer = instance.GetComponent<MeshRenderer>();
        meshRenderer.material = GameManager.Instance.GetBallMeterial;
        _lastFaceCordinate = new Vector3(0, 0, meshRenderer.bounds.extents.z);

        //---R
        SpawnObjectHelper();
    }

    //-- this will help in spawning object when we call it.Since we are only building our level in z-direction so we do not need to accept the parameters in vector3 anymore.
    public void SpawnObjectHelper()
    {
        if (DistanceTillToBuild > _endCordinate.z)
        {
            DistanceTillToBuild = _endCordinate.z;
        }

        while (_lastFaceCordinate.z < DistanceTillToBuild)
        {
            ChooseTypeOfSpawnObject();
        }
        if (_lastFaceCordinate.z >= _endCordinate.z && _shouldPlaceTheEndTile)
        {
            PlaceTheEndTile();
            _shouldPlaceTheEndTile = false;
        }
    }

    public void ChangeEndDistance()
    {
        var distance = DistanceTillToBuild;
        distance += 20;
        if (distance > _endCordinate.z)
        {
            DistanceTillToBuild = _endCordinate.z;
        }
        else
        {
            DistanceTillToBuild = distance;
        }
    }

    // this will help to choose between different categories of objects
    private void ChooseTypeOfSpawnObject()
    {
        // we decided that at least 50% of the object will be static in our scene
        var dyanmicVsStaticProb = Random.Range(1, 100);

        if (dyanmicVsStaticProb < 50)
        {
            InstantiateGameProps(DefaultObjects.Instance);
        }
        else
        {
            var differentFeaturesRange = Random.Range(1, 100);
            if (differentFeaturesRange <= 25)
            {
                MakeOjbectEasy(MovableObjects.Instance, _moveProb);
            }
            else if (differentFeaturesRange >= 26 && differentFeaturesRange <= 50)
            {
                MakeOjbectEasy(RotationalObjects.Instance, _rotationProb);
            }
            else if (differentFeaturesRange >= 51 && differentFeaturesRange <= 75)
            {
                MakeOjbectEasy(MovableObjects.Instance, _scaleProb);
            }
            else
            {
                MakeOjbectEasy(RotationalObjects.Instance, _otherProb);
            }
        }
    }

    private void MakeOjbectEasy(IPutObject instance, float probType)
    {
        var shouldISpawn = Random.Range(0, 26);
        if (shouldISpawn <= probType)
        {
            InstantiateGameProps(instance);
        }
        else
        {
            InstantiateGameProps(DefaultObjects.Instance);
        }
    }

    private void InstantiateGameProps(IPutObject instance)
    {
        _lastFaceCordinate = instance.InstantiateObject(_lastFaceCordinate, _gap, _parent.transform);
    }

    private void PlaceTheEndTile()
    {
        var instance = Instantiate(_startCube, _lastFaceCordinate + Vector3.forward * _gap, Quaternion.identity, _parent.transform);
        var meshRenderer = instance.GetComponent<MeshRenderer>();
        meshRenderer.material = GameManager.Instance.GetBallMeterial;
        var extent = meshRenderer.bounds.extents.z;
        instance.transform.position = _lastFaceCordinate + Vector3.forward * (_gap + extent);
        instance.tag = Constants.consts.victoryObjectTag;
    }

    //---Y
    //++ TESTING ZONE
    [ContextMenu("Spawn Rotational Cube")]
    private void SpawnRCube()
    {
        RotationalObjects.Instance.InstantiateObject(Vector3.zero, .1f, _parent.transform);
    }

    [ContextMenu("Spawn Scalable Cube")]
    private void SpawnSCube()
    {
        ScalableObjects.Instance.InstantiateObject(Vector3.zero, .1f, _parent.transform);
    }

    [ContextMenu("Spawn Movable Cube")]
    private void SpawnMCube()
    {
        MovableObjects.Instance.InstantiateObject(Vector3.zero, .1f, _parent.transform);
    }
}