using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(HumanRandom))]
[RequireComponent(typeof(Emitter))]
[RequireComponent(typeof(AudioSource))]
public class GetAllRings : MonoBehaviour, ITriggerManager
{
    List<GameObject> triggers;
    [SerializeField] List<AudioClip> goodSounds = null;
    [SerializeField] List<AudioClip> badSounds = null;
    [SerializeField] List<AudioClip> victorySounds = null;
    [SerializeField] Emitter SpawnEmitter;
    [SerializeField] float timeToComplete = 0f;
    Timer resetTimer = null;
    HumanRandom random;
    List<int> sequence;
    int current = 0;

    AudioSource audioSource = null;

    public event Action OnSequenceComplete = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        triggers = SpawnEmitter.Emit();
        SpawnEmitter.isEnabled = false;

        int count = 0;
        foreach (GameObject g in triggers)
        {
            if (g.TryGetComponent(out ITrigger trigger))
            {
                trigger.SetId(count, this);
                count++;
            }
        }

        if(timeToComplete > 0)
        {
            resetTimer = new Timer(timeToComplete, -1, null, Reset);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void Reset()
    {
        foreach (GameObject g in triggers)
        {
            if (g.TryGetComponent(out ITrigger trigger))
            {
                trigger.Broadcast(0);
            }
        }
        current = 0;

        resetTimer.Reset();

        foreach (AudioClip sound in badSounds)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    public void Broadcast(int triggerId)
    {
        if (triggers[triggerId].TryGetComponent(out ITrigger trigger))
        {
            current++;


            if (current == 1)
            {
                if (resetTimer != null)
                {
                    resetTimer.Unpause();
                }
            }

            if (current == triggers.Count)
            {
                Done();
            }
            else
            {
                foreach (AudioClip sound in goodSounds)
                {
                    audioSource.pitch = 1 + 0.09f * current;
                    audioSource.PlayOneShot(sound);
                }
                trigger.Broadcast(1);
            }
        }
    }

    private void Done()
    {
        foreach (AudioClip sound in victorySounds)
        {
            audioSource.PlayOneShot(sound);
        }

        new Timer(0.3f, 3, null, () => {
            foreach (AudioClip sound in goodSounds)
            {
                audioSource.pitch = 1 + 0.09f * current++;
                audioSource.PlayOneShot(sound);
            }
        });

        Emitter[] myEmitters = GetComponents<Emitter>();
        foreach (Emitter e in myEmitters)
        {
            e.Emit();
        }

        foreach (GameObject g in triggers)
        {
            //GameObject.Destroy(g);
            g.SetActive(false); // deleting object isn't really needed. deactivating the object is enough.
        }

        if(resetTimer != null)
            resetTimer.Remove();

        OnSequenceComplete();
        new Timer(1, 1, null, () => { gameObject.SetActive(false); }); // deleting object isn't really needed. deactivating the object is enough.
        //GameObject.Destroy(this);
    }
}
