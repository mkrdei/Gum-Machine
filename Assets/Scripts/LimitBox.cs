using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitBox : MonoBehaviour
{
    public Material[] materials;
    private Renderer m_renderer;
    public bool triggered;
    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider other)
    {
        if(other.tag=="Gum")
        {
            m_renderer.material = materials[1];
            triggered = true;
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag=="Gum")
        {
        m_renderer.material = materials[0];
        triggered = false;
        }
    }

    
}
