using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraManager : Singleton<CameraManager>
{
    public Vector3 offset = new Vector3(0,0,-27.25f);
    public float positioningTime = 1;
    [HideInInspector]
    public bool positioning;
    private Vector3 platformOffsetPosition;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        SetPositionToPlatform();
    }
    public void SetPositionToPlatform()
    {
        platformOffsetPosition = new Vector3(LevelManager.Instance.GetCurrentPlatform().transform.position.x+offset.x, Camera.main.transform.position.y, LevelManager.Instance.GetCurrentPlatform().transform.position.z+offset.z);
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
    
}
