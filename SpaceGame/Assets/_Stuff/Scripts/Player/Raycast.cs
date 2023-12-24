using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{
    public float length;
    [SerializeField] RaycastHit hit;

    private Camera cam;

    private bool pressing;
    private void Awake()
    {
        pressing = false;
    }

    private void Update()
    {
        Debug.Log(pressing);
    }

    void DoRaycast()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, length))
        {
            if(Input.GetMouseButtonDown(0))
            {
                pressing = true;
                
            }
            if(Input.GetMouseButtonUp(0))
            {
                pressing = false;
            }
        }
    }
}
