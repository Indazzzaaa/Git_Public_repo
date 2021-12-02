using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region variables

    [SerializeField] private AudioClip[] _gameMusicClips;
    [SerializeField] private AudioClip _ballDestroyingClip;
    [SerializeField] private AudioClip _ballTapClip;
    [SerializeField] private AudioClip _victoryClip;

    [Space(20)]
    [SerializeField] private float _runnigClipSound;

    private float _timer;
    private AudioSource _audioSource;

    public static AudioManager Instance;

    //---R
    public AudioClip GetDestroyingSound { get => _ballDestroyingClip; }

    public AudioClip GetVicotrySound { get => _victoryClip; }
    public AudioClip GetBallTapClip { get => _ballTapClip; }

    #endregion variables

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

        _audioSource = GetComponent<AudioSource>();
        _runnigClipSound = 0;
        _timer = 1; // just the any number so that our music get starts atleast.
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _runnigClipSound)
        {
            var clip = RandomClip();
            _runnigClipSound = clip.length;
            _audioSource.PlayOneShot(RandomClip());
            _timer = 0;
        }
    }

    private AudioClip RandomClip()
    {
        return _gameMusicClips[Random.Range(0, _gameMusicClips.Length)];
    }

    public void PlaySound(AudioClip ClipToPlay)
    {
        _audioSource.PlayOneShot(ClipToPlay);
    }
}