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
    private void Awake()
    {
        anim = GetComponent<Animator>();
        virtualCamera = GameObject.Find("PlayerVirtualCamera").GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Start()
    {
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
}
