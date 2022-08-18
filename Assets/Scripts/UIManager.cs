using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
    GameObject slider;
    RectTransform sliderBody, sliderFilled;
    float sliderValue;

    // Start is called before the first frame update
    void Start()
    {
        slider = GameObject.Find("Canvas").transform.Find("Slider").gameObject;
        sliderBody = slider.transform.Find("Body").GetComponent<RectTransform>();
        sliderFilled = slider.transform.Find("Filled").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
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
}
