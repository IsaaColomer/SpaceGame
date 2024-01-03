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
    private GameManager gameManager;
    private CustomEventHandler _events;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GameObject.Find("PlayerVirtualCamera").GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        _events = FindObjectOfType<CustomEventHandler>();
        isAccelerating = false;
    }
    private void Start()
    {
        ResetVirtualCameraProperties();
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
        if (isAccelerating && rb.velocity.magnitude > 1.5f && Input.GetAxis("Vertical") == 1)
        {
            //anim.SetBool("Accelerate", true);
            _events.onAccelerate?.Invoke();
        }
        else
        {
            //anim.SetBool("Accelerate", false);
        }
    }
    public void ResetVirtualCameraProperties()
    {
        gameManager.ResetIntensity();
    }
}