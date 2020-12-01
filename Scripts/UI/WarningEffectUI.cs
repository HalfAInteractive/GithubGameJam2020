using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarningEffectUI : MonoBehaviour
{
    [SerializeField] int numOfIndicators = 3;
    [SerializeField] float indicatorInvertal = .5f;

    private void OnEnable()
    {
        StartCoroutine(WarningIndicatorsCoroutine());
    }

    IEnumerator WarningIndicatorsCoroutine()
    {
        TMP_Text words = GetComponent<TMP_Text>();
        AudioSource audioSource = GetComponent<AudioSource>();

        int currIndicators = 1;
        var wait = new WaitForSeconds(indicatorInvertal);

        while(currIndicators < numOfIndicators)
        {
            words.fontStyle = FontStyles.Bold;
            audioSource.pitch = 1f;
            audioSource?.Play();
            yield return wait;

            
            words.fontStyle = FontStyles.Normal;
            audioSource.pitch = 0.6f;
            audioSource?.Play();
            yield return wait;

            currIndicators++;
        }

        words.fontStyle = FontStyles.Bold;
        audioSource.pitch = 1f;
        audioSource?.Play();
    }
}
