using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamGravity : MonoBehaviour
{
    [SerializeField] float streamStrength = 0;

    private void OnTriggerStay(Collider col)
    {
        if(col.TryGetComponent(out IPushable pushing))
        {
            GameObject other = col.gameObject;
            CapsuleCollider c = GetComponent<CapsuleCollider>();
            
            Vector3 capsuleDir = c.transform.rotation * new Vector3(0, 1, 0);
            capsuleDir.Normalize();
            
            Vector3 capsuleBase = transform.position + capsuleDir * c.height/2;
            
            Vector3 relativePosVector = capsuleBase - other.transform.position;
            
            float overlap = Vector3.Dot(-capsuleDir, -relativePosVector);
            
            //Debug.Log(overlap);
            
            Vector3 target = capsuleBase - capsuleDir * overlap;
            
            Vector3 dir = target - other.transform.position;
            
            //Debug.DrawRay(other.transform.position, dir, Color.white, 0.3f);
            //Debug.DrawRay(capsuleBase, -capsuleDir * overlap, Color.white, 0.3f);
            
            if(dir.magnitude > 0.5f)
            {
                // just letting you know OnTriggerStay runs on the physics engine
                // so you'll have to move any rigidbodies things by Time.fixedDeltaTime.
                // I also created an attractor class that you can look into.
                dir.Normalize();
                pushing.Push(dir * Time.deltaTime * streamStrength);
            }
        }
    }
}
