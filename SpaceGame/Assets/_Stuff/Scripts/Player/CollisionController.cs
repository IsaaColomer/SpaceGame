using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        bool a = collision.gameObject.CompareTag("NPC");
        bool b = false;
        if (collision.gameObject.GetComponent<NPCManager>())
            b = rb.velocity.magnitude > collision.gameObject.GetComponent<NPCManager>().velToKill;
        bool c = collision.gameObject.GetComponent<NPCManager>();


        Debug.Log(a + " " + b + " " + c);


        if (collision.gameObject.CompareTag("NPC") && rb.velocity.magnitude > collision.gameObject.GetComponent<NPCManager>().velToKill)
        {
            if(collision.gameObject.GetComponent<NPCManager>())
                collision.gameObject.GetComponent<NPCManager>().KillNPC();
        }        
    }
}
