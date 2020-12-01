using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class EnemyProjectile : MonoBehaviour, IAmmo
{
    [SerializeField] bool followPlayer;
    [SerializeField] float speed;
    [SerializeField] List<AudioClip> fireSounds;

    float lifetime = 5f;
    Rigidbody rigidBody = null;
    AudioSource audioSource = null;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Fire(Vector3 dir, float s, float aimDist)
    {
        foreach (AudioClip sound in fireSounds)
        {
            audioSource.PlayOneShot(sound);
        }

        //moonRigidbody.AddForce(aimDir * speed, ForceMode.Impulse);
        rigidBody.AddForce(dir * s, ForceMode.Impulse);

        StartCoroutine(AmmoLifetimeAfterFiredCoroutine());
    }

    public void Select()
    {

    }

    public bool IsFired()
    {
        return true;
    }

    void OnDisable()
    {
        var rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    IEnumerator AmmoLifetimeAfterFiredCoroutine()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false); // TODO - probably make it poolable instead
    }
}
