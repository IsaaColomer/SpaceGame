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
    public List<GameObject> products = new List<GameObject>();
    public List<GameObject> allShelves = new List<GameObject>();
    public List<GameObject> allItemCollect = new List<GameObject>();
    public List<GameObject> selectedProductsToCollect = new List<GameObject>();
    [SerializeField] private List<GameObject> selectedShelves = new List<GameObject>();
    public List<ShelfAndProduct> shelfAndProducts = new List<ShelfAndProduct>();
    [Header("DEBUG ONLY")]
    public List<GameObject> shelves = new List<GameObject>();
    public List<GameObject> product = new List<GameObject>();

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
    }

    private void Start()
    {
        GenerateRandomCollectLocation();
        StartCoroutine(WaitAndGetProducts());
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
                        Debug.Log(item2.gameObject.name);
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
}
