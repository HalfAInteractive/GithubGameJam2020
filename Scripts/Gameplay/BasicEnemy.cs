using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour, IEnemy
{
    public void Boost(float amount, ForceMode forceMode){}

    public void Bounce(float amount, Vector3 targetPos, ForceMode forceMode){}

    public void TakeDamage(int amount) { } //transform.gameObject.SetActive(false); }
}
