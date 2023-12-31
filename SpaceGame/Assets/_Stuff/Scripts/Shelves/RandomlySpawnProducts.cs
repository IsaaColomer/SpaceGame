using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomlySpawnProducts : MonoBehaviour
{
    private List<GameObject> possibleSpawns = new List<GameObject>();
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        foreach (Transform p in transform)
        {
            if(p.CompareTag("SpawnPosition"))
            {
                possibleSpawns.Add(p.gameObject);
            }
        }
        Debug.Log("Total ammount of possible spawns: " + possibleSpawns.Count);

        for(int  i = 0; i < possibleSpawns.Count; ++i)
        {
            if(Random.Range(0,2) == 1)
            {
                Instantiate(gameManager.products[Random.Range(0, gameManager.products.Count)], possibleSpawns[i].transform.position, Quaternion.identity, possibleSpawns[i].transform);
            }
        }
    }
}
