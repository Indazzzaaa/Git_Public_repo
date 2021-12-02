using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Mechanics
{
    /// <summary>
    /// Class designed to move in any direction, but direction have to be chosen while instantiating object, other wise
    /// there will be obscure movements noticeable
    /// </summary>
    public class MoveInAnyDirection : MonoBehaviour
    {
        #region variables

        [Space(20)]
        [SerializeField] private AxisOnApplied _appliedAxis;

        [SerializeField] private float _speed;
        [SerializeField] private float _distanceToFromCenter; //-- Distance we want our object to move in either direction positive to negative side
        //---R

        private Transform _myTransform;

        // #checkPlease

        private bool _canMoveForward; // to do the back and forth simulation in any direction

        // #checkPlease  check that if it is properly given while laying out the level
        private float back, front; // these are the extreme positions of the object in it's travailing direction

        #endregion variables

        //---Y
        private void Start()
        {
            _myTransform = GetComponent<Transform>();
            _canMoveForward = true;
            SetStartAndEndPositions();
        }

        private void Update()
        {
            MoveMe();
        }

        // This will be required , since we will be randomly placing the whole level so, first instancing at reference point then we are keeping it at desired position;
        public void SetStartAndEndPositions()
        {
            var pos = _myTransform.position;
            switch (_appliedAxis)
            {
                case AxisOnApplied.X_AXIS:
                    back = pos.x - _distanceToFromCenter;
                    front = pos.x + _distanceToFromCenter;
                    break;

                case AxisOnApplied.Y_AXIS:
                    back = pos.y - _distanceToFromCenter;
                    front = pos.y + _distanceToFromCenter;
                    break;

                case AxisOnApplied.Z_AXIS:
                    back = pos.z - _distanceToFromCenter;
                    front = pos.z + _distanceToFromCenter;
                    break;

                default:
                    break;
            }
        }

        private void MoveMe()
        {
            var pos = _myTransform.position;
            switch (_appliedAxis)
            {
                case AxisOnApplied.X_AXIS:
                    ApplingMovement(direction: Vector3.right, pos.x);
                    break;

                case AxisOnApplied.Y_AXIS:
                    ApplingMovement(direction: Vector3.up, pos.y);
                    break;

                case AxisOnApplied.Z_AXIS:
                    ApplingMovement(direction: Vector3.forward, pos.z);
                    break;

                default:
                    break;
            }
        }

        private void ApplingMovement(Vector3 direction, float compareDistance)
        {
            if (_canMoveForward)
            {
                _myTransform.position += direction * _speed * Time.deltaTime;
                if (compareDistance > front)
                {
                    _canMoveForward = false;
                }
            }
            else
            {
                _myTransform.position -= direction * _speed * Time.deltaTime;
                if (compareDistance < back)
                {
                    _canMoveForward = true;
                }
            }
        }

        public void SetMyValues(AxisOnApplied appliedAxis, float speed, float distanceFromCenter)
        {
            _appliedAxis = appliedAxis;
            _speed = speed;
            _distanceToFromCenter = distanceFromCenter;
        }
    }
}