using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using System;

public class Ball : MonoBehaviour
{
    public Vector3 raisedOffset;
    public Vector3 holdOffset;
    public Vector3 dunkOffset;

    public UnityEvent destroyEvent;

    Timer dunkTimer;

    [SerializeField]
    [Range(0.001f, 10.0f)]
    private float dunkDuration = 1.0f;

    [SerializeField]
    private Ease dunkEase = Ease.Linear;

    public UnityEvent explosionEvent;

    public GameObject explosion;

    Vector3 explosionPlacement;

    public Material explosionMaterial;

    public static event Action OnExplode = delegate { };

    // Start is called before the first frame update
    void Start()
    {
      dunkTimer = new Timer(0.75f, -1, null, () => { transform.localPosition = holdOffset; dunkTimer.Pause(); });
      dunkTimer.Pause();
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    
    public void DunkRaise()
    {
      if(gameObject.activeSelf)
      {
        transform.localPosition = raisedOffset;
        dunkTimer.Pause();
        dunkTimer.Reset();
        //transform.DOMove(raisedOffset, dunkDuration).SetEase(dunkEase);
      }
    }
    
    public void DunkRelease()
    {
      if(gameObject.activeSelf)
      {
        transform.localPosition = dunkOffset;
        dunkTimer.Reset();
        dunkTimer.Unpause();
      }
    }
    
    private void OnTriggerEnter(Collider other)
    {
      if(gameObject.activeSelf)
      {
        if(other != null && other.gameObject.name == "Net")
        {
          explosionPlacement = other.gameObject.transform.position;
          new Timer(3.5f, 1, null, ()=>
          {
              GameObject.Instantiate(explosion, explosionPlacement, transform.rotation);
              GetComponent<MeshRenderer>().enabled = false;
              OnExplode();
          });

          transform.position = other.gameObject.transform.position;
          transform.localScale = new Vector3(25, 25, 25);
          GetComponent<MeshRenderer>().material = explosionMaterial;
          GameObject.Destroy(other.gameObject);
          Destroy();
        }
      }
    }
    
    public void Destroy()
    {
      destroyEvent.Invoke();
      
      dunkTimer.Remove();
      //new Timer(1.5f, 1, null, ()=>{SceneManager.LoadScene("PauseMenu");});
      new Timer(10.5f, 1, null, ()=>{SceneManager.LoadScene("PauseMenu");GameObject.Destroy(this.gameObject);});
      new Timer(3.5f, 1, null, ()=>{explosionEvent.Invoke();});
    }
}
