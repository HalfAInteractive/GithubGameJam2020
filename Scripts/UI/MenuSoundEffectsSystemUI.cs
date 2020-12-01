using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MenuSoundEffectsSystemUI : MonoBehaviour
{
    public static MenuSoundEffectsSystemUI Play { get; private set; }

    [Header("UI Sound Effects")]
    [SerializeField] AudioClip buttonSelected = null;
    [SerializeField] AudioClip buttonSubmitted = null;
    [SerializeField] AudioClip buttonBack = null;

    AudioSource audioSource = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if(Play == null)
        {
            Play = this;
        }
    }

    public void ButtonSelected()
    {
        audioSource.PlayOneShot(buttonSelected);
    }

    public void ButtonSubmitted()
    {
        audioSource.PlayOneShot(buttonSubmitted);
    }

    public void ButtonBack()
    {
        audioSource.PlayOneShot(buttonBack);
    }

    public void SliderValueChanged()
    {
        audioSource.PlayOneShot(buttonSelected);
    }
}
