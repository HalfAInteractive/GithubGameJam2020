using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ammo/projectile class might be better
// TODO - make this object be poolable. meaning, if I were to deactivate this game object after firing it, 
// all the settings would work the same as if it were to be re-enabled then fire again.
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Moon : MonoBehaviour, IAmmo, IPushable
{
    [SerializeField] Vector3 dir;
    [SerializeField] float speed;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject explosion;
    [SerializeField] TrailRenderer moonTrails;
    [SerializeField] ParticleSystem moonHighlightParticles;
    [SerializeField] List<AudioClip> fireSounds = null;


    [SerializeField] AudioSource audio2d = null;
    [SerializeField] Color selectColor = Color.white;
    [SerializeField] Color inqueueColor = Color.black;

    float lifetime = 5f;
    Rigidbody moonRigidbody = null;
    AudioSource audioSource = null;

    enum State { fired, ready, free };

    bool fired = false; // in possession, fired
    bool ready; // in possession
    bool free; // not in posession

    private void Awake()
    {
        moonRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // BUG TODO - when shooting, it it renters collider, it gets reloaded to the player. 
    // it is fixed, but could be a bandaid fix
    private void OnTriggerEnter(Collider other)
    {
        if (fired)
        {
            // temp enemies and players can't hurt themselves with their own projectiles.
            if (other.TryGetComponent(out IPlayer player))
            {
                if (parent != null && parent != other.gameObject)
                {
                    player.TakeDamage(1);
                    KillObject();
                }
            }

            if (other.TryGetComponent(out IEnemy enemy))
            {
                if (parent != other.gameObject)
                {
                    enemy.TakeDamage(1);
                    KillObject();
                }
            }
        }
        else
        {
            if (other.TryGetComponent(out IPlayer player))
            {
                parent = other.gameObject;
                transform.parent = parent.transform;

                GetComponent<Orbit>().Init(other.gameObject);
                GetComponent<MeshRenderer>().material.color = inqueueColor; // will change to select color if is at the front
                player.Reload(this);
                fired = false;

                // so this won't reload back into the player.
                // there's probably a better way, but it fixes that bug.
                gameObject.layer = GameConstants.PLAYER_LAYER;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                moonTrails.emitting = false;
                moonHighlightParticles.Stop();
                audioSource.Stop(); // stops the continuous hissing sound
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    void KillObject()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);// Instantiate puff of smoke
        Destroy(gameObject); // TODO - probably make it poolable instead
    }
    
    public void Push(Vector3 dir)
    {
        if (fired == false)
        {
            //rb.position = Vector3.Lerp(rb.position, rb.position + dir, .6f);
            transform.position += dir;
            //float goodNumber = 50f;
            //rb.AddForce(dir * goodNumber, ForceMode.Force);
        }
    }

    public bool IsFired() => fired;

    public void Fire(Vector3 dir, float speed, float aimDist)
    {
        audioSource.Play(); 
        foreach (AudioClip sound in fireSounds)
        {
            audio2d.PlayOneShot(sound);
        }
        moonTrails.emitting = true;
        moonRigidbody.isKinematic = false;
        transform.parent = null;

        //Debug.Log(dir + ": " + speed);

        GetComponent<Orbit>().orbiting = null;

        // Local Aim Start
        Vector3 targetPos = parent.transform.position + dir * aimDist;

        if (aimDist == 0)
        {
            targetPos = dir;
        }

        Vector3 aimDir = targetPos - transform.position;
        aimDir.Normalize();
        // Local Aim Finish

#if UNITY_EDITOR
        print($"launching : {gameObject} at {Time.realtimeSinceStartup}");
        Debug.DrawRay(transform.position, aimDir * aimDist, Color.red, 5f);
        //Debug.Log(aimDist);
#endif



        //moonRigidbody.AddForce(aimDir * speed, ForceMode.Impulse);
        moonRigidbody.AddForce(aimDir * speed, ForceMode.Impulse);

        StartCoroutine(AmmoLifetimeAfterFiredCoroutine());

        fired = true;
    }

    public void Select()
    {
        GetComponent<MeshRenderer>().material.color = selectColor;

        //transform.localPosition = new Vector3(1,1,0.5f) * 1.5f;
        //GetComponent<Orbit>().orbiting = null;
    }

    IEnumerator AmmoLifetimeAfterFiredCoroutine()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject); // TODO - probably make it poolable instead
    }
}
