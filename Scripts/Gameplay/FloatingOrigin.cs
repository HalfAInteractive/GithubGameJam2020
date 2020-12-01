using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingOrigin : MonoBehaviour
{    
    public static void Rebase(Vector3 newPosition)
    {
        // Rebasing has an odd interaction with the cinemachine cam.
        // I've tried removing all the damping and it helped some.
        // Let me know if you have any ideas on how to fix this.
        for(int i = 0; i < SceneManager.sceneCount; i++)
        {
            foreach(GameObject g in SceneManager.GetSceneAt(i).GetRootGameObjects())
            {
                g.transform.position -= newPosition;
            }
        }
    }
}
