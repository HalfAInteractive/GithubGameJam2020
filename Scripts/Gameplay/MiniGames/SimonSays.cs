using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HumanRandom))]
[RequireComponent(typeof(Emitter))]
[RequireComponent(typeof(AudioSource))]
public class SimonSays : MonoBehaviour, ITriggerManager
{
    [SerializeField] [Range(1, 12)] int toWin = 3;
    [SerializeField] [Range(1,6)] int inUse = 3;
    [SerializeField] List<GameObject> triggers;
    [SerializeField] List<AudioClip> goodSounds = null;
    [SerializeField] List<AudioClip> badSounds = null;
    [SerializeField] List<AudioClip> victorySounds = null;
    [SerializeField] Emitter SpawnEmitter;
    HumanRandom random;
    List<int> sequence;
    int current = 0;
    int counter = 0;
    int progress = 1;

    AudioSource audioSource = null;

    void Start()
    {
        triggers = SpawnEmitter.Emit();
        SpawnEmitter.isEnabled = false;

        print(triggers.Count);

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

        SetSequence();

        SetCurrent();
    }

    void SetCurrent()
    {

    }

    void SetSequence()
    {
        if (sequence.Count > 0)
        {
            sequence.Clear();
            random.Reset();
        }
        for (int i = 0; i < triggers.Count; i++)
        {
            sequence.Add(random.GetNextRand());
        }
    }

    void PlaySequence()
    {
        counter = 0;
        new Timer(1.5f, progress, null, () => {
            if (triggers[sequence[counter]].TryGetComponent(out ITrigger trigger))
            {
                trigger.Broadcast(1);
            }
            counter++;
        });

        Debug.Log(progress);

        new Timer(progress + 1, 1, null, () =>
        {
            foreach (GameObject g in triggers)
            {
                if (g.TryGetComponent(out ITrigger trigger2))
                {
                    trigger2.SetLock(false);
                }
            }
        });
    }

    void SetFlash()
    {

    }

    public void Broadcast(int triggerId)
    {
        if (triggers[triggerId].TryGetComponent(out ITrigger trigger))
        {
            if (triggerId == sequence[current])
            {
                current++;

                if (current == toWin)
                {
                    Done();
                }
                else
                {
                    foreach (AudioClip sound in goodSounds)
                    {
                        audioSource.PlayOneShot(sound);
                    }

                    trigger.Broadcast(1); //Good
                    trigger.SetLock(true);

                    if (current == progress)
                    {
                        progress++;
                        current = 0;

                        foreach (AudioClip sound in goodSounds)
                        {
                            audioSource.PlayOneShot(sound);
                        }

                        new Timer(1.5f, 1, null, () => { PlaySequence(); });
                    }
                    else
                    {
                        new Timer(1.5f, 1, null, () => { trigger.SetLock(false); });
                    }
                }
            }
            else
            {
                current = 0;
                progress = 1;

                foreach (AudioClip sound in badSounds)
                {
                    audioSource.PlayOneShot(sound);
                }

                foreach (GameObject g in triggers)
                {
                    if (g.TryGetComponent(out ITrigger trigger2))
                    {
                        trigger2.Broadcast(0); //Bad
                        trigger2.SetLock(true);
                    }
                }

                SetSequence();

                new Timer(2.0f, 1, null, () => { PlaySequence(); });
            }
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
            GameObject.Destroy(g);
        }

        GameObject.Destroy(this);
    }
}
