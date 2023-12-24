using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _UiFeedback : MonoBehaviour
{
    private Image circle;
    private void Awake()
    {
        circle = GameObject.Find("CircleFeedback").GetComponent<Image>();
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(CircleAnimation(2));
        }
    }
    public IEnumerator CircleAnimation(float time)
    {
        float tmpTime = time;
        while (tmpTime > 0)
        {
            Remap(circle.fillAmount, time-=Time.deltaTime, 0, 1, 0);
        }
        yield return new WaitForSeconds(time);
    }
    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
