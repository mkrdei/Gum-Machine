using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{

    private int triggeredLimitBoxNumber;
    private LimitBox[] limitBoxes;
    public float timeToWaitAfterCompleted = 2.5f;
    [SerializeField]
    public bool platformPassed;
    public bool temporaryPass;
    private float passTimestamp;

    // Start is called before the first frame update
    void Start()
    {
        limitBoxes = gameObject.transform.Find("Limits").GetComponentsInChildren<LimitBox>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(LevelManager.Instance.GetCurrentPlatform()==gameObject)
        {
            foreach(LimitBox limitBox in limitBoxes)
            {
                if(!limitBox.triggered)
                {
                    temporaryPass = false;
                    triggeredLimitBoxNumber = 0;
                    break;
                }
                triggeredLimitBoxNumber++;     
                if(triggeredLimitBoxNumber==limitBoxes.Length)
                {
                    temporaryPass = true;
                    passTimestamp = Time.time + timeToWaitAfterCompleted;
                }
            }
            if(temporaryPass)
            {
                UIManager.Instance.GetSlider().SetActive(true);
                float sliderValue = Mathf.MoveTowards(UIManager.Instance.GetSliderValuePercentage(),1,Time.deltaTime/timeToWaitAfterCompleted);
                UIManager.Instance.SetSliderValuePercentage(sliderValue);
                if(passTimestamp <= Time.time)
                {
                    platformPassed = true;
                }
            }else
            {
                UIManager.Instance.SetSliderValuePercentage(0);
                UIManager.Instance.GetSlider().SetActive(false);
            }
        }
        
        
    }

    public GameObject DuplicatePlatform()
    {
        GameObject duplicatedPlatform = Instantiate(gameObject,transform.position,Quaternion.identity, transform.parent);
        duplicatedPlatform.name = transform.name;
        duplicatedPlatform.transform.SetSiblingIndex(transform.GetSiblingIndex());
        return duplicatedPlatform;
    }

}
