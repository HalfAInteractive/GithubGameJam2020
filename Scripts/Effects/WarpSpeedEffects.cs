using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpSpeedEffects : MonoBehaviour
{
    GameManager gameManager = null;
    AudioSource audioSource = null;
    List<ParticleSystem> warpParticleSystems = null;

    private void Awake()
    {
        warpParticleSystems = new List<ParticleSystem>();

        int numOfObjs = transform.childCount;
        for(int i = 0; i < numOfObjs; i++)
        {
            transform.GetChild(i).TryGetComponent(out ParticleSystem particles);
            if(particles != null)
            {
                warpParticleSystems.Add(particles);
            }
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        gameManager = GetComponentInParent<GameManager>();

        gameManager.OnWarpSceneBegin += PlayWarpParticles;
        gameManager.OnWarpSceneFinished += StopWarpParticles;
    }

    private void OnDestroy()
    {
        gameManager.OnWarpSceneBegin -= PlayWarpParticles;
        gameManager.OnWarpSceneFinished -= StopWarpParticles;
    }

    void PlayWarpParticles(Level level)
    {
        if (level == Level.Tutorial) return;

        audioSource.Play();
        foreach (var particles in warpParticleSystems)
        {
            particles.Play();
        }
    }

    void StopWarpParticles(Level level)
    {
        if (level == Level.Tutorial) return;

        foreach (var particles in warpParticleSystems)
        {
            particles.Stop();
        }
    }
}
