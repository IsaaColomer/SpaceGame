using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private Rigidbody rb;
    private GameManager gameManager;
    
    private bool isPressingIneractKey = false;
    private bool isInTrigger = false;
    public float explosionShelfRadius;
    public float explosionForce;
    public float cameraShakeMult;
    [SerializeField] private GameObject tmp;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        isPressingIneractKey = false;
        isInTrigger = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(gameManager.interactKey))
        {
            isPressingIneractKey=true;
        }
        else if(Input.GetKeyUp(gameManager.interactKey))
        {
            isPressingIneractKey = false;
        }

        if(isPressingIneractKey && isInTrigger)
        {
            gameManager.timeToDestroyProduct -= Time.deltaTime;
            if(gameManager.timeToDestroyProduct < 0f)
            {
                if(tmp != null)
                {
                    gameManager.timeToDestroyProduct = gameManager.resetTimeToDestroyProduct;
                    gameManager.ClearDestroyProductFromAllLists(tmp);
                    Destroy(tmp);
                }
            }
        }
        else
        {
            gameManager.timeToDestroyProduct = gameManager.resetTimeToDestroyProduct;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NPC") && rb.velocity.magnitude > collision.gameObject.GetComponent<NPCManager>().velToKill)
        {
            if(collision.gameObject.GetComponent<NPCManager>())
                collision.gameObject.GetComponent<NPCManager>().KillNPC();
        }        
        else
        {
            if(collision.gameObject.CompareTag("Shelf"))
            {
                if (rb.velocity.magnitude > 1f)
                    gameManager.ShakeCamera(cameraShakeMult*rb.velocity.magnitude, .5f);

                Collider[] coll = Physics.OverlapSphere(collision.contacts[0].point, explosionShelfRadius);
                foreach(Collider col in coll)
                {
                    Rigidbody r = col.GetComponent<Rigidbody>();
                    if(r != null)
                    {
                        r.AddExplosionForce(explosionForce*rb.velocity.magnitude, collision.contacts[0].point, explosionShelfRadius);
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CollectItemHere"))
        {
            if (gameManager.GetRelatedProduct(other.transform.parent) != null)
            {
                tmp = gameManager.GetRelatedProduct(other.transform.parent);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("CollectItemHere"))
        {
            isInTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CollectItemHere"))
        {
            isInTrigger = false;
            gameManager.timeToDestroyProduct = gameManager.resetTimeToDestroyProduct;
            tmp = null;
        }
    }
}
