using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    public Enemy parent;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent(out PlayerController p))
        {
            parent.PlayerNotUsed = p;
            parent.engaged = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent(out PlayerController p))
        {
            parent.engaged = false;
        }
    }
}
