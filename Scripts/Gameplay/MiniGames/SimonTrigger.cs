using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimonTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] List<Color> colors;
    int id = 0;
    ITriggerManager manager;
    bool locked = false;
    bool flickering = false;
    int index = 0;

    Timer flickerTimer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Trigger()
    {
        manager.Broadcast(id);
    }

    public void SetId(int i, ITriggerManager m)
    {
        id = i;
        manager = m;
    }

    public void SetLock(bool l)
    {
        locked = l;
    }

    public void Broadcast(int data)
    {
        if (data == 1)
        {
            Flicker(1);
        }
        else if (data == 0)
        {
            Flicker(3);
        }
    }

    private void Flicker(int num)
    {
        if (flickerTimer != null)
            flickerTimer.Remove();
        flickerTimer = new Timer(1f/(num*2), num * 2, null, () => { flickering = !flickering; Color c = GetComponent<Renderer>().material.color = (flickering) ? colors[1] : colors[0]; });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (locked == false)
        {
            if (other.TryGetComponent(out IPlayer player))
            {
                Trigger();
            }
            else if(other.TryGetComponent(out IAmmo ammo))
            {
                if(ammo.IsFired())
                    Trigger();
            }
        }
    }
}
