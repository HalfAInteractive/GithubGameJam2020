using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public UnityEvent interactEvent;
    public UnityEvent inRangeEvent;
    
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
      if(player != null && player.GetComponent<JoelPlayerController>().E)
      {
        Interact();
      }
    }
    
    public void Interact()
    {
      interactEvent.Invoke();
    }
    
        //When the Primitive collides with the walls, it will reverse direction
    private void OnTriggerEnter(Collider other)
    {
      if(gameObject.activeSelf)
      {
        if(other.gameObject.name == "Player")
        {
          player = other.gameObject;
        }
      }
    }

    //When the Primitive exits the collision, it will change Color
    private void OnTriggerExit(Collider other)
    {
      if(gameObject.activeSelf)
      {
        if(other.gameObject != null)
        {
          JoelPlayerController temp = other.gameObject.GetComponent<JoelPlayerController>();
          if(temp != null && temp.E)
          {
            player = null;
          }
        }
      }
    }
    
    public void Destroy()
    {
      GameObject.Destroy(this.gameObject);
    }
}
