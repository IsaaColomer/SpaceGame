using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductManager : MonoBehaviour
{
    private Outline outLine;
    private Material _mat02;
    private Material _mat03;
    private MeshRenderer mr;
    public Material transparent;
    private void Awake()
    {
        outLine = GetComponent<Outline>();
        DisableOutline();
        mr = GetComponent<MeshRenderer>();
        _mat02 = mr.materials[1];
        _mat03 = mr.materials[2];
    }
    public void DisableOutline()
    {
        outLine.enabled = false;
    }
    public void EnableOutline()
    {
        outLine.enabled = true;
    }
}
