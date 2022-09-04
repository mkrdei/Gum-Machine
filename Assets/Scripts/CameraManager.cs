using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraManager : Singleton<CameraManager>
{
    private Transform[] confettiTransforms;
    [SerializeField]
    private Vector3 offset = new Vector3(0,0,-27.25f);
    [SerializeField]
    private float positioningTime = 1;
    [HideInInspector]
    private bool positioning;
    private Vector3 platformOffsetPosition;
    private GameObject confettiPositionsParent;
    
    // Start is called before the first frame update
    void Start()
    {
        SetConfettis();
    }

    void Update()
    {
        SetPositionToPlatform();
    }
    public void SetPositionToPlatform()
    {
        platformOffsetPosition = new Vector3(PlatformManager.Instance.GetCurrentPlatform().transform.position.x+offset.x, Camera.main.transform.position.y, PlatformManager.Instance.GetCurrentPlatform().transform.position.z+offset.z);
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
    
    private void SetConfettis()
    {
        confettiPositionsParent = GameObject.Find("ConfettiPositions");
        int confettiNumber = confettiPositionsParent.transform.childCount;
        confettiTransforms = new Transform[confettiNumber];
        for(int i = 0; i < confettiNumber; i++)
        {
            confettiTransforms[i] = confettiPositionsParent.transform.GetChild(i).transform;
        }
    }
    public void SprayConfettis()
    {
        AudioManager.Instance.PlayAudioOneShot("Confetti",0.5f,1f);
        foreach(Transform confettiTransform in confettiTransforms)
            ParticleSystemManager.Instance.PlayParticleSystem("Confetti",confettiPositionsParent.transform,confettiTransform.position);
        
    }
    public float GetPositioningTime()
    {
        return positioningTime;
    }
    public bool IsPositioning()
    {
        return positioning;
    }
}
