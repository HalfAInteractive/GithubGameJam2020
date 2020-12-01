using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class AudioFade : MonoBehaviour
{
    AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        audioSource.volume = 0f;
        audioSource.DOFade(1f, 1.5f);
    }

    public void FadeOut()
    {
        audioSource.DOFade(0f, 1.5f);
    }
}
