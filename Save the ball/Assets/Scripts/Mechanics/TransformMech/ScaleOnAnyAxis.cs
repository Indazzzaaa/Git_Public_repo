using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Mechanics
{
    /// <summary>
    /// This script will help to shrink  or scale the object in any direction
    /// </summary>
    public class ScaleOnAnyAxis : MonoBehaviour

    {
        [Space(15)]
        [SerializeField] private float _scalingSpeed;

        [SerializeField] private float _minShrinkScale;
        [SerializeField] private float _maxExpandScale;

        [Space(10)]
        [Header("------ Only For Visualizing -----------")]
        [SerializeField] private bool _canExpand;

        [SerializeField] private AxisOnApplied _appliedAxis;

        private Transform _myTransforms;

        //---Y
        private void Start()
        {
            _myTransforms = GetComponent<Transform>();
            _canExpand = false;
        }

        private void Update()
        {
            ChangeMyScale();
        }

        public void SetMyValues(AxisOnApplied appliedAxis, float scalingSpeed, int maxScale)
        {
            _appliedAxis = appliedAxis;
            _scalingSpeed = scalingSpeed;
            _maxExpandScale = maxScale;
        }

        private void ChangeMyScale()
        {
            var scale = _myTransforms.localScale;
            switch (_appliedAxis)
            {
                case AxisOnApplied.X_AXIS:
                    ApplingScale(Vector3.right, scale.x);
                    break;

                case AxisOnApplied.Y_AXIS:
                    ApplingScale(Vector3.up, scale.y);
                    break;

                case AxisOnApplied.Z_AXIS:
                    ApplingScale(Vector3.forward, scale.z);
                    break;

                default:
                    break;
            }
        }

        private void ApplingScale(Vector3 direction, float scale)
        {
            if (!_canExpand)
            {
                _myTransforms.localScale -= direction * _scalingSpeed * Time.deltaTime;

                if (scale < _minShrinkScale)
                {
                    _canExpand = true;
                }
            }
            else
            {
                _myTransforms.localScale += direction * _scalingSpeed * Time.deltaTime;
                if (scale > _maxExpandScale)
                {
                    _canExpand = false;
                }
            }
        }
    }
}