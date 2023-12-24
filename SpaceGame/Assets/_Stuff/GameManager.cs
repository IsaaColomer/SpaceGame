using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // TEMPORARLY
    public GameObject uiCircle;
    private void Awake()
    {
        uiCircle = GameObject.Find("CircleFeedback");
        uiCircle.SetActive(false);
    }
    public void ActiveUiCircle()
    {
        uiCircle.SetActive(true);
    }
}
