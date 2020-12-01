using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebaseSphere : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {   
        if(other.TryGetComponent(out IPlayer player))
        {
            FloatingOrigin.Rebase(transform.position);
            // Should the Rebase Sphere Destroy itself?
            GameObject.Destroy(this.gameObject);
        }
    }
}
