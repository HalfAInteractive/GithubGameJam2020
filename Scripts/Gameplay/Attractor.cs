using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attracts anything with a rigidbody that comes into its trigger sphere
/// and pulls it towards its center.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class Attractor : MonoBehaviour
{
    [SerializeField] float strength = 10f;
    float minStrength = 0.00001f;

    private void OnValidate()
    {
        strength = strength < minStrength ? minStrength : strength;
    }

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out Rigidbody rigidbody))
        {
            Vector3 dir = transform.position - rigidbody.position;
            rigidbody.AddForce(dir * strength, ForceMode.Force);
        }
    }
}
