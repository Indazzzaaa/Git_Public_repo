using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDestroyerMovement : MonoBehaviour
{
    #region variables

    [Space(20)]
    [SerializeField] private GameObject _upDestroyer;

    [SerializeField] private GameObject _downDestoryer;

    [SerializeField] private float _distanceFromCenter;

    [Space(10)]
    [SerializeField] private Animator _animationController; // this is for playing animation after ball destroyed

    private Transform _playerTransforms;
    private Transform _myTransfoms;

    #endregion variables

    private void Start()
    {
        _upDestroyer.transform.position = Vector3.up * _distanceFromCenter;
        _downDestoryer.transform.position = Vector3.down * _distanceFromCenter;

        _playerTransforms = GameObject.FindGameObjectWithTag(Constants.consts.playerTag).GetComponent<Transform>();
        _myTransfoms = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Time.frameCount % 5 == 0)
        {
            //- once player destroyed it will throw the missingReferenceException and we do not need this component too
            try
            {
                var zNeededOnly = _myTransfoms.position;
                zNeededOnly.z = _playerTransforms.position.z;
                _myTransfoms.position = zNeededOnly;
            }
            catch (MissingReferenceException)
            {
                _animationController.gameObject.SetActive(true);
                _animationController.SetBool("play", true);

                gameObject.SetActive(false);
            }
        }
    }
}