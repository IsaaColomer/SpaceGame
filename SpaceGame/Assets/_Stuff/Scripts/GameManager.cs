using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class ShelfAndProduct
{
    public string name = "_";
    public GameObject shelf;
    public GameObject product;
}
public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> products = new List<GameObject>();
    private List<GameObject> allShelves = new List<GameObject>();
    private List<GameObject> allItemCollect = new List<GameObject>();
    private List<GameObject> selectedProductsToCollect = new List<GameObject>();
    private List<GameObject> selectedShelves = new List<GameObject>();
    private List<ShelfAndProduct> shelfAndProducts = new List<ShelfAndProduct>();
    [Header("PRODUCTS")]
    public float distanceToActivateOutline = 5f;
    [Header("DEBUG ONLY")]
    private List<GameObject> shelves = new List<GameObject>();
    private List<GameObject> product = new List<GameObject>();

    [Header("Controlls")]
    public KeyCode interactKey;

    [Header("GAMEPLAY")]
    public float timeToDestroyProduct = 1f;
    [HideInInspector] public float resetTimeToDestroyProduct;

    private GameObject player;

    [HideInInspector] public int maxItemsToCollect = 8;
    private int itemCounterForSpawn = 0;
    [HideInInspector] public int collectedItemsTrueAmmount = 0;
    [HideInInspector] public int collectedItemsAmmount = 0;
    private void Awake()
    {
        allShelves = GameObject.FindGameObjectsWithTag("Shelf").ToList();
        foreach (GameObject item in allShelves)
        {
            foreach (Transform shel in item.transform)
            {
                if(shel.CompareTag("CollectItemHere"))
                {
                    allItemCollect.Add(shel.gameObject);
                }
            }            
        }

        player = GameObject.Find("PlayerKart");
    }

    private void Start()
    {
        GenerateRandomCollectLocation();
        StartCoroutine(WaitAndGetProducts());

        resetTimeToDestroyProduct = timeToDestroyProduct;
    }

    public void GenerateRandomCollectLocation()
    {
        for(int i = 0; i < allItemCollect.Count; ++i)
        {
            if(itemCounterForSpawn < maxItemsToCollect)
            {
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    itemCounterForSpawn++;
                    selectedShelves.Add(allItemCollect[i].transform.parent.gameObject);                    
                }
                else
                {
                    allItemCollect[i].SetActive(false);
                }
            }
            else
            {
                allItemCollect[i].SetActive(false);
            }
        }
        collectedItemsTrueAmmount = itemCounterForSpawn;
    }
    IEnumerator WaitAndGetProducts()
    {
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < selectedShelves.Count; ++i)
        {
            GetProductsOnSelectedShelves(selectedShelves[i].transform);
        }
        CreateEachClasses();
    }
    public void GetProductsOnSelectedShelves(Transform s)
    {
        List<GameObject> allProductsinThisShelf = new List<GameObject>();

        foreach(Transform item in s)
        {
            if(item.GetComponentInChildren<Transform>() != null)
            {
                foreach (Transform item2 in item)
                {
                    if (item2.CompareTag("Product"))
                    {
                        allProductsinThisShelf.Add(item2.gameObject);
                    }
                }
            }           
        }
        for(int i = 0; i < allProductsinThisShelf.Count; ++i)
        {
            if(UnityEngine.Random.Range(0,5) == 0)
            {
                selectedProductsToCollect.Add(allProductsinThisShelf[i]);
                shelves.Add(s.gameObject);
                product.Add(allProductsinThisShelf[i]);

                break;
            }
        }
    }
    private void Update()
    {
        LookForCloseProducts();
    }
    public void LookForCloseProducts()
    {
        foreach (GameObject item in selectedProductsToCollect)
        {
            if(item != null)
            {
                if (Vector3.Distance(player.transform.position, item.transform.position) < distanceToActivateOutline)
                {
                    if (!item.GetComponent<Outline>().enabled)
                    {
                        item.GetComponent<Outline>().enabled = true;
                    }

                    Debug.DrawLine(player.transform.position, item.transform.position, Color.green);
                }
                else
                {
                    if (item.GetComponent<Outline>().enabled)
                    {
                        item.GetComponent<Outline>().enabled = false;
                    }

                    Debug.DrawLine(player.transform.position, item.transform.position, Color.red);
                }
            }                       
        }
    }

    public void CreateEachClasses()
    {
        for (int l = 0; l < shelves.Count; ++l)
        {
            ShelfAndProduct tmp = new ShelfAndProduct();
            tmp.shelf = shelves[l];
            tmp.product = product[l];
            tmp.name = shelves[l].name + product[l].name;

            if (tmp.name != "_")
                shelfAndProducts.Add(tmp);
        }
    }
    public GameObject GetRelatedProduct(Transform go)
    {
        GameObject ret = null;
        for(int i = 0; i < shelfAndProducts.Count; ++i)
        {
            if (shelfAndProducts[i].shelf == go.gameObject)
            {
                ret = shelfAndProducts[i].product;
            }
        }
        Debug.Log(ret.name);
        return ret;
    }

    public void ClearDestroyProductFromAllLists(GameObject g)
    {
        for(int i = 0; i < shelfAndProducts.Count; ++i)
        {
            if (shelfAndProducts[i].product == g)
            {
                shelfAndProducts[i].product = null;
            }
        }
        for(int j = 0;  j < selectedProductsToCollect.Count; ++j)
        {
            if (selectedProductsToCollect[j] == g)
            {
                selectedProductsToCollect.RemoveAt(j);
            }
        }
    }
}
