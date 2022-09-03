using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using System.IO;
public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]
    private string[] levels;
    private string reachedLevel,highestLevelReached;
    private bool levelPassed = false;
    private int reachedLevelIndex;
    // Start is called before the first frame update
    void Start()
    {
        LoadLevelNames();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PassLevel()
    {
        SaveHighestLevelReached();
        SaveReachedLevel();
        CameraManager.Instance.SprayConfettis();
        levelPassed = true;
        LoadReachedLevel();


    }

    public IEnumerator LoadLevelWithDelay(string levelName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(levelName);
    }
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadReachedLevel()
    {
        reachedLevelIndex = PlayerPrefs.GetInt("reachedLevelIndex");
        
        if(reachedLevelIndex<levels.Length)
        {
            reachedLevel = levels[reachedLevelIndex];
            StartCoroutine(LoadLevelWithDelay(reachedLevel,2f));
        }
        else
        {
            reachedLevelIndex = 0;
            PlayerPrefs.SetInt("reachedLevelIndex", reachedLevelIndex);
            StartCoroutine(LoadLevelWithDelay("MainMenu",2f));
        }
            
    }

    public bool LevelPassed()
    {
        return levelPassed;
    }

    private void SaveHighestLevelReached()
    {
        int highestLevelReachedIndex = PlayerPrefs.GetInt("highestLevelReachedIndex");
        if(highestLevelReachedIndex<reachedLevelIndex)
        {
            Debug.Log("New level record " + reachedLevelIndex + " " + highestLevelReachedIndex);
            PlayerPrefs.SetInt("highestLevelReachedIndex",reachedLevelIndex);
        }
    }

    private void SaveReachedLevel()
    {
        reachedLevelIndex = 0;
        foreach(string level in levels)
        {
            reachedLevelIndex+=1;
            if(level==SceneManager.GetActiveScene().name)
            {
                break;
            }
        }
        PlayerPrefs.SetInt("reachedLevelIndex", reachedLevelIndex);
        SaveHighestLevelReached();
    }
    private void LoadLevelNames()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;  
        List<string> levelsList = new List<string>();
        for(int i = 0; i < sceneCount; i++ )
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            path = path.Replace('/',Path.DirectorySeparatorChar);
            path = path.Replace('\\',Path.DirectorySeparatorChar);
            string sceneName = path.Replace(".unity","");
            string[] a = sceneName.Split(Path.DirectorySeparatorChar);
            sceneName = a[a.Length - 1];
            if(sceneName.Contains("PlatformLevel"))
            {
                levelsList.Add(sceneName);
            }
        }
        levels = levelsList.ToArray();
    }

    public string[] GetLevelNames()
    {
        return levels;
    }
    public int GetHighestLevelReachedIndex()
    {
        int highestLevelReachedIndex = PlayerPrefs.GetInt("highestLevelReachedIndex");
        return highestLevelReachedIndex;
    }
    public string GetLevelNameByIndex(int levelIndex)
    {
        return levels[levelIndex];
    }
    public int GetLevelLength()
    {
        return levels.Length;
    }
}
