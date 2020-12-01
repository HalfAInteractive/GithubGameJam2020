using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeControllerUI : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    string masterStr = "master", musicStr = "music", soundStr = "sound", uiStr = "ui";

    [Header("Sliders volumes")]
    [SerializeField] Slider master;
    [SerializeField] Slider music;
    [SerializeField] Slider sound;
    [SerializeField] Slider ui;

    public void MasterVolumeChanged(float val)
    {
        MenuSoundEffectsSystemUI.Play?.SliderValueChanged();
        audioMixer.SetFloat(masterStr, SliderToDecibelConversion(val / master.maxValue));
    }

    public void MusicVolumeChanged(float val)
    {
        MenuSoundEffectsSystemUI.Play?.SliderValueChanged();
        audioMixer.SetFloat(musicStr, SliderToDecibelConversion(val / music.maxValue));
    }

    public void SoundVolumeChanged(float val)
    {
        MenuSoundEffectsSystemUI.Play?.SliderValueChanged();
        audioMixer.SetFloat(soundStr, SliderToDecibelConversion(val / sound.maxValue));
    }

    public void UIVolumeChanged(float val)
    {
        MenuSoundEffectsSystemUI.Play?.SliderValueChanged();
        audioMixer.SetFloat(uiStr, SliderToDecibelConversion(val / ui.maxValue));
    }

    float SliderToDecibelConversion(float val)
    {
        if (val <= 0.1f) val = 0.001f;

        return 20f * Mathf.Log10(val);
    }
}
