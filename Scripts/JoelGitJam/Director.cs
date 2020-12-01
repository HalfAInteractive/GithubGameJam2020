using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Director : MonoBehaviour
{
    public int priority = 11;
    [SerializeField]
    public List<float> shotDurations;
    [SerializeField]
    public List<CinemachineVirtualCamera> cameras;
    
    Queue<float> durationQ;
    Queue<CinemachineVirtualCamera> cameraQ;
    // Start is called before the first frame update
    void Start()
    {
      durationQ = new Queue<float>();
      cameraQ = new Queue<CinemachineVirtualCamera>();
      foreach(float shot in shotDurations)
      {
        durationQ.Enqueue(shot);
      }
      foreach(CinemachineVirtualCamera cam in cameras)
      {
        cameraQ.Enqueue(cam);
      }
      NextShot();
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    
    public void NextShot()
    {
      new Timer(durationQ.Dequeue(), 1, null, () => { cameraQ.Dequeue().Priority = priority++; });
    }
}
