using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    
    public LevelGenerator generator;
    public int index;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void Set(LevelGenerator g)
    {
        generator = g;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toPlayer = transform.position - generator.player.transform.position;
        if(toPlayer.magnitude >= generator.maxRange)
        {
            Finished();
        }
    }
    
    public void Finished()
    {
        generator.count--;
        generator.FreeZone(index);
        GameObject.Destroy(this.gameObject);
    }
}
