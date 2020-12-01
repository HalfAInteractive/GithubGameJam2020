using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerController3 : MonoBehaviour
{

    public GameObject head;
    public float speedNormal;
    public float speedSolar;
    
    public GameObject icarusParticles;

    [SerializeField]
    public enum State
    {
      paused,
      playing,
      dialogue,
      menu,
      dead,
      cutscene
    }
    
    public State gameState = State.playing;
    
    public UnityEvent LMBUpEvent;
    public UnityEvent LMBDownEvent;
    
    public GameObject orbiting;

    //inputs flag variables are used to determine if a key is currently pressed.
    Dictionary<string, bool> inputs;

    bool focused = true;

    public Vector3 velocity;
    public float frictionSolar = 1f;
    public float frictionNormal = 0.9f;
    public float friction;
    public Timer frictionTimer;
    public float mouseSpeed;
    
    public float scrollMomentum;
    
    public Texture2D cursor;
    
    public GameObject planetGlow;
    public GameObject planetLight;
    
    float setRange;
    
    float heat = 0;

    void SetupInputs()
    {
      inputs = new Dictionary<string, bool>();
      inputs.Add("W", false);
      inputs.Add("A", false);
      inputs.Add("S", false);
      inputs.Add("D", false);
      inputs.Add("Space", false);
      inputs.Add("LMB", false);
      inputs.Add("RMB", false);
      inputs.Add("MMB", false);
    }
    
    void SetCursor()
    {
      
    }

    // Start is called before the first frame update
    void Start()
    {
      gameState = State.paused;

      new Timer(0.15f, 1, null, () => { gameState = State.playing; });
      
      SetupInputs();
      Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
      frictionTimer = new Timer(1f, -1, null, () => { frictionTimer.Pause(); });
      
      setRange = planetLight.GetComponent<Light>().range;
      
      icarusParticles.transform.localScale = MultiplyAcross(icarusParticles.transform.localScale, transform.localScale); 
    }
    
    Vector3 MultiplyAcross(Vector3 A, Vector3 B)
    {
      return new Vector3(A.x * B.x,A.y * B.y,A.z * B.z);
    }
    
    public void SetState(int i)
    {
      gameState = (State)i;
    }
    
    public void HandleScreenFocus()
    {
      if(focused && !Application.isFocused)
      {
        gameState = State.paused;
        SetupInputs();
      }
      else if(!focused && Application.isFocused)
      {
        gameState = State.playing;
      }
      focused = Application.isFocused;
    }

    // Update is called once per frame
    void Update()
    {
      HandleInputs();
      HandleScreenFocus();
      
      if(gameState == State.playing)
      {
        //mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSpeed;
        //mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSpeed;
        
        if(inputs["LMB"])
        {
          
        }
        
        Vector3 dir = orbiting.transform.position - transform.position;
        Vector3 ortho = new Vector3(dir.z, dir.y, -dir.x);
        
        Vector3 forward = dir;
        Vector3 right = ortho;

        if(inputs["W"])
        {
          velocity += forward;
        }
        if(inputs["A"])
        {
          //GetComponent<Orbit>().magnitude += 0.01f;
          //velocity -= right;
        }
        if(inputs["S"])
        {
          velocity -= forward;
        }
        if(inputs["D"])
        {
          //GetComponent<Orbit>().magnitude -= 0.01f;
          //velocity += right;
        }
        
        scrollMomentum += Input.mouseScrollDelta.y;
        
        if(scrollMomentum > 0)
        {
          velocity -= forward;
        }
        else if(scrollMomentum < 0)
        {
          velocity += forward;
        }
        
        if(dir.magnitude <= 1f) // close to sun
        {
          frictionTimer.Reset();
          frictionTimer.Unpause();
          
          heat = 1;
        }
        
        friction = (frictionTimer.paused) ? frictionNormal : frictionSolar;
        float speed = (frictionTimer.paused) ? speedNormal : speedSolar;
        if(!frictionTimer.paused)
        {
          if(scrollMomentum < 0.0f)
          {
            scrollMomentum = 0.0f;
          }
        }
        scrollMomentum *= friction;
        
        if(scrollMomentum < 0.01f && scrollMomentum > -0.01f)
        {
          scrollMomentum = 0.0f;
        }
        

        
        if(Input.GetMouseButtonUp(2))
        {
          GetComponent<Orbit>().magnitude *= -1;
        }
        
        velocity.Normalize();
        velocity *= speed;
        
        Vector3 testPos = transform.position + velocity * Time.deltaTime;
        Vector3 testVec = orbiting.transform.position - testPos;
        if(testVec.magnitude > 0.4f && testVec.magnitude < 35.0f)
        {
          transform.position = transform.position + velocity * Time.deltaTime;
        }
        
        testVec = orbiting.transform.position - transform.position;
        
        Vector3 testVecDir = testVec;
        testVecDir.Normalize();
        
        icarusParticles.transform.position = transform.position + testVecDir * 0.6f * transform.localScale.x;
        
        
        Vector3 blop = icarusParticles.transform.position - orbiting.transform.position;
        float degree = Mathf.Atan2(blop.z, -blop.x) * 360/(2*Mathf.PI) - 90.0f;
        
        Debug.Log(degree);
        
        icarusParticles.transform.rotation = Quaternion.Euler(0,degree,0);

        float heatMagnitude = testVec.magnitude - 7.0f;
        heatMagnitude = (heatMagnitude < 0) ? heatMagnitude/7.0f : heatMagnitude/35.0f;
        
        if(testVec.magnitude < 7.0f)
        {
          icarusParticles.GetComponent<ParticleSystem>().emissionRate = 10;
        }
        else
        {
          icarusParticles.GetComponent<ParticleSystem>().emissionRate = 0;
        }

        heat -=  heatMagnitude * Time.deltaTime / 7f;
        
        Mathf.Clamp(heat, 0f, 1f);
        
        planetGlow.GetComponent<Renderer>().material.SetFloat("Heat", heat);
        planetLight.GetComponent<Light>().intensity = heat * setRange * 5;
        planetLight.GetComponent<Light>().range = heat * setRange * 2 + setRange/4;
        
        velocity = Vector3.zero; //velocity * friction;
      }
      else if(gameState == State.paused)
      {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
          gameState = State.playing;
        }
      }
    }
    
    public void HandleInputs()
    {
      if(Input.GetKeyDown(KeyCode.W))
      {
        inputs["W"] = true;
      }
      if(Input.GetKeyUp(KeyCode.W))
      {
        inputs["W"] = false;
      }

      if(Input.GetKeyDown(KeyCode.A))
      {
        inputs["A"] = true;
      }
      if(Input.GetKeyUp(KeyCode.A))
      {
        inputs["A"] = false;
      }

      if(Input.GetKeyDown(KeyCode.S))
      {
        inputs["S"] = true;
      }
      if(Input.GetKeyUp(KeyCode.S))
      {
        inputs["S"] = false;
      }

      if(Input.GetKeyDown(KeyCode.D))
      {
        inputs["D"] = true;
      }
      if(Input.GetKeyUp(KeyCode.D))
      {
        inputs["D"] = false;
      }
      
      if(Input.GetKeyDown(KeyCode.Space))
      {
        inputs["Space"] = true;
      }
      if(Input.GetKeyUp(KeyCode.Space))
      {
        inputs["Space"] = false;
      }

      if(Input.GetKeyUp(KeyCode.Escape))
      {
        gameState = State.paused;
        Cursor.lockState = CursorLockMode.None;
      }
      
      if(Input.GetMouseButtonDown(0))
      {
        inputs["LMB"] = true;
        if(LMBDownEvent != null)
        {
          LMBDownEvent.Invoke();
        }
      }
      if(Input.GetMouseButtonUp(0))
      {
        inputs["LMB"] = false;
        if(LMBDownEvent != null)
        {
          LMBUpEvent.Invoke();
        }
      }
    }
}
