using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _UiFeedback : MonoBehaviour
{
    private Image circle;
    private bool docircleAnimaiton;
    [HideInInspector] public float time = 3f;
    private Raycast a;
    private float time_;
    private void Awake()
    {
        circle = GameObject.Find("CircleFeedback").GetComponent<Image>();
        a = FindObjectOfType<Raycast>();
    }
    private void Start()
    {
        // bools
        circle.enabled = true;
        docircleAnimaiton = true;

        // floats
        time_ = time;
    }
    private void Update()
    {
        if(docircleAnimaiton)
        {
            CircleAnimation();
        }
    }
    public void CircleAnimation()
    {
        time -= Time.deltaTime;
        if (time > 0 && !a.pressing)
        {
            float d = Remap(time, 0, time_, 0, 1);
            Debug.Log("This is d value " + time + "this is the map functino value: "+ d);
            circle.fillAmount = d;
        }
        else
        {
            circle.fillAmount = 0;
            docircleAnimaiton = false;
            circle.enabled = false;
            this.gameObject.SetActive(false);
        }
        
    }
    public static float Remap(float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
}
