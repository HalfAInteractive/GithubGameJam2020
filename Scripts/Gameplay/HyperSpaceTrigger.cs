using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HyperSpaceTrigger : MonoBehaviour
{
    [SerializeField] List<AudioClip> pickupSounds = null;
    private void Awake()
    {
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IPlayer player))
        {
            //GetComponent<AudioSource>().Play();
            GlobalVolumeController.PlayVisualFX.HyperspaceVision(intensity: -1f, duration: 4f, inTransitionSpeed: 1f, outTransitionSpeed: 0.25f);
            
            foreach(AudioClip sound in pickupSounds)
            {
                GetComponent<AudioSource>().PlayOneShot(sound);
            }
        }
    }
}
