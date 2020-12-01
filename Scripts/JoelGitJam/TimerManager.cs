using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static List<Timer> timers;
    public static List<Timer> removeTimers;
  
    // Start is called before the first frame update
    void Awake()
    {
      timers = new List<Timer>();
      removeTimers = new List<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Timer t in new List<Timer>(timers))
        {
            t.Update();
        }
        foreach(Timer t in removeTimers)
        {
            timers.Remove(t);
        }
        removeTimers.Clear();
    }
    
    public static void AddTimer(Timer t)
    {
        timers.Add(t);
    }
    
    public static void RemoveTimer(Timer t)
    {
        removeTimers.Add(t);
    }
    
    public static void Empty()
    {
      timers.Clear();
    }
}
