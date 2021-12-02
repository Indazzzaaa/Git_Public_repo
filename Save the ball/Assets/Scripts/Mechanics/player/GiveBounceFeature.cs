using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

/// <summary>
/// To Give the object bouncy behavior , from Rigidbody, and that works well
/// </summary>
namespace Scripts.Mechanics
{
    [RequireComponent(typeof(Rigidbody))]
    public class GiveBounceFeature : MonoBehaviour
    {
        [SerializeField] private float _upMaxForce;

        private Rigidbody _myBody;

        //---R
        private void Start()
        {
            _myBody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(consts.environmentTag))
            {
                _myBody.AddForce(Vector3.up * _upMaxForce, ForceMode.Impulse);
            }
            // #SeeThisIfRequired
        }
    }
}