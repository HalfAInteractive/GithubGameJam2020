using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    TMP_Text fps = null;
    int deltaTime = 0;

    private void Awake()
    {
        fps = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        StartCoroutine(DisplayFPSText());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        deltaTime = (int)(1f / Time.unscaledDeltaTime);
    }

    IEnumerator DisplayFPSText()
    {
        var wait = new WaitForSeconds(.5f);
        while (true)
        {
            fps.text = $"{deltaTime}";
            yield return wait;
        }
    }
}
