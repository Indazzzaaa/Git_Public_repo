using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmitTest : MonoBehaviour
{
    private ParticleSystem _myParticles;

    private void Start()
    {
        _myParticles = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            print("Space is pressed");
            PlayParticle();
        }
    }

    [ContextMenu("See particle in action")]
    private void PlayParticle()
    {
        print("Space is pressed");
        _myParticles.Play();
    }
}