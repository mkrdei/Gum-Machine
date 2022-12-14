using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallCounter : Singleton<FallCounter>
{
    private int fallCount;
    private bool counting = true;
    // Start is called before the first frame update
    void Start()
    {
        fallCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.transform.tag=="Gum" && counting && fallCount<3)
        {
            fallCount += 1;
            UIManager.Instance.Strike();
        }
    }
    public int GetFallCount()
    {
        return fallCount;
    }
    public void ResetFallCount()
    {
        fallCount = 0;
    }
    public void Counting(bool _counting)
    {
        counting = _counting;
    }
}
