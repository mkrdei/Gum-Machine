using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private LimitBox[] limitBoxes;
    private float timeToWaitAfterCompleted = 2.5f;
    private float passTimestamp;
    private float timerTick = 0.0f;
    private float timerTickInterval = 0.1f;
    private int triggeredLimitBoxNumber;
    public bool platformPassed;
    public bool temporaryPass;

    void Start()
    {
        limitBoxes = gameObject.transform.Find("Limits").GetComponentsInChildren<LimitBox>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(PlatformManager.Instance.GetCurrentPlatform()==gameObject)
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
                if(timerTick<sliderValue)
                {
                    AudioManager.Instance.PlayAudioOneShot("Timer",0.5f,1.25f);
                    timerTick+=timerTickInterval;
                }
                if(passTimestamp <= Time.time && FallCounter.Instance.GetFallCount()<PlatformManager.Instance.fallLimit)
                {
                    platformPassed = true;
                }
            }else
            {
                timerTick=0f;
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
