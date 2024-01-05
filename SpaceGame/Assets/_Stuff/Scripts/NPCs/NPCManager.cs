using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using System.Data;
public class NPCManager : MonoBehaviour
{
    private Animator anim;
    public ParticleSystem killFx;
    public float velToKill = .25f;
    // only for killing Animation
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    private bool shouldMove;
    private NavMeshAgent agent;
    public float walkDistance;
    private GameManager gameManager;
    private bool updateState;
    public bool doMove; // this for inspector and to do or not all calculus and stuff
    // die
    // moving
    // shouldMove -> OnlyForStart
    // velocity
    // isSilly
    private void Awake()
    {
        anim = GetComponent<Animator>();
        virtualCamera = GameObject.Find("PlayerVirtualCamera").GetComponent<CinemachineVirtualCamera>();
        perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        agent = GetComponent<NavMeshAgent>();
        gameManager = FindObjectOfType<GameManager>();
        updateState = true;
    }
    private void Start()
    {
        anim.SetBool("doMove", doMove);
        if(!shouldMove)
        {
            walkDistance = 0f;
            anim.SetBool("shouldMove", false);
        }
        else
        {
            anim.SetBool("shouldMove", true);
        }
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
    private void Update()
    {
        if(agent.isOnNavMesh)
        {
            if(!agent.hasPath && updateState)
            {
                if(doMove)
                    StartCoroutine(WaitAndMove());
            }
        }
        if(shouldMove)
        {
            anim.SetFloat("velocity", anim.velocity.magnitude);
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
    private void ResetIntensity()
    {
        perlinNoise.m_AmplitudeGain = 0f;
    }
    #endregion
    public void DestroyNPC()
    {
        Destroy(this.gameObject);
    }
    public void AiMoveToDestination()
    {
        if (shouldMove)
        {
            anim.SetBool("moving", true);
            anim.SetBool("shouldMove", true);
            updateState = true;
            List<Transform> tmpList = new List<Transform>();
            tmpList = gameManager.GetAllSelectedItemCollect();
            int rnd = Random.Range(0, tmpList.Count-1);
            GameObject go = tmpList[rnd].gameObject;
            agent.SetDestination(go.transform.position);
        }     
    }
    public IEnumerator WaitAndMove()
    {
        shouldMove = true;
        anim.SetBool("moving", false);
        anim.SetBool("shouldMove", false);
        anim.SetBool("isSilly", false);
        updateState = false;
        yield return new WaitForSeconds(Random.Range(.5f, 5f));
        AiMoveToDestination();
        shouldMove = true;
        anim.SetBool("isSilly", RandomBoolean());
        anim.SetBool("moving", true);
        anim.SetBool("shouldMove", true);
        updateState = true;
    }
    public bool RandomBoolean()
    {
        if(Random.Range(0, 2) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
