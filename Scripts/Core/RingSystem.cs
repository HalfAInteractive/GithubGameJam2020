using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Rotates ring based off of player's velocity
/// </summary>
public class RingSystem : MonoBehaviour
{
    [SerializeField] 
    [Range(0f, 1f)]
    float rotationLerpVal = .2f;

    [SerializeField]
    [Range(0f, 100f)]
    float rotationStrength = 2f;

    Rigidbody playerRigidbody = null;
    Quaternion rotateTo = Quaternion.identity;
    Vector3 velocity = Vector3.zero, ringScale = Vector3.zero;

    void Awake()
    {
        ringScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(ringScale, 1f).SetEase(Ease.OutBounce);
    }

    private void Start()
    {
        playerRigidbody = GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        velocity = playerRigidbody.velocity * rotationStrength;
        rotateTo = Quaternion.Euler(velocity.z, 0f, -velocity.x);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotateTo, rotationLerpVal);
    }
}
