using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts.Mechanics
{
    /// <summary>
    /// This system is designed for spawning the object and move either up or down direction
    /// </summary>
    public class ObjectSpawningSystem : MonoBehaviour
    {
        [Header("----------------Set Manually In Inspector--------------------")]
        [Space(20)]
        [SerializeField] private int offset;

        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _endPosition;
        [SerializeField] private int _lengthOfList;

        [Space(20)]
        [SerializeField] private GameObject _objectToSpawn;

        [Range(0, 2)]
        [SerializeField] private float _obstacleMovementSpeed;

        //---Y
        [Header("Do not touch these")]
        [SerializeField] private float _timeToWait; // waiting time , when next object will be activated ;

        [SerializeField] private List<GameObject> _listOfObjects;

        public static ObjectSpawningSystem Instance;

        //---R
        public Vector3 StartPosition { get => _startPosition; }

        public Vector3 EndPosition { get => _endPosition; }

        private void Start()
        {
            // #hardcoded

            // #Note End position must be higher then the start position then only obstacles will move.
            _startPosition = transform.position - new Vector3(0, offset, 0);
            _endPosition = _startPosition + new Vector3(0, 2 * offset, 0);
            // singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this) //-if instance is not this , then there is system present in scene so no need of it
            {
                Destroy(gameObject);
            }

            //---Y
            // filling the list with spawn-able object and setting them deactivated
            for (int i = 0; i < _lengthOfList; i++)
            {
                var spawnableObjects = Instantiate(_objectToSpawn, _startPosition, Quaternion.identity);
                _listOfObjects.Add(spawnableObjects);
                var scriptRef = spawnableObjects.GetComponent<MoveUpOrDown>();
                scriptRef.MyMovementSpeed = _obstacleMovementSpeed; // feed them with the speed they have to move
                spawnableObjects.SetActive(false);
            }

            StartCoroutine(ActivateTheObstacles());
        }

        private IEnumerator ActivateTheObstacles()
        {
            var distanceBetweenStartAndEnd = Vector3.Distance(_startPosition, _endPosition);
            _timeToWait = distanceBetweenStartAndEnd / _listOfObjects.Count;
            // filling the list with spawn-able object and setting them deactivated
            foreach (var obj in _listOfObjects)
            {
                obj.SetActive(true);
                yield return new WaitForSeconds(_timeToWait);
            }
        }
    }
}