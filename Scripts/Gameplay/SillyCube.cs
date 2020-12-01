using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SillyCube : MonoBehaviour, IFocusable
{
    [SerializeField] ItemDrop drop = null;
    [SerializeField] AudioClip popup = null;
    [SerializeField] AudioClip inFocused;
    [SerializeField] AudioClip outFocused;

    [Header("Particle things")]
    [SerializeField] ParticleSystem explosionParticles = null;
    [SerializeField] ParticleSystem orbitingParticles = null;

    AudioSource audioSource = null;

    public bool IsInFocus { get; private set; } = false;
    public bool HasBeenFocused { get; private set; } = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one * 4f, .75f).SetEase(Ease.InOutBounce);

        audioSource.clip = popup;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }

    public void InLineOfSights()
    {
        if (IsInFocus) return;

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        explosionParticles.Play();

        orbitingParticles.gameObject.SetActive(false);
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(inFocused);
        transform.localScale = Vector3.one * 3f;
        IsInFocus = true;
        HasBeenFocused = true;

        Instantiate(drop.GetRandomCommonDrop(), transform.position + 5 * Vector3.right, Quaternion.identity);
        Instantiate(drop.GetRandomCommonDrop(), transform.position + 5 * Vector3.left, Quaternion.identity);
        Instantiate(drop.GetRandomCommonDrop(), transform.position + 5 * Vector3.up, Quaternion.identity);
        Instantiate(drop.GetRandomCommonDrop(), transform.position + 5 * Vector3.down, Quaternion.identity);
        Instantiate(drop.GetRandomCommonDrop(), transform.position + 5 * Vector3.forward, Quaternion.identity);
        Instantiate(drop.GetRandomCommonDrop(), transform.position + 5 * Vector3.back, Quaternion.identity);
        Instantiate(drop.GetRandomRareDrop(), transform.position, Quaternion.identity);
    }

    public void OutOfLineOfSights()
    {
        if (!IsInFocus) return;


        //audioSource.PlayOneShot(outFocused);
        transform.localScale = Vector3.one;
        IsInFocus = false;
    }
}
