using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SequenceTrigger : MonoBehaviour, ITrigger
{
    [SerializeField] List<Material> materials;
    int id = 0;
    ITriggerManager manager;
    bool locked = false;

    public void Trigger()
    {
         manager.Broadcast(id);
    }

    public void SetLock(bool l)
    {
        
    }

    public void SetId(int i, ITriggerManager m)
    {
        id = i;
        manager = m;
    }

    public void Broadcast(int data)
    {
        GetComponent<Renderer>().material = materials[data];
        locked = data == 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!locked)
        {
            if (other.TryGetComponent(out IPlayer player) || other.TryGetComponent(out IAmmo ammo))
            {
                Trigger();
            }
        }
    }
}
