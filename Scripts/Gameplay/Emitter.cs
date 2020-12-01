using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO - added notes.
public class Emitter : MonoBehaviour
{
    [SerializeField] GameObject emissionObject;
    [SerializeField] float radius = 1;
    [SerializeField] float count = 1;
    [SerializeField] bool lockRotation = false;
    [SerializeField] bool prebuilt = false;
    [SerializeField] bool onStart = false;
    [SerializeField] bool onlyEdges = false;
    [SerializeField] bool asChildren = false;
    public bool isEnabled = true;

    private void Start()
    {
        if(onStart)
        {
            Emit();
        }
    }

    public List<GameObject> Emit()
    {
        if (isEnabled)
        {
            List<GameObject> temp = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                float X = Random.Range(-1f, 1f) * radius;
                float Y = Random.Range(-1f, 1f) * radius;
                float Z = Random.Range(-1f, 1f) * radius;

                Vector3 pos = new Vector3(X, Y, Z); // should use Random.insideUnitSphere

                if(onlyEdges)
                {
                    pos.Normalize();
                    pos *= radius;
                }// should use Random.onUnitSphere

                Quaternion randRotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)); // should use Random.rotation
                if (lockRotation)
                    randRotation = Quaternion.identity;
                if (prebuilt)
                {
                    GameObject g = GameObject.Instantiate(emissionObject, transform.position + pos, randRotation);
                    g.transform.parent = transform;
                    foreach(Transform child in g.transform)
                    {
                        Debug.Log(child.gameObject.name);

                        temp.Add(child.gameObject);
                    }
                    //GameObject.Destroy(g);
                }
                else
                {
                    GameObject g = GameObject.Instantiate(emissionObject, transform.position + pos, randRotation);
                    if (asChildren)
                    {
                        g.transform.parent = transform;
                    }
                    temp.Add(g);
                }
            }
            transform.DetachChildren();
            return temp;
        }
        return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
