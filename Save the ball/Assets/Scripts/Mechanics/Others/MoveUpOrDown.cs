using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Mechanics
{
    /// <summary>
    /// To move the object up or down with the specific speed
    /// </summary>
    public class MoveUpOrDown : MonoBehaviour
    {
        private float _myMovementSpeed;
        private Transform _myTransfrom;

        public float MyMovementSpeed { set => _myMovementSpeed = value; }

        private void Start()
        {
            _myTransfrom = GetComponent<Transform>();
        }

        private void Update()
        {
            MoveMe();
            DoSelfDisable();
        }

        private void MoveMe()
        {
            var lastFrameRenderTime = Time.deltaTime;
            _myTransfrom.position += Vector3.up * _myMovementSpeed * lastFrameRenderTime;
        }

        private void DoSelfDisable()
        {
            if (_myTransfrom.position.y < ObjectSpawningSystem.Instance.EndPosition.y) return;
            _myTransfrom.position = ObjectSpawningSystem.Instance.StartPosition;
        }
    }
}