using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
    GameObject slider;
    RectTransform sliderBody, sliderFilled;
    private Image blackScreen;
    float sliderValue;
    public float fadeTime = 0.5f;
    [HideInInspector]
    public bool fadeIn,fadeOut;
    private Color blackScreenColor;

    // Start is called before the first frame update
    void Start()
    {
        blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        blackScreenColor = new Color(255,255,255,0);
        blackScreenColor = blackScreen.color;
        slider = GameObject.Find("Canvas").transform.Find("Slider").gameObject;
        sliderBody = slider.transform.Find("Body").GetComponent<RectTransform>();
        sliderFilled = slider.transform.Find("Filled").GetComponent<RectTransform>();
        fadeOut = true;
    }

    // Update is called once per frame
    void Update()
    {
        FadeIn();
        FadeOut();
        ScaleSlider();
    }

    public void SetSliderValuePercentage(float value)
    {
        sliderValue = value>1?1:(value<0?0:value);
    }
    public float GetSliderValuePercentage()
    {
        return sliderValue;
    }

    private void ScaleSlider()
    {
        Vector3 scale = new Vector3(sliderBody.localScale.x*sliderValue,sliderBody.localScale.y,sliderBody.localScale.z);
        sliderFilled.localScale = scale;
    }
    public GameObject GetSlider()
    {
        return slider;
    }
    public void FadeIn()
    {
        if(fadeIn)
        {
            Debug.Log("FadeIn " + blackScreenColor.a);
            blackScreenColor.a = Mathf.MoveTowards(blackScreenColor.a,1f,Time.deltaTime/fadeTime);
            blackScreen.color = blackScreenColor;
            fadeOut = false;
            if(blackScreenColor.a==255f)
                fadeIn = false;
        }
    }
    public void FadeOut()
    {
        if(fadeOut)
        {
            Debug.Log("FadeOut " + blackScreenColor.a);
            blackScreenColor.a = Mathf.MoveTowards(blackScreenColor.a,0f,Time.deltaTime/fadeTime);
            blackScreen.color = blackScreenColor;
            fadeIn = false;
            if(blackScreenColor.a==0f)
                fadeOut = false;
        }
    }

    public bool BlackScreenEnabled()
    {
        Debug.Log("Black Screen");
        bool blacked = blackScreenColor.a == 1f;
        if(blacked)
            fadeIn = false;
        return blacked;
    }
    
}
