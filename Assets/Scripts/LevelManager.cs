using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
public class LevelManager : Singleton<LevelManager>
{

    public int fallLimit = 3;
    public GameObject[] platforms;
    private GameObject[] platformPrefabs;
    private GameObject currentPlatform, currentPlatformCopy;
    public PlatformManager[] platformManagers;
    [SerializeField]
    protected int currentPlatformIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get platform managers.
        platformManagers = new PlatformManager[platforms.Length];
        platformPrefabs = new GameObject[platforms.Length];
        for(int i = 0; i < platforms.Length; i++)
        {   
            platformPrefabs[i] = platforms[i];
            platformManagers[i] = platforms[i].GetComponent<PlatformManager>();
        }
        // First platform is the current platform.
        currentPlatformIndex = 0;
        currentPlatform = platforms[currentPlatformIndex];
        //GumSpawner.Instance.SetSpawners();
        CreatePlatformCopy();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(CameraManager.Instance.positioning)
        {
            FallCounter.Instance.counting = false;
        }else
        {
            FallCounter.Instance.counting = true;
        }
        if(platforms.Length>currentPlatformIndex+1)
        {
            if(platformManagers[currentPlatformIndex].platformPassed && !CameraManager.Instance.fadeInAndOut)
            {
                NextLevel();
            }
            else if(FallCounter.Instance.GetFallCount()>=fallLimit)
            {
                UIManager.Instance.GetSlider().SetActive(false);
                FallCounter.Instance.counting = false;
                CameraManager.Instance.fadeInAndOut = true;
                if(CameraManager.Instance.blackScreenEnabled)
                    ResetCurrentPlatform();
            }   
        }
        
    }
    public int GetCurrentPlatformIndex()
    {
        return currentPlatformIndex;
    }
    public GameObject GetCurrentPlatform()
    {
        return platforms[currentPlatformIndex].gameObject;
    }
    private void CreatePlatformCopy()
    {
        currentPlatformCopy = Instantiate(currentPlatform,currentPlatform.transform.position,Quaternion.identity,currentPlatform.transform.parent);
        currentPlatformCopy.SetActive(false);
        currentPlatformCopy.transform.name = currentPlatform.transform.name;
    }
    private void DestroyOldPlatform()
    {
        Debug.Log("Destroy old platform.");
        Destroy(platforms[currentPlatformIndex-1]);
    }
    private void NextLevel()
    {
        Debug.Log("Next level.");
        Destroy(currentPlatformCopy);
        currentPlatformIndex += 1;
        Invoke("DestroyOldPlatform",CameraManager.Instance.positioningTime*0.9f);
        currentPlatform = platforms[currentPlatformIndex];
        CreatePlatformCopy();
        //GumSpawner.Instance.SetSpawners();
        FallCounter.Instance.ResetFallCount();
    }

    private void ResetCurrentPlatform()
    {
        Debug.Log("Resetting current platform.");
        GameObject deletedPlatform = currentPlatform.transform.parent.Find(currentPlatform.transform.name).gameObject;
        platforms[currentPlatformIndex] = currentPlatformCopy;
        currentPlatform = platforms[currentPlatformIndex];
        // Set PlatformManager
        platformManagers[currentPlatformIndex] = currentPlatform.GetComponent<PlatformManager>();
        //GumSpawner.Instance.SetSpawners();
        currentPlatform.transform.SetSiblingIndex(deletedPlatform.transform.GetSiblingIndex());
        Destroy(deletedPlatform);
        currentPlatform.SetActive(true);
        CreatePlatformCopy();
        FallCounter.Instance.ResetFallCount();
        FallCounter.Instance.counting = true;
        platformManagers[currentPlatformIndex].platformPassed = false;
    }
}
    


