using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MasterExploder : MonoBehaviour
{
    [SerializeField] 
    [Range(0,60f)]float timeToDestroyItself = 5f;
    [SerializeField] AudioClip explosionSound = null;

    [Header("particle setup")]
    [SerializeField] ParticleSystem sphereParticle = null;
    [SerializeField] ParticleSystem explosionParticles = null;
    [SerializeField] ParticleSystem explosionStayParticles = null;
    [SerializeField] ParticleSystem sonicBoom = null;

    AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayExplosion();
    }


    void PlayExplosion()
    {
        audioSource.Play();

        sphereParticle.Play();
        explosionParticles.Play();
        explosionStayParticles.Play();
        sonicBoom.Play();

        StartCoroutine(DelayKillRoutine());
    }

    IEnumerator DelayKillRoutine()
    {
        yield return new WaitForSeconds(timeToDestroyItself);
        Destroy(gameObject);
    }
}
