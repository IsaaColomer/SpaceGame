using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerationFeedback : MonoBehaviour
{
    private Rigidbody rb;
    public bool isAccelerating;
    public KeyCode accKey;
    private Animator anim;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GameObject.Find("PlayerVirtualCamera").GetComponent<Animator>();
        isAccelerating = false;
    }
    private void Update()
    {
        if(Input.GetKeyDown(accKey))
        {
            isAccelerating = true;
        }
        else if(Input.GetKeyUp(accKey))
        {
            isAccelerating = false;
        }
        if (isAccelerating && rb.velocity.magnitude > 1f)
        {
            anim.SetBool("Accelerate", true);
            Debug.Log("Accelerating");
        }
        else
        {
            anim.SetBool("Accelerate", false);
            Debug.Log("Not Accelerating");
        }
    }
}
