using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Test : MonoBehaviour
{
    //Testing Testing 1,2,3.
    // Start is called before the first frame update
    // 28913712983172389724767846283623486788346238437863782322
    void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => print("butt"));
        seq.AppendInterval(5f);
        seq.AppendCallback(() => print("poop"));
    }
}
