using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public float length;
    [SerializeField] RaycastHit hit;

    private Camera cam;

    private void FixedUpdate()
    {
        
    }

    void DoRaycast()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, length))
        {
            if(Input.GetMouseButtonDown(0))
            {
                //
            }
        }
    }
}
