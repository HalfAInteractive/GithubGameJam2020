using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] float boostAmount = 100f;

    AudioSource audioSource = null;
    [SerializeField] List<AudioClip> sounds = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

#if UNITY_EDITOR
        if( audioSource.outputAudioMixerGroup == null )
        {
            Debug.LogError($"ERR: Missing audio mixer group on audiosource on {this}. Please add its respective audio mixer (Music, Sound, UI etc).", this);
        }
#endif

    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO - Boost player when player enters the trigger collider
        // Check if it is the player, then 
        //  1. Boost it by passing the boostAmount.
        //      - to confirm if the boost occurs, check console log. 
        //        It will print a statement saying it is boosting player by an amount.
        //  2. Play the audio when it confirms that the player passed through and boosts them.
        //
        // Just a thought - how would you change your code to make this also boost an enemy?
        
        List<IMoveable> boostList;
        
        InterfaceUtility.GetInterfaces<IMoveable>(out boostList, other.gameObject);
        
        foreach(IMoveable boostable in boostList)
        {
            foreach(AudioClip sound in sounds)
            {
                audioSource.PlayOneShot(sound);
            }
            boostable.Boost(boostAmount, ForceMode.Acceleration);
        }

        /* answer to the problem
        if(other.TryGetComponent(out IBoostable boostableObj))
        {
            boostableObj.Boost(boostAmount, ForceMode.Acceleration);
            audioSource.Play(); 
        }
        */

    }
}
