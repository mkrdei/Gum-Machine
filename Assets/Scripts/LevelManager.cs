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
    private string currentLevel,highestLevelReached;
    private bool levelPassed = false;
    private int currentLevelIndex;
    // Start is called before the first frame update
    void Start()
    {
        LoadLevelNames();
        AssignCurrentLevelIndex();
        SaveCurrentLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PassLevel()
    {
        SaveHighestLevelReached();
        CameraManager.Instance.SprayConfettis();
        levelPassed = true;
        LoadNextLevel();
    }

    public IEnumerator LoadLevelWithDelay(string levelName, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Loading level " + levelName);
        SceneManager.LoadScene(levelName);
    }
    public void LoadLevel(string levelName)
    {
        Debug.Log("Loading level " + levelName);
        SceneManager.LoadScene(levelName);
    }

    
    private void LoadNextLevel()
    {
        if(currentLevelIndex+1<levels.Length)
        {
            string nextLevel = levels[currentLevelIndex+1];
            StartCoroutine(LoadLevelWithDelay(nextLevel,2f));
        }
        else
        {
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
        if(highestLevelReachedIndex<currentLevelIndex+1)
        {
            PlayerPrefs.SetInt("highestLevelReachedIndex",currentLevelIndex+1);
        }
    }

    private void AssignCurrentLevelIndex()
    {
        currentLevelIndex = 0;
        foreach(string level in levels)
        {
            if(level==SceneManager.GetActiveScene().name)
            {
                break;
            }
            currentLevelIndex+=1;
        }
        if(currentLevelIndex==levels.Length)
        {
            currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex");
        }
    }
    private void SaveCurrentLevel()
    {
        if(levels[currentLevelIndex].Contains("PlatformLevel"))
            PlayerPrefs.SetInt("currentLevelIndex", currentLevelIndex);
    }
    public void LoadCurrentLevel()
    {
        currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex");
        currentLevel = levels[currentLevelIndex];
        LoadLevel(currentLevel);
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
