using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
public class NPCManager : MonoBehaviour
{
    private Animator anim;
    public ParticleSystem killFx;
    public float velToKill = .25f;
    // only for killing Animation
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;

    List<GameObject> go = new List<GameObject>();
    List<Transform> got = new List<Transform>();
    //

    public float minDistanceForArguing = 2f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        virtualCamera = GameObject.Find("PlayerVirtualCamera").GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        go = GameObject.FindGameObjectsWithTag("NPC").ToList();
        foreach (GameObject t in go)
        {
            got.Add(t.transform);
        }
    }
    private void Start()
    {
        DecideIfArgue();
        ResetIntensity();
    }
    public void KillNPC()
    {
        anim.SetBool("die", true);
    }
    public void PlayKillParticle()
    {
        killFx.Play();
        ShakeCamera(4f, .5f);
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
    private void ResetIntensity()
    {
        perlinNoise.m_AmplitudeGain = 0f;
    }
    #endregion
    public void DestroyNPC()
    {
        Destroy(this.gameObject);
    }
    public void DecideIfArgue()
    {
        foreach (GameObject tmp in go)
        {
            if(tmp != this.gameObject)
            {
                if((tmp.transform.position - this.gameObject.transform.position).magnitude < minDistanceForArguing)
                {
                    anim.SetBool("isSomeOneClose", true);
                    transform.LookAt(tmp.transform.position);
                }
            }
        }
    }
    
    Transform GetClosestNPC(List<Transform> enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in enemies)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }
    private void FixedUpdate()
    {
        DecideIfArgue();
    }

    private void Update()
    {
        List<GameObject> go = GameObject.FindGameObjectsWithTag("NPC").ToList();
        foreach (GameObject tmp in go)
        {
            if (tmp != this.gameObject)
            {
                if((tmp.transform.position - this.gameObject.transform.position).magnitude < minDistanceForArguing)
                {
                    Debug.DrawLine(this.gameObject.transform.position, tmp.transform.position, Color.green);
                }
                else
                {
                    Debug.DrawLine(this.gameObject.transform.position, tmp.transform.position, Color.red);
                }                    
            }
        }
    }
}
