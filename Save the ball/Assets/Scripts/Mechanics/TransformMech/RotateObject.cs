using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Mechanics
{
    /// <summary>
    /// Rotating object around it's center
    /// </summary>
    public enum AxisOnApplied : byte
    {
        X_AXIS, Y_AXIS, Z_AXIS
    }

    public class RotateObject : MonoBehaviour
    {
        [SerializeField] private float _rotatingSpeed;
        [SerializeField] private AxisOnApplied _rotationAxis;
        private Collider _myCollider;//-- just to get the center of the object

        private void Start()
        {
            _myCollider = GetComponent<Collider>();
        }

        private void Update()
        {
            RotateMe();
        }

        // this for setting the values from the outside
        public void SetMyValues(AxisOnApplied rotationAxis, float roationSpeed)
        {
            _rotationAxis = rotationAxis;
            _rotatingSpeed = roationSpeed;
        }

        private void RotateMe()
        {
            switch (_rotationAxis)
            {
                case AxisOnApplied.X_AXIS:
                    transform.RotateAround(_myCollider.bounds.center, Vector3.right, _rotatingSpeed * Time.deltaTime);
                    break;

                case AxisOnApplied.Y_AXIS:
                    transform.RotateAround(_myCollider.bounds.center, Vector3.up, _rotatingSpeed * Time.deltaTime);
                    break;

                case AxisOnApplied.Z_AXIS:
                    transform.RotateAround(_myCollider.bounds.center, Vector3.forward, _rotatingSpeed * Time.deltaTime);
                    break;

                default:
                    Debug.LogError("Please select the axis");
                    break;
            }
        }
    }
}