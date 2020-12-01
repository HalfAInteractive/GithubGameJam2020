using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hover : MonoBehaviour
{

    [SerializeField]
    public Vector3 targetPos = Vector3.zero;
    

    [SerializeField]
    [Range(0.001f, 10.0f)]
    private float hoverDuration = 1.0f;

    [SerializeField]
    private Ease hoverEase = Ease.Linear;
    
    public Timer hoverTimer;

    // Start is called before the first frame update
    void Start()
    {
      Vector3 startPos = transform.position;
      
      transform.DOMove(targetPos, hoverDuration).SetEase(hoverEase);
      
      hoverTimer = new Timer(hoverDuration+0.01f, -1, null, () =>
      {
        if(transform.position == targetPos)
        {
          transform.DOMove(startPos, hoverDuration).SetEase(hoverEase);
        }
        else
        {
          transform.DOMove(targetPos, hoverDuration).SetEase(hoverEase);
        }
      });
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    
    void OnDestroy()
    {
      hoverTimer.Remove();
      DOTween.Kill(transform);
    }
    
}
