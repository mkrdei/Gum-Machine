using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class GumSpawner : Singleton<GumSpawner>
{
    private Transform[] spawners;
    private bool active;
    [SerializeField]
    private float spawnRate = 20;
    [SerializeField]
    private Vector3 spawnOffset;
    [SerializeField]
    private Vector2 sprayRange = new Vector2(-0.1f,0.1f);
    [SerializeField]
    private float projectileSpeed=500;
    [SerializeField]
    private GameObject gum;
    private GameObject gums;
    private Quaternion downRotation;
    private GameObject instantiatedGum;
    private float timeStamp;
    private GameObject currentPlatform;
    // Start is called before the first frame update
    void Start()
    {
        active = true;
        SetSpawners();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButton(0) && active)
        {
            if(timeStamp <= Time.time)
                foreach(Transform spawner in spawners)
                {
                    LaunchInDirection(spawner); 
                    AudioManager.Instance.PlayAudioOneShot("Gum Spawn",0.3f,1f);
                } 
        }
    }

    private void LaunchInDirection(Transform spawner)
    {
        timeStamp = Time.time + 1/spawnRate;
        instantiatedGum = Instantiate(gum,spawner.position + spawnOffset,downRotation,gums.transform);
        Rigidbody gumRb = instantiatedGum.GetComponent<Rigidbody>();
        Vector3 forceDirection = new Vector3(-spawner.up.x + Random.Range(sprayRange.x,sprayRange.y),-spawner.up.y,-spawner.up.z);
        gumRb.AddForce(forceDirection*projectileSpeed);
    }


    public void SetSpawners()
    {
        Transform spawnersParent = PlatformManager.Instance.GetCurrentPlatform().transform.Find("Spawners");
        spawners = new Transform[spawnersParent.childCount];
        for(int i = 0; spawnersParent.childCount>i;i++)
        {
            spawners[i] = spawnersParent.GetChild(i);
        }
        gums = new GameObject("Gums");
        gums.transform.parent = PlatformManager.Instance.GetCurrentPlatform().transform;
        downRotation = Quaternion.LookRotation(transform.position,Vector3.forward);
        timeStamp = Time.time;
        spawnOffset = transform.TransformVector(spawnOffset);
    }

    public void PauseSpawners()
    {
        active = false;
    }

    public void ActivateSpawners()
    {
        active = true;
    }
    public bool SpawnerActive()
    {
        return active;
    }
}
