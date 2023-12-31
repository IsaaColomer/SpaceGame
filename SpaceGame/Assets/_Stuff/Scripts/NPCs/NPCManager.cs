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

    //

    public float minDistanceForArguing = 2f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        virtualCamera = GameObject.Find("PlayerVirtualCamera").GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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
        Debug.Log("Playing kill particle");
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
        List<GameObject> go = GameObject.FindGameObjectsWithTag("NPC").ToList();
        foreach (GameObject tmp in go)
        {
            if(tmp != this.gameObject)
            {
                if(Vector3.Distance(this.gameObject.transform.position, tmp.transform.position) < minDistanceForArguing)
                {
                    anim.SetBool("isSomeOneClose", true);
                    transform.LookAt(tmp.transform.position);
                }
                else
                {
                    anim.SetBool("isSomeOneClose", false);
                }
            }
        }
    }


    private void Update()
    {
        List<GameObject> go = GameObject.FindGameObjectsWithTag("NPC").ToList();
        foreach (GameObject tmp in go)
        {
            if (tmp != this.gameObject)
            {
                if(Vector3.Distance(this.gameObject.transform.position, tmp.transform.position) < minDistanceForArguing)
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
