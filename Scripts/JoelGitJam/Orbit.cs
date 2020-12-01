using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject orbiting;
    public float minSpeed;
    public float maxSpeed;
    public float radius;
    public float magnitude = 1; // used to reverse direction.
    Quaternion randomRotation;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false; // disables the updates. micro-optimization if updating isn't needed yet
    }

    public void Init(GameObject parent)
    {
        orbiting = parent;

        randomRotation = Random.rotation;
        Vector3 temp = randomRotation * new Vector3(1, 0, 0);
        temp.Normalize();
        transform.localPosition = temp * radius;

        enabled = true; // re-enables the update
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (orbiting != null)
        { 
            if (magnitude < -3f)
            {
                magnitude = -3f;
            }
            if (magnitude > 3f)
            {
                magnitude = 3f;
            }

            Vector3 dir = orbiting.transform.position - transform.position;
            Vector3 ortho = randomRotation * new Vector3(-dir.z, dir.y, dir.x);
            ortho.Normalize();

            float speed = Easing.Mix(maxSpeed, minSpeed, (dir.magnitude - 0.3f) / 20f);

            transform.position += ortho * (Time.deltaTime * speed * magnitude);

            //Preserve Radius
            Vector3 currDir = transform.position - orbiting.transform.position;
            if (currDir.magnitude != radius)
            {
                currDir.Normalize();
                transform.position = orbiting.transform.position + currDir * radius;
            }
        }
    }
    
    public void SetOrbitTarget(GameObject g)
    {
      orbiting = g;
    }
}
