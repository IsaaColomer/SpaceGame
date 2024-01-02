using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource fxMaster;
    private Rigidbody rb;
    [SerializeField] private bool enableSound;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enableSound = true;
    }

    private void Update()
    {
        if(Mathf.Abs(rb.velocity.magnitude) > 0f)
        {
            if(enableSound)
            {
                EnableMovingSound();
            }
        }
        if(Mathf.Abs(rb.velocity.magnitude) <= 0.2f)
        {
            enableSound = true;
            fxMaster.enabled = false;
        }
    }
    public void EnableMovingSound()
    {
        fxMaster.enabled = true;
        enableSound = false;
    }
}
