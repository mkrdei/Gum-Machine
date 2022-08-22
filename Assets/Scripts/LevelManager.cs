using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
public class LevelManager : Singleton<LevelManager>
{

    public int fallLimit = 3;
    private bool passedLevel = false;
    public GameObject[] platforms;
    private GameObject[] platformPrefabs;
    private GameObject currentPlatform, currentPlatformCopy;
    public PlatformManager[] platformManagers;
    [SerializeField]
    protected int currentPlatformIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject blackScreen = GameObject.Find("BlackScreen");
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
            FallCounter.Instance.ResetFallCount();
        }
        
        if(platformManagers[currentPlatformIndex].platformPassed && !UIManager.Instance.fadeIn && !UIManager.Instance.fadeOut)
        {
            UIManager.Instance.ResetStrikes();
            if(platforms.Length==currentPlatformIndex+1 && !passedLevel)
            {
                UIManager.Instance.SetPlatformIconColor(currentPlatformIndex,"DarkGum");
                PassLevel();
                Debug.Log("Passed level");
            }
            else if(platforms.Length>currentPlatformIndex+1)
            {
                NextPlatform();
                UIManager.Instance.SetPlatformIconColor(currentPlatformIndex-1,"DarkGum");
                UIManager.Instance.SetPlatformIconColor(currentPlatformIndex,"LightGum");
                Debug.Log("Next platform.");
            }
            
        }
        else if(FallCounter.Instance.GetFallCount()>=fallLimit)
        {
            if(platforms.Length>currentPlatformIndex)
            {
                Debug.Log("Fall limit reached.");
                UIManager.Instance.GetSlider().SetActive(false);
                FallCounter.Instance.counting = false;
                UIManager.Instance.fadeIn = true;
                if(UIManager.Instance.BlackScreenEnabled())
                {
                    ResetCurrentPlatform();
                    UIManager.Instance.ResetStrikes();
                    Debug.Log("Resetting current platform.");
                }
            }
        }
        
    }
    public int GetPlatformSize()
    {
        return platforms.Length;
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
        FallCounter.Instance.counting = true;
        Destroy(platforms[currentPlatformIndex-1]);
        UIManager.Instance.ResetStrikes();
    }
    private void NextPlatform()
    {
        Destroy(currentPlatformCopy);
        currentPlatformIndex += 1;
        FallCounter.Instance.counting = false;
        Invoke("DestroyOldPlatform",CameraManager.Instance.positioningTime*0.99f);
        currentPlatform = platforms[currentPlatformIndex];
        CreatePlatformCopy();
        GumSpawner.Instance.SetSpawners();
        FallCounter.Instance.ResetFallCount();
    }

    private void ResetCurrentPlatform()
    {
        GameObject deletedPlatform = currentPlatform.transform.parent.Find(currentPlatform.transform.name).gameObject;
        platforms[currentPlatformIndex] = currentPlatformCopy;
        currentPlatform = platforms[currentPlatformIndex];
        // Set PlatformManager
        platformManagers[currentPlatformIndex] = currentPlatform.GetComponent<PlatformManager>();
        GumSpawner.Instance.SetSpawners();
        currentPlatform.transform.SetSiblingIndex(deletedPlatform.transform.GetSiblingIndex());
        Destroy(deletedPlatform);
        currentPlatform.SetActive(true);
        CreatePlatformCopy();
        FallCounter.Instance.ResetFallCount();
        FallCounter.Instance.counting = true;
        UIManager.Instance.fadeOut = true;
        platformManagers[currentPlatformIndex].platformPassed = false;
    }
    private void PassLevel()
    {
        CameraManager.Instance.SprayConfettis();
        passedLevel = true;
    }
}
    


