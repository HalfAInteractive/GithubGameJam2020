using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damage sphere for testing purposes. Can be used in an actual gameplay.
/// </summary>
public class DamageSphere : MonoBehaviour
{
    [SerializeField]
    float bounceStrength = 10;

    [SerializeField]
    int damageAmount = 1;
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IPlayer player))
        {
            player.TakeDamage(damageAmount);
            player.Bounce(bounceStrength, transform.position, ForceMode.VelocityChange);
        }

        /* if you want to damage enemies or anything that can take damage, it would just 
         * need the IDamageable interface and it can be damaged if it is implemented.
        if(other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damageAmount);
        }
        */
    }

    // calls the same tick amount as fixed update so keep that in mind.
    // if you want to do damage over time, you would use this.
    // or can use a combination of triggerEnter and triggerExit
    //private void OnTriggerStay(Collider other)
    //{
    //}
}
