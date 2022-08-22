using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : Singleton<ParticleSystemManager>
{
    public ParticleSystem[] particleSystems;
    // Start is called before the first frame update
    void Start()
    {
        particleSystems = Resources.LoadAll<ParticleSystem>("Particle Systems");
    }

    // Update is called once per frame
    public void PlayParticleSystem(string particleSystemName, Transform _parent, Vector3 _position)
    {
        foreach(ParticleSystem particleSystem in particleSystems)
        {
            if(particleSystem.name==particleSystemName)
            {
                ParticleSystem particle = Instantiate(particleSystem,_position,Quaternion.identity,_parent);
                particleSystem.Play();
                // No need, ParticleSystem already has a feature.
                //StartCoroutine(DestroyFinishedParticleSystem(particle, particleSystem.main.startLifetime.constant));
                break;
            }
        }
        
    }
    IEnumerator DestroyFinishedParticleSystem(ParticleSystem particleSystem,float duration)
    {
        yield return new WaitForSeconds(duration);
        if(particleSystem.gameObject!=null)
            Destroy(particleSystem.gameObject);
    }
}
