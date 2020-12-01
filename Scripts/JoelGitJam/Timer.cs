using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Timer
{
    public delegate void Del();
    public delegate void Del2(float percent);
    
    public float elapsedTime;
    public float endTime;
    public bool paused;
    public int loops;
    public bool endless;

    
    public Del2 updateFunc;
    public Del endFunc;
    
    public Timer(float time = 1, int numLoops = 1, Del2 up = null, Del end = null)
    {
      endTime = (time > 0) ? time : 1; // if this is zero, could loop indefinitely
      loops = numLoops;
      updateFunc = up;
      endFunc = end;
      
      paused = false;
      elapsedTime = 0;
      endless = (numLoops <= 0) ? true : false;
      
      TimerManager.AddTimer(this);
    }

    // Update is called once per frame
    public void Update()
    {
      if(!paused)
      {
        elapsedTime += Time.deltaTime;
        if(updateFunc != null)
          updateFunc(GetPercent());
        float count = 0;
        while(elapsedTime > endTime)
        {
          elapsedTime -= endTime;
          loops--;
          if(endFunc != null)
          {
            endFunc();
          }
          if(!endless && loops <= 0)
          {
            Remove();
            return;
          }
          if(count++ > 100)
          {
            Debug.Log("Timer Ran too long");
            Remove();
            return;
          }
        }
      }
    }
    
    public float GetPercent()
    {
      return elapsedTime/endTime;
    }
    
    public void Pause()
    {
      paused = true;
    }
    
    public void Unpause()
    {
      paused = false;
    }
    
    public void Call()
    {
      if(endFunc != null)
      {
        endFunc();
      }
    }
    
    public void Reset()
    {
      elapsedTime = 0;
    }
    
    public void Set(float percent)
    {
      elapsedTime = endTime * percent;
    }
    
    public void Remove()
    {
      TimerManager.RemoveTimer(this);
      updateFunc = null;
      endFunc = null;
    }
}
