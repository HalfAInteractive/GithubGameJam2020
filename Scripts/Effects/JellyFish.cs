using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class JellyFish : MonoBehaviour
{
    public Vector3 dir;
    public float distance;
    
    [SerializeField]
    private float hoverDuration = 1.0f;

    [SerializeField]
    private Ease hoverEase = Ease.Linear;
    
    public Vector3 targetPos;
    
    public ParticleSystem party;
    
    float A = 20;
    float B = 0;
    bool expanding = false;
    // Start is called before the first frame update
    void Start()
    {
        dir.Normalize();

        transform.DOMove(targetPos, hoverDuration).SetEase(hoverEase);
        ParticleSystem.ShapeModule s = party.shape;
        
        // letting you know this conflicts with scene switching. 
        // so i disabled it on the camera. 
        Timer tim = new Timer(hoverDuration, -1, (float percent) => {s.angle = Easing.Mix(A, B, percent);}, () => 
        {
            targetPos += dir * distance;
            transform.DOMove(targetPos, hoverDuration).SetEase(hoverEase);
            //float C = A;
            //A = B;
            //B = C;
        });
    }
}
