using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCollector : MonoBehaviour
{
    [SerializeField] int ringCountGoal = 30;
    [SerializeField] ParticleSystem starParticles;
    [SerializeField] ParticleSystem ringLostExplosionParticles;
    [SerializeField] GameObject ring;
    [SerializeField] AudioClip ringActivated = null;

    PlayerController PC = null;
    ParticleSystem.MainModule starParticlesMain;
    ParticleSystem.MainModule ringLostExplosionParticlesMain;

    public int StarCount { get; private set; } = 0;
    public bool HasRing { get; private set; } = false;

    private void Awake()
    {
        ring.SetActive(false);

        starParticlesMain = starParticles.main;
        starParticlesMain.maxParticles = 0;

        ringLostExplosionParticlesMain = ringLostExplosionParticles.main;
        ringLostExplosionParticlesMain.maxParticles = 0;
    }

    private void Start()
    {
        PC = GetComponent<PlayerController>();

        PC.OnPlayerStarCollect += Collect;
        PC.OnPlayerDamaged += Lost;
        PC.OnPlayerKilled += Died;
    }

    private void OnDestroy()
    {
        PC.OnPlayerStarCollect -= Collect;
        PC.OnPlayerDamaged -= Lost;
        PC.OnPlayerKilled -= Died;
    }

    void Collect(int amt)
    {
        StarCount += amt;
        if (HasRing) return; // we can keep counting but we don't want it to run the other stuff below
        
        starParticlesMain.maxParticles = StarCount;

        if(StarCount >= ringCountGoal)
        {
            ring.SetActive(true);
            starParticles.Stop();
            starParticles.gameObject.SetActive(false);
            GetComponent<AudioSource>().PlayOneShot(ringActivated);
            HasRing = true;
        }
    }

    void Lost(int currHealth)
    {
        HasRing = false;

        starParticlesMain.maxParticles = 0;
        starParticles.Stop();
        starParticles.gameObject.SetActive(false);
        starParticles.gameObject.SetActive(true);


        ring.SetActive(false);

        ringLostExplosionParticlesMain.maxParticles = StarCount;
        ringLostExplosionParticles.Play();


        StarCount = 0;
    }

    void Died()
    {
        Lost(0);
    }
}
