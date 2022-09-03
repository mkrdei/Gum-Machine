using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject platformIcon;
    private GameObject slider;
    private RectTransform sliderBody, sliderFilled;
    private Image blackScreen;
    private GameObject platformIcons;
    private GameObject strikesContainer;
    private Color blackScreenColor;
    private float sliderValue;
    [SerializeField]
    private float fadeTime = 0.5f;
    private bool fadeIn,fadeOut;

    
    // Start is called before the first frame update
    void Start()
    {
        SetPlatformIcons();
        
        blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        blackScreenColor = new Color(255,255,255,0);
        blackScreenColor = blackScreen.color;

        slider = GameObject.Find("Slider");
        sliderBody = slider.transform.Find("Body").GetComponent<RectTransform>();
        sliderFilled = slider.transform.Find("Filled").GetComponent<RectTransform>();

        strikesContainer = GameObject.Find("StrikesContainer");

        // Fadeout at the start of the level.
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
            blackScreenColor.a = Mathf.MoveTowards(blackScreenColor.a,0f,Time.deltaTime/fadeTime);
            blackScreen.color = blackScreenColor;
            fadeIn = false;
            if(blackScreenColor.a==0f)
                fadeOut = false;
        }
    }

    public bool BlackScreenEnabled()
    {
        bool blacked = blackScreenColor.a == 1f;
        if(blacked)
            fadeIn = false;
        return blacked;
    }
    public void Strike()
    {
        Image strikeImage = strikesContainer.transform.GetChild(FallCounter.Instance.GetFallCount()-1).GetComponent<Image>();
        strikeImage.color = Palette.Instance.GetColor("FailRed");
    }
    public void ResetStrikes()
    {
        for(int i=0; i<strikesContainer.transform.childCount; i++)
        {
            Image strikeImage = strikesContainer.transform.GetChild(i).GetComponent<Image>();
            strikeImage.color = Palette.Instance.GetColor("White");
        }
    }
    private void SetPlatformIcons()
    {
        platformIcons = GameObject.Find("PlatformIcons");
        for(int i=0; i<PlatformManager.Instance.GetPlatformSize(); i++)
        {   
            GameObject icon = Instantiate(platformIcon,platformIcons.transform);
            icon.GetComponentInChildren<TextMeshProUGUI>().text = (i+1).ToString();
            if(i==0)
                icon.GetComponent<Image>().color = Palette.Instance.GetColor("LightGum");
        }
    }

    public void SetPlatformIconColor(int platformIndex, string colorName)
    {
        Image iconImage = platformIcons.transform.GetChild(platformIndex).GetComponent<Image>();
        iconImage.color = Palette.Instance.GetColor(colorName);
    }
    public bool IsFadingIn()
    {
        return fadeIn;
    }
    public bool IsFadingOut()
    {
        return fadeOut;
    }
    public void FadeIn(bool _fadeIn)
    {
        fadeIn = _fadeIn;
    }
    public void FadeOut(bool _fadeOut)
    {
        fadeOut = _fadeOut;
    }
}
