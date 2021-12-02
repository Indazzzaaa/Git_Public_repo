using System;
using UnityEngine;
using Constants;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;

namespace Scripts.Mechanics
{
    /// <summary>
    /// Script designed to move the object on which it attached on tapping the screen , specific unit further
    /// </summary>
    ///
    [RequireComponent(typeof(Rigidbody))]
    public class MoveForward : MonoBehaviour
    {
        [SerializeField] private float distanceToMoveForward;
        [SerializeField] private bool _canMoveForward;

        [Space(10)]
        [SerializeField] private ParticleSystem _collidingParticle;

        private Touch _screenTouch;
        private Rigidbody _myBody;

        //---R
        private void Start()
        {
            _myBody = GetComponent<Rigidbody>();
            _canMoveForward = true;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && _canMoveForward)
            {
                Movement2();
            }
            else if (Input.touchCount > 0 && _canMoveForward)
            {
                _screenTouch = Input.GetTouch(0);
                if (_screenTouch.phase == TouchPhase.Ended)
                {
                    Movement2();
                }
            }
        }

        // this by applying the force
        private void Movement2()
        {
            _canMoveForward = false; //- we do not want to move once we moved, without touching the ground
            _myBody.AddForce(Vector3.forward * distanceToMoveForward, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(consts.environmentTag))
            {
                //play the colliding particle
                PlayCollidedParticle();
                var meshRenderer = collision.gameObject.GetComponent<MeshRenderer>();
                meshRenderer.material = GameManager.Instance.GetBallMeterial;
                _canMoveForward = true;
                //_mySource.PlayOneShot(AudioManager.Instance.GetBallTapClip);
                var audioManager = AudioManager.Instance;
                audioManager.PlaySound(audioManager.GetBallTapClip);
                // we want to know that play is not colliding with the side faces and it's collision registered
                var face = collision.GetContact(0);
                if (face.normal.z < -.5f || face.normal.z > .5f)
                {
                    return; // we are avoiding the left or right faces
                }
                else
                {
                    if (collision.gameObject.GetComponent<MoveInAnyDirection>() != null)
                    {
                        // do not consider the last point if it is movable
                        return;
                    }
                    else
                    {
                        GameManager.Instance.PlayerLastCollidingCordinate = _myBody.position; //-- updating the last colliding coordinate
                    }
                }
            }
            else if (collision.gameObject.CompareTag(consts.victoryObjectTag))
            {
                
                var face = collision.GetContact(0);
                if (face.normal.z < -.5f || face.normal.z > .5f)
                {
                    return;
                }
                else
                {
                    GameManager.Instance.PlayerLastCollidingCordinate = _myBody.position;
                    AfterWinning();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(consts.destoryingTag)) return;
            AfterDyingObjectDo();
            gameObject.SetActive(false); // deactivating the player
        }

        private async void ResetTheLevel()
        {
            await Task.Delay(100);
            GameData.Instance.IncrementLevel();
            GameData.Instance.SaveData();
            SceneManager.LoadScene(1);
        }

        private async void AfterDyingObjectDo()
        {
            GameManager.Instance.IsPlayerDied = true;
            GameData.Instance.IncrementAttempts();
            GameData.Instance.SaveData();
            GameManager.Instance.PlayDestroyingParticles(transform.position);
            var audioManager = AudioManager.Instance;
            audioManager.PlaySound(audioManager.GetDestroyingSound);
            await Task.Delay(1200);
            GameManager.Instance.ShowMeResumePanel();
        }

        [ContextMenu("Play the particle")]
        private void PlayCollidedParticle()
        {
            _collidingParticle.Play();
        }

        private async void AfterWinning()
        {
            GameManager.Instance.PlayTheVictoryShow();
            await Task.Delay(5000);
            ResetTheLevel();
        }
    }
}