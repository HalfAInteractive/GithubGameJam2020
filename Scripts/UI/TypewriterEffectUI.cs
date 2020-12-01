using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypewriterEffectUI : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("Time it takes to finish the typewriting")]
    float goalTime = 1f;

    AudioSource audioSource = null;
    TMP_Text words = null;

    private void Awake()
    {
        words = GetComponent<TMP_Text>();
    }
    private void OnValidate()
    {
        goalTime = goalTime <= 0f ? 0.1f : goalTime;
    }

    private void OnEnable()
    {
        StartCoroutine(TypewriteCoroutine());   
    }

    public void Perform()
    {
        StartCoroutine(TypewriteCoroutine());
    }

    IEnumerator TypewriteCoroutine()
    {
        words.maxVisibleCharacters = 0;
        audioSource = GetComponent<AudioSource>();
        yield return new WaitForSeconds(1f);

        int maxCharCount = words.text.Length,
        currCharCount = 0;

        float typewritterSpeed = goalTime / ((float)maxCharCount);
        var wait = new WaitForSeconds(typewritterSpeed);
        while (currCharCount <= maxCharCount)
        {
            if(audioSource != null)
            {
                audioSource.Play();
            }

            words.maxVisibleCharacters = currCharCount;
            currCharCount++;
            yield return wait;
        }
    }
}
