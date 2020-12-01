using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] float blackHoleStrength;
    [SerializeField] float effectiveRange;
    [SerializeField] GameObject parent = null;
    
    void Start()
    {
        effectiveRange = GetComponent<Renderer>().bounds.extents.magnitude;
    }
    
    private void OnTriggerStay(Collider collider)
    {
        if(collider.TryGetComponent(out IPushable pushing))
        {
            
            Vector3 dir =  transform.position - collider.transform.position;
            
            if(dir.magnitude > 0.5f)
            {
                float iDist = (dir.magnitude < effectiveRange) ? (effectiveRange - dir.magnitude)/effectiveRange : 0.1f;
                
                iDist *= iDist;
                iDist *= iDist;
                
                dir.Normalize();
                pushing.Push(dir * (Time.fixedDeltaTime * blackHoleStrength) * iDist);
            }
        }
    }
    
    //private void OnCollisionStay(Collision c)
    //{
    //    if(parent == null || c.transform != parent.transform)
    //    {
    //        Debug.Log(c.gameObject.name);
    //        if(c.gameObject.TryGetComponent(out Rigidbody rb))
    //        {
    //            Debug.Log(c.gameObject.name);
    //            GameObject other = c.gameObject;
    //            Vector3 dir =  transform.position - other.transform.position;
    //            
    //            if(dir.magnitude > 0.5f)
    //            {
    //                dir.Normalize();
    //                other.GetComponent<Rigidbody>().AddForce(dir * Time.deltaTime * blackHoleStrength);
    //            }
    //        }
    //    }
    //}
}
