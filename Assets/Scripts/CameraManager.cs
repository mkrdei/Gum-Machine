using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    private Renderer _renderer;
    private Transform blackScreen;
    public Vector3 offset = new Vector3(0,0,-27.25f);
    public float positioningTime = 1;
    public float fadeTime = 0.5f;
    [HideInInspector]
    public bool fadeInAndOut,blackScreenEnabled,positioning;
    private Color blackScreenColor;
    private Vector3 platformOffsetPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        blackScreen = transform.Find("Black Screen");
        _renderer = blackScreen.GetComponent<Renderer>();
        blackScreenColor = _renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        FadeInAndOut();
        SetPositionToPlatform();
    }
    public void SetPositionToPlatform()
    {
        platformOffsetPosition = new Vector3(LevelManager.Instance.GetCurrentPlatform().transform.position.x+offset.x, Camera.main.transform.position.y, LevelManager.Instance.GetCurrentPlatform().transform.position.z+offset.z);
        Debug.Log((Camera.main.transform.position-platformOffsetPosition).sqrMagnitude<0.1f);
        if((Camera.main.transform.position-platformOffsetPosition).sqrMagnitude>0.1f)
        {
            float distance = Vector3.Distance(platformOffsetPosition,Camera.main.transform.position);
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, platformOffsetPosition, (distance/positioningTime)*Time.deltaTime);
            positioning = true;
        }      
        else
        {
            positioning = false;
        }
    }

    public void FadeInAndOut()
    {
        if(fadeInAndOut)
        {
            if(blackScreenColor.a == 1f)
            {
                blackScreenEnabled = true;
            }
            if(!blackScreenEnabled)
            {
                blackScreenColor.a = Mathf.MoveTowards(blackScreenColor.a,1f,Time.deltaTime/fadeTime);
                _renderer.material.color = blackScreenColor;
            }
            else if(blackScreenEnabled)
            {
                blackScreenColor.a = Mathf.MoveTowards(blackScreenColor.a,0f,Time.deltaTime/fadeTime);
                _renderer.material.color = blackScreenColor;
            }
            // Starts with 0.0...01 alpha value. When it comes to 0 it will be the end of the cycle of fading. 
            if(blackScreenColor.a == 0f)
            {
                fadeInAndOut = false;
                blackScreenEnabled = false;
            }
            
        }
        
    }
}
