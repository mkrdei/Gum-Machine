using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
public class LevelManager : Singleton<LevelManager>
{
    private SceneAsset[] levels;
    string highestLevelReached;
    public bool levelPassed = false;
    // Start is called before the first frame update
    void Start()
    {
        levels = Resources.LoadAll<SceneAsset>("Levels");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PassLevel()
    {
        int highestLevelReachedIndex = SceneManager.GetSceneByName(PlayerPrefs.GetString("highestLevelReached")).buildIndex;
        int reachedLevelIndex = SceneManager.GetActiveScene().buildIndex+1;
        if(highestLevelReachedIndex<reachedLevelIndex)
        {
            Debug.Log("New level record");
            highestLevelReached = SceneManager.GetSceneByBuildIndex(reachedLevelIndex).name;
            PlayerPrefs.SetString("highestLevelReached",highestLevelReached);
            Debug.Log("Highest level" + highestLevelReached);
        }
        CameraManager.Instance.SprayConfettis();
        levelPassed = true;
        Invoke("StartLevel",2);
    }

    public void StartLevel()
    {
        highestLevelReached = PlayerPrefs.GetString("highestLevelReached");
        Debug.Log("Starting level " + highestLevelReached);
        if(highestLevelReached != null && highestLevelReached != "")
        {
            try
            {
                SceneManager.LoadScene(highestLevelReached);
            }catch(Exception e)
            {
                Debug.Log("Start level exception: " + e.ToString());
                SceneManager.LoadScene(levels[0].name);
            }
            
        }else
        {
            SceneManager.LoadScene(levels[0].name);
        }
    }

    public bool LevelPassed()
    {
        return levelPassed;
    }
}
