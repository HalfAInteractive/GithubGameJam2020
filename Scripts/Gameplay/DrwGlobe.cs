using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrwGlobe : MonoBehaviour
{
    
    [SerializeField] List<AudioClip> enterSounds = null;
    [SerializeField] List<AudioClip> exitSounds = null;
    [SerializeField] List<Material> materials;
    [SerializeField] float glowTime = 1.5f;

    AudioSource audioSource = null;

    bool awake = false;
    
    Timer triggerExitTimer = null;
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

#if UNITY_EDITOR
        if( audioSource.outputAudioMixerGroup == null )
        {
            Debug.LogError($"ERR: Missing audio mixer group on audiosource on {this}. Please add its respective audio mixer (Music, Sound, UI etc).", this);
        }
#endif
        awake = true;

    }
    
    private void OnTriggerEnter(Collider other)
    {   
    
        if(other.TryGetComponent(out IPlayer player))
        {
            GetComponent<Renderer>().material = materials[0];
            
            foreach(AudioClip sound in enterSounds)
            {
                //audioSource.PlayOneShot(sound);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {      
        if(other.TryGetComponent(out IPlayer player))
        {
            if(triggerExitTimer != null)
            {
                triggerExitTimer.Reset();
            }
            else
            {
                triggerExitTimer = new Timer(glowTime, 1, null, () => {GetComponent<Renderer>().material = materials[1]; triggerExitTimer = null;});
            }
            foreach(AudioClip sound in exitSounds)
            {
                //audioSource.PlayOneShot(sound);
            }
        }
    }
    
}
