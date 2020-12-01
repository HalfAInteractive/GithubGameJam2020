using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [SerializeField] float blackHoleStrength;
    
    private void OnTriggerStay(Collider c)
    {
        GameObject other = c.gameObject;
        //Vector3 v = other.GetComponent<Rigidbody>().velocity;
        
        Vector3 dir = other.transform.position - transform.position;
        
        if(dir.magnitude > 0.5f)
        {
            other.GetComponent<Rigidbody>().AddForce(dir * blackHoleStrength/(dir.magnitude/100f));
        }
    }
}
