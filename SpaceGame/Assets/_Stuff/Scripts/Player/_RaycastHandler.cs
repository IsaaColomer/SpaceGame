using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _RaycastHandler : MonoBehaviour
{
    public float rayLength = 500f;
    private RaycastHit hit = new RaycastHit();
    private Camera cam;
    private PlayerManager pM;
    private void Awake()
    {
        cam = Camera.main;
        pM = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        MyRaycast(cam.transform.position, cam.transform.forward, hit, rayLength);
    }

    public void MyRaycast(Vector3 s, Vector3 d, RaycastHit h, float l)
    {
        if(Physics.Raycast(s, d, out hit, l))
        {
            if(hit.transform.gameObject != null)
            {
                if (pM.CheckIfObjectCanBeInteracted(hit.transform.gameObject))
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                    }                    
                }
            }
        }
    }
}
