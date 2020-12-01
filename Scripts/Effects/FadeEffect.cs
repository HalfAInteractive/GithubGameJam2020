using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    private void Start()
    {
        GetComponent<GlobalVolumeController>().FadeIn(rate : .5f);
    }
}
