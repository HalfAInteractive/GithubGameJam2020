using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDamageEffect : MonoBehaviour
{
    [SerializeField] AudioClip hurtSFX = null;
    [SerializeField] GameObject explosionPrefab = null;

    [SerializeField] Color secondLifeColor;
    [SerializeField] Color lastLifeColor;

    PlayerController PC = null;
    MeshRenderer meshRenderer = null;
    Color playerColor = Color.black;
    AudioSource audioSource = null;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        PC = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

        playerColor = meshRenderer.material.color;
    }

    private void Start()
    {
        PC.OnPlayerDamaged += ShowHurtEffect;
        PC.OnPlayerKilled += ShowDiedEffect;
    }

    private void OnDestroy()
    {
        PC.OnPlayerDamaged -= ShowHurtEffect;
        PC.OnPlayerKilled -= ShowDiedEffect;
    }

    void ShowHurtEffect(int currHealth)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(meshRenderer.material.DOColor(Color.black, .1f).SetEase(Ease.Linear));
        seq.AppendInterval(1f);

        Color healthColor = playerColor;
        if(currHealth == 2)
        {
            healthColor = secondLifeColor;
        }
        else if(currHealth == 1)
        {
            healthColor = lastLifeColor;
        }


        seq.Append(meshRenderer.material.DOColor(healthColor, 1f).SetEase(Ease.Linear));

        audioSource.PlayOneShot(hurtSFX);
        GlobalVolumeController.PlayVisualFX.HurtVisionEffect();
    }

    void ShowDiedEffect()
    {
        GlobalVolumeController.PlayVisualFX.ExplodeEffect();
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
