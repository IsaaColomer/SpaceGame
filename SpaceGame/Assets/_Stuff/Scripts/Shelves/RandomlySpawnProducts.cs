using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomlySpawnProducts : MonoBehaviour
{
    public List<GameObject> products = new List<GameObject>();
    private List<GameObject> possibleSpawns = new List<GameObject>();

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
                Instantiate(products[Random.Range(0, products.Count)], possibleSpawns[i].transform.position, Quaternion.identity, possibleSpawns[i].transform);
            }
        }
    }
}
