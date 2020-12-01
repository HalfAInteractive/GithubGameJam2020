using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarpPortal : MonoBehaviour
{
//#if UNITY_EDITOR
//    [TextArea(2,5)]
//    public string Note = "When scaling this warp portal, scale the box collider and not this parent object.";
//#endif

    [Space(5)]
    [SerializeField] AudioClip startup;
    [SerializeField] Level WarpToLevel;

    GameManager gameManager = null;
    

    private void Awake()
    {
#if UNITY_EDITOR
        if(WarpToLevel == Level.Default)
        {
            Debug.LogError($"ERR: WarpToLevel is not set on {gameObject.name}. Set to level name where this warp portal will warp you to.", this);
            Debug.Break();
        }
#endif

        transform.localScale = Vector3.zero;
        GetComponent<Collider>().enabled = false;
    }


    private void Start()
    {
        gameManager = HelperFunctions.FindGameManagerInBaseScene();
    }

    private void OnEnable()
    {
        transform.DOScale(Vector3.one, 1f)
            .SetEase(Ease.InOutBack)
            .OnComplete(() => GetComponent<Collider>().enabled = true);

        GetComponent<AudioSource>().PlayOneShot(startup);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IPlayer player))
        {
            //print("tsetsetsetsetset going to next scene");
            GetComponent<Collider>().enabled = false;
            gameManager.InitiateLevelChange(WarpToLevel);
        }
    }
}
