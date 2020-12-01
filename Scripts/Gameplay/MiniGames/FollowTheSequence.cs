using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(HumanRandom))]
[RequireComponent(typeof(Emitter))]
[RequireComponent(typeof(AudioSource))]
public class FollowTheSequence : MonoBehaviour, ITriggerManager
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

        random = GetComponent<HumanRandom>();
        random.SetRange(0, triggers.Count);

        sequence = new List<int>();

        int count = 0;
        foreach (GameObject g in triggers)
        {
            if (g.TryGetComponent(out ITrigger trigger))
            {
                trigger.SetId(count, this);
                count++;
            }
        }

        audioSource = GetComponent<AudioSource>();

        if (timeToComplete > 0)
        {
            resetTimer = new Timer(timeToComplete, -1, null, Reset);
            resetTimer.Pause();
        }

        SetSequence();

        SetCurrent();
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
        resetTimer.Pause();

        SetCurrent();

        foreach (AudioClip sound in badSounds)
        {
            audioSource.PlayOneShot(sound);
        }
    }

    void SetCurrent()
    {
        if (triggers[sequence[current]].TryGetComponent(out ITrigger trigger))
        {
            trigger.Broadcast(2);
        }
    }

    void SetSequence()
    {
        for (int i = 0; i < triggers.Count; i++)
        {
            sequence.Add(random.GetNextRandSingle());
        }
    }

    public void Broadcast(int triggerId)
    {
        if (triggers[triggerId].TryGetComponent(out ITrigger trigger))
        {
            if (triggerId == sequence[current])
            {
                current++;
                if(current == 1)
                {
                    if(resetTimer != null)
                    {
                        resetTimer.Unpause();
                    }
                }
                foreach (AudioClip sound in goodSounds)
                {
                    audioSource.PlayOneShot(sound);
                }

                trigger.Broadcast(1); //Good

                if (current == triggers.Count)
                {
                    Done();
                }
            }
            else
            {
                current = 0;

                foreach (AudioClip sound in badSounds)
                {
                    audioSource.PlayOneShot(sound);
                }

                foreach (GameObject g in triggers)
                {
                    if (g.TryGetComponent(out ITrigger trigger2))
                    {
                        trigger2.Broadcast(0); //Bad
                    }
                }
            }
            SetCurrent();
        }
    }

    private void Done()
    {
        foreach (AudioClip sound in victorySounds)
        {
            audioSource.PlayOneShot(sound);
        }
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

        OnSequenceComplete();
        new Timer(1, 1, null, () => { gameObject.SetActive(false); }); // deleting object isn't really needed. deactivating the object is enough.
        //GameObject.Destroy(this);
    }
}
