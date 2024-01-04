using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class ShelfAndProduct
{
    public string name = "_";
    public GameObject shelf;
    public GameObject product;
    public GameObject ground;
}
public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> products = new List<GameObject>();
    private List<GameObject> allShelves = new List<GameObject>();
    private List<GameObject> allItemCollect = new List<GameObject>();
    private List<GameObject> selectedProductsToCollect = new List<GameObject>();
    private List<GameObject> selectedShelves = new List<GameObject>();
    [HideInInspector] public List<ShelfAndProduct> shelfAndProducts = new List<ShelfAndProduct>();
    private Canvas canvas;
    private Transform bg;

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

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;

    public GameObject uiInstantiateProduct;
    private List<GameObject> uiProductsList = new List<GameObject>();
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
        virtualCamera = GameObject.Find("PlayerVirtualCamera").GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        canvas = FindObjectOfType<Canvas>();
        bg = GameObject.Find("BgPostit").GetComponent<Transform>();
    }

    private void Start()
    {
        GenerateRandomCollectLocation();
        StartCoroutine(WaitAndGetProducts());
        StartCoroutine(WaitAndFillUI());
        

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
                }
                else
                {
                    if (item.GetComponent<Outline>().enabled)
                    {
                        item.GetComponent<Outline>().enabled = false;
                    }
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

            foreach (Transform item in shelves[l].transform)
            {
                if(item.gameObject.name == "CollectItemHere")
                {
                    tmp.ground = item.gameObject;
                }
            }

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

        return ret;
    }

    public void ClearDestroyProductFromAllLists(GameObject g)
    {
        CheckAndDeleteUiProduct(g);
        for (int i = 0; i < shelfAndProducts.Count; ++i)
        {
            if (shelfAndProducts[i].product == g)
            {
                shelfAndProducts[i].product = null;
                shelfAndProducts[i].shelf = null;
                Destroy(shelfAndProducts[i].ground.gameObject);
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
    #region Shake Camera
    public void ShakeCamera(float intensity, float shakeTime)
    {
        perlinNoise.m_AmplitudeGain = intensity;
        StartCoroutine(WaitTime(shakeTime));
    }
    IEnumerator WaitTime(float shakeTime)
    {
        yield return new WaitForSeconds(shakeTime);
        ResetIntensity();
    }
    public void ResetIntensity()
    {
        perlinNoise.m_AmplitudeGain = 0f;
        perlinNoise.m_FrequencyGain = 0.16f;
    }
    #endregion

    public IEnumerator WaitAndFillUI()
    {
        yield return new WaitForSeconds(2f);
        GenerateUiProducts();
    }

    public void GenerateUiProducts()
    {
        
        int h = 0;
        foreach (GameObject g in selectedProductsToCollect)
        {
            
            GameObject go = Instantiate(uiInstantiateProduct, Vector3.zero, Quaternion.identity, bg);
            //Vector3 asd = new Vector3(0f, h * 2.25f, 0f);
            go.GetComponent<Transform>().position = new Vector3(0, 0f, 0f);
            go.GetComponent<Transform>().localPosition = new Vector3(0, 0f, 0f);
            go.GetComponent<Transform>().localScale = Vector3.one;
            go.GetComponentInChildren<Image>().sprite = g.GetComponent<ProductManager>().uiSprite;
            go.GetComponentInChildren<TextMeshProUGUI>().text =" ";
            go.GetComponent<UiProduct>().relatedGo = g;
            uiProductsList.Add(go);
            h++;
        }
    }
    public void CheckAndDeleteUiProduct(GameObject go)
    {
        foreach (GameObject g in uiProductsList)
        {
            if(g.GetComponent<UiProduct>().relatedGo == go)
            {
                GameObject pendingToDelete = g;
                
                Destroy(g);
                uiProductsList.Remove(pendingToDelete);
                break;
            }
        }
    }

    // NPCS AI
    public List<Transform> GetAllSelectedItemCollect()
    {
        List<Transform> list = new List<Transform>();
        foreach(GameObject g in selectedProductsToCollect)
        {
            if(g.activeSelf)
            {
                list.Add(g.transform);
            }
        }
        return list;
    }
}
