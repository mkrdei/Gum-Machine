using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
public class PlatformManager : Singleton<PlatformManager>
{
    [SerializeField]
    private int fallLimit = 3;
    private GameObject[] platforms;
    private GameObject[] platformPrefabs;
    private Platform[] platformScripts;
    private GameObject currentPlatform, currentPlatformCopy;
    
    private int currentPlatformIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        SetPlatforms();
    }

    // Update is called once per frame
    void Update()
    {
        if(CameraManager.Instance.IsPositioning())
        {
            FallCounter.Instance.ResetFallCount();
        }
        if(platformScripts[currentPlatformIndex].IsPassed() && !UIManager.Instance.IsFadingIn() && !UIManager.Instance.IsFadingOut())
        {
            UIManager.Instance.ResetStrikes();
            if(platforms.Length==currentPlatformIndex+1 && !LevelManager.Instance.LevelPassed())
            {
                UIManager.Instance.SetPlatformIconColor(currentPlatformIndex,"DarkGum");
                LevelManager.Instance.PassLevel();
                Debug.Log("Passed level");
            }
            else if(platforms.Length>currentPlatformIndex+1)
            {
                NextPlatform();
                Debug.Log("Next platform. " + currentPlatformIndex);
                UIManager.Instance.SetPlatformIconColor(currentPlatformIndex-1,"DarkGum");
                UIManager.Instance.SetPlatformIconColor(currentPlatformIndex,"LightGum");
                
            }
            
        }
        else if(FallCounter.Instance.GetFallCount()>=fallLimit)
        {
            if(platforms.Length>currentPlatformIndex)
            {
                Debug.Log("Fall limit reached.");
                UIManager.Instance.GetSlider().SetActive(false);
                FallCounter.Instance.Counting(false);
                UIManager.Instance.FadeIn(true);
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
        FallCounter.Instance.Counting(true);
        Destroy(platforms[currentPlatformIndex-1]);
        UIManager.Instance.ResetStrikes();
    }
    private void NextPlatform()
    {
        Destroy(currentPlatformCopy);
        currentPlatformIndex += 1;
        FallCounter.Instance.Counting(false);
        Invoke("DestroyOldPlatform",CameraManager.Instance.GetPositioningTime()*0.99f);
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
        platformScripts[currentPlatformIndex] = currentPlatform.GetComponent<Platform>();
        GumSpawner.Instance.SetSpawners();
        currentPlatform.transform.SetSiblingIndex(deletedPlatform.transform.GetSiblingIndex());
        Destroy(deletedPlatform);
        currentPlatform.SetActive(true);
        CreatePlatformCopy();
        FallCounter.Instance.ResetFallCount();
        FallCounter.Instance.Counting(true);
        UIManager.Instance.FadeOut(true);
        platformScripts[currentPlatformIndex].Passed(false);
    }
    public int GetFallLimit()
    {
        return fallLimit;
    }
    private void SetPlatforms()
    {
        platforms = new GameObject[transform.childCount];
        // Get platform managers.
        platformScripts = new Platform[platforms.Length];
        platformPrefabs = new GameObject[platforms.Length];
        for(int i = 0; i < transform.childCount; i++)
        {   
            platforms[i] = transform.GetChild(i).gameObject;
            platformPrefabs[i] = platforms[i];
            platformScripts[i] = platforms[i].GetComponent<Platform>();
        }
        // First platform is the current platform.
        currentPlatformIndex = 0;
        currentPlatform = platforms[currentPlatformIndex];
        CreatePlatformCopy();
    }
}
    


