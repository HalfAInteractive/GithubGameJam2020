using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarField : MonoBehaviour
{
  
    public GameObject star;
    public int count = 0;
    public float minX;
    public float maxX;
    
    public float minZ;
    public float maxZ;
    
    public float minSize;
    public float maxSize;
    
    public Color StartColor;
    public Color EndColor;
    
    // Start is called before the first frame update
    void Start()
    {
      for(int i = 0; i < count; i++)
      {
        float X = Random.Range(minX, maxX);
        float Z = Random.Range(minZ, maxZ);
        float Size = Random.Range(minSize, maxSize);
        GameObject g = GameObject.Instantiate(star, new Vector3(X, -3, Z), Quaternion.identity);
        g.hideFlags = HideFlags.HideInHierarchy;
        g.transform.localScale = new Vector3(Size, Size, Size);
        
        Material matt = new Material(g.GetComponent<Renderer>().material.shader);
        
        float r = Random.Range(0.0f, 1.0f);
        
        matt.color = Easing.MixHSV(StartColor, EndColor, r);
        
        g.GetComponent<Renderer>().material = matt;
        g.GetComponent<Hover>().targetPos = g.transform.position + new Vector3(0.0f, -0.1f, 0.0f);
      }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
