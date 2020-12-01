using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStream : MonoBehaviour
{
    [SerializeField] float streamStrength;
    [SerializeField] Transform streamStart;
    [SerializeField] Transform streamEnd;
    
    Vector3 dir = Vector3.zero;
    
    public void Start()
    {
        dir =  streamStart.position - streamEnd.position;
        dir.Normalize();
    }
    
    private void OnTriggerStay(Collider c)
    {
        GameObject other = c.gameObject;
        
        Vector3 v = other.GetComponent<Rigidbody>().velocity;
        
        if(v.magnitude < streamStrength)
        {
            if(other.GetComponent<Rigidbody>().velocity.magnitude < streamStrength)
                other.GetComponent<Rigidbody>().AddForce(dir * Time.deltaTime * streamStrength);
        }
    }
}
