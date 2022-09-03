using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading scene " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
    
}
