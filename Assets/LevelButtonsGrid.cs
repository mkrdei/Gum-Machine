using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelButtonsGrid : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        CreateLevelButtons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateLevelButtons()
    {
        int levelLength = LevelManager.Instance.GetLevelLength();
        int highestLevelReachedIndex = LevelManager.Instance.GetHighestLevelReachedIndex();
        for (int i = 0; i < levelLength; i++)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab,transform);
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = (i+1).ToString();
            Button button = levelButton.GetComponent<Button>();
            if(highestLevelReachedIndex>=i)
            {
                button.interactable = true;
            }
            button.onClick.AddListener(() => {LoadLevel(levelButton.transform.GetSiblingIndex());});
        }
    }
    private void LoadLevel(int levelIndex)
    {
        Debug.Log(levelIndex);
        string levelName = LevelManager.Instance.GetLevelNameByIndex(levelIndex);
        Debug.Log(levelName +" Loading");
        LevelManager.Instance.LoadLevel(levelName);
    }
}
