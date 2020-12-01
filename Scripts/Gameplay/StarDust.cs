using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StarDust : MonoBehaviour, IPushable
{
    [SerializeField] int coinValue = 1;
    [SerializeField] ParticleSystem starDustParticles = null;
    [SerializeField] ParticleSystem confirmPickupParticles = null;
    [SerializeField] List<AudioClip> pickupSounds = null;
    [SerializeField] List<GameObject> starShapes = null;    

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IPlayer player))
        {
            player.CollectCoin(coinValue);

            var audioSource = GetComponent<AudioSource>();
            foreach(AudioClip sound in pickupSounds)
            {
                audioSource.pitch = Random.Range(0.95f, 1.05f);
                audioSource.PlayOneShot(sound);
            }

            GetComponent<Collider>().enabled = false;
            //GetComponent<Renderer>().enabled = false;

            Color color = starDustParticles.startColor;
            color.a = 0.1f;
            starDustParticles.startColor = color;
            confirmPickupParticles.Play();
            StartCoroutine(AfterCollectedCoroutine());

            foreach (var star in starShapes)
                star.SetActive(false);

        }
    }
    
    IEnumerator AfterCollectedCoroutine()
    {
        yield return new WaitForSeconds(1f);
        starDustParticles.Stop();

        yield return new WaitForSeconds(1.8f);
        gameObject.SetActive(false);
    }

    public void Push(Vector3 dir)
    {
        transform.position += dir;
    }
}
