using System.Collections;
using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class AmmoTarget : MonoBehaviour, IAutoTarget
{
    [SerializeField] AudioClip popup;
    [SerializeField] ParticleSystem targetParticles;
    [SerializeField] ParticleSystem goalParticles;
    [SerializeField] ParticleSystem hitVerifyParticle;

    public event Action OnAmmoEntered = delegate { };

    public bool HasAmmoPassedThrough { get; private set; } = false;

    private void Awake()
    {
        targetParticles.transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        HasAmmoPassedThrough = false;
        GetComponent<AudioSource>().PlayOneShot(popup);
        targetParticles.Play();
        targetParticles.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutBounce);
    }

    private void OnDisable()
    {
        ResetTarget();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IAmmo ammo))
        {
            if(ammo.IsFired())
            {
                OnAmmoEntered();

                GetComponent<AudioSource>().Play();
                GetComponent<Collider>().enabled = false;
                targetParticles.Stop();

                hitVerifyParticle.Play();
                goalParticles.Play();

                HasAmmoPassedThrough = true;
                StartCoroutine(DisableDelayCoroutine());
            }
        }
    }

    IEnumerator DisableDelayCoroutine()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    private void ResetTarget()
    {
        targetParticles.transform.localScale = Vector3.zero;
        GetComponent<Collider>().enabled = true;
    }
}
