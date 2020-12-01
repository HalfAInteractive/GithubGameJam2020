using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class JoelPlayerController : MonoBehaviour
{

    public GameObject head;
    public float speed;

    [SerializeField]
    public enum State
    {
      paused,
      dialogue,
      playing,
      menu,
      dead,
      cutscene
    }
    
    public State gameState = State.playing;
    public State resumeState = State.playing;

    public float mouseX = 0;
    public float mouseY = 0;
    public float mouseSpeed = 0;
    public float mouseMagnitude = 0;
    public float baseDunkValue = 0;
    public float extremeDunkValue = 0;
    
    public UnityEvent LMBUpEvent;
    public UnityEvent LMBDownEvent;

    public bool W, A, S, D;
    public bool E;
    public bool LMB, RMB, MMB;
    bool focused = true;

    public float xRot, yRot;
    public bool dunking = false;
    public bool extremeDunking = false;
    Timer dunkTimer;
    
    public float gravity;
    public Vector3 velocity;
    public float frictionAir;
    public float frictionGround;
    
    private bool grounded = false;
    public LayerMask layerMask;
    
    public bool ragdollin;
    public GameObject netPos;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        gameState = State.paused;

        new Timer(4.5f, 1, null, () => { gameState = State.playing; });
        
        dunkTimer = new Timer(0.15f, -1, null, () => { dunking = false; extremeDunking = false; });
    }
    
    public void SetState(State s)
    {
      gameState = s;
    }
    
    public void Pause()
    {
      gameState = State.paused;
    }
    
    public void Resume()
    {
      gameState = State.playing;
    }
    
    public void ApplyForce()
    {
      ragdollin = true;
      velocity = Vector3.zero;
      velocity += (transform.position - netPos.transform.position) * 40;
    }

    // Update is called once per frame
    void Update()
    {
      if(focused && !Application.isFocused)
      {
        gameState = State.paused;
        Cursor.lockState = CursorLockMode.None;
      }
      else if(!focused && Application.isFocused)
      {
        gameState = State.playing;
        Cursor.lockState = CursorLockMode.Locked;
      }
      focused = Application.isFocused;
      
      if(gameState == State.playing)
      {
          if(Input.GetKeyDown(KeyCode.W))
          {
              W = true;
          }
          if(Input.GetKeyUp(KeyCode.W))
          {
              W = false;
          }

          if(Input.GetKeyDown(KeyCode.A))
          {
              A = true;
          }
          if(Input.GetKeyUp(KeyCode.A))
          {
              A = false;
          }

          if(Input.GetKeyDown(KeyCode.S))
          {
              S = true;
          }
          if(Input.GetKeyUp(KeyCode.S))
          {
              S = false;
          }

          if(Input.GetKeyDown(KeyCode.D))
          {
              D = true;
          }
          if(Input.GetKeyUp(KeyCode.D))
          {
              D = false;
          }
          
          if(Input.GetKeyDown(KeyCode.E))
          {
              E = true;
          }
          if(Input.GetKeyUp(KeyCode.E))
          {
              E = false;
          }

          if(Input.GetKeyUp(KeyCode.Escape))
          {
            gameState = State.paused;
            Cursor.lockState = CursorLockMode.None;
          }
          
          if(Input.GetMouseButtonDown(0))
          {
            LMB = true;
            if(LMBDownEvent != null)
            {
              LMBDownEvent.Invoke();
            }
          }
          if(Input.GetMouseButtonUp(0))
          {
            LMB = false;
            if(LMBDownEvent != null)
            {
              LMBUpEvent.Invoke();
            }
          }
          
          mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSpeed;
          mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSpeed;

          mouseMagnitude = Mathf.Sqrt(mouseX * mouseX + mouseY * mouseY);

          xRot -= mouseY;
          yRot += mouseX;
          //transform.rotation = new Quaternion(rot.x + mouseX * mouseSpeed, rot.y + mouseY * mouseSpeed, rot.z, 1);
          transform.rotation = Quaternion.Euler(xRot, yRot, 0);

          Vector3 forward = transform.forward;
          Vector3 right = transform.right;

          forward.y = 0;
          right.y = 0;
          
          forward.Normalize();
          right.Normalize();
          
          if(LMB)
          {
            if(mouseMagnitude > baseDunkValue)
            {
              //RaycastHit hit;
              //if(Physics.Raycast(transform.position, transform.TransformDirection(forward), out hit, Mathf.Infinity, layerMask))
              //{
                  Debug.DrawRay(transform.position, transform.forward * 100000, Color.red, 0.5f);
              //}
              dunking = true;
              dunkTimer.Reset();
              
              if(mouseMagnitude > extremeDunkValue)
              {
                extremeDunking = true;
              }
            }
            else
            {
              Debug.DrawRay(transform.position, transform.forward * 100000, Color.yellow, 0.5f);
            }
          }

          if(W)
          {
              velocity += forward;
          }
          if(A)
          {
              velocity -= right;
          }
          if(S)
          {
              velocity -= forward;
          }
          if(D)
          {
              velocity += right;
          }
          
          float friction = frictionAir;
          if(grounded)
          {
            friction = frictionGround;
          }
          if(!ragdollin)
          {
            velocity.Normalize();
            velocity *= speed * friction;
          }
          
          
          //Physics
          RaycastHit hit;
          
          if(Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
          {
            if(hit.distance < gravity * Time.deltaTime)
            {
              grounded = true;
              velocity += Vector3.down * hit.distance;
            }
            else
            {
              grounded = false;
              if(ragdollin)
              {
                velocity += Vector3.down * gravity/10;
              }
              else
              {
                velocity += Vector3.down * gravity;
              }
              
            }
            Debug.DrawRay(transform.position, Vector3.down * hit.distance, Color.red, 0.5f);
          }
          //else
          //{
          //  Debug.DrawRay(transform.position, Vector3.down * 1000, Color.green, 0.5f);
          //}
          

          transform.position = transform.position + velocity * Time.deltaTime;
          if(!ragdollin)
          {
            velocity = Vector3.zero; //velocity * friction;
          }
      }
      else if(gameState == State.paused)
      {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
          gameState = State.playing;
          Cursor.lockState = CursorLockMode.Locked;
        }
        
        W = false;
        A = false;
        S = false; 
        D = false; 
      }
      
    }
}
