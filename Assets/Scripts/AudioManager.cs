using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AudioManager : Singleton<AudioManager>
{
    private AudioSource[] audioSources;
    private AudioClip[] audioClips;
    // Start is called before the first frame update
    void Start()
    {
       audioClips = Resources.LoadAll<AudioClip>("Audio");
       
    }

    void Update()
    {
        audioSources = GetComponents<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            if(source!=null)
                if(!source.isPlaying)
                {
                    Destroy(source);
                }
        }
    }

// Need to rework this function similar to PlayAudioOneShot, coroutines are slow.
/*
    public void PlayAudio(string audioName,float _volume,float _pitch)
    {
        foreach(AudioClip clip in audioClips)
        {
            if(clip.name==audioName)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.volume = _volume;
                source.pitch = _pitch;
                source.Play();
                //StartCoroutine(DestroyFinishedAudio(source, source.clip.length));
                break;
            }
        }
    }
    */
    public void PlayAudioOneShot(string audioName,float _volume,float _pitch)
    {
        foreach(AudioClip clip in audioClips)
        {
            if(clip.name==audioName)
            {
                AudioSource source = null;
                if(audioSources.Length != 0)
                    if(audioSources[0].clip!=null)
                        source = audioSources[0];
                    else
                        source = gameObject.AddComponent<AudioSource>();
                else
                    source = gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.volume = _volume;
                source.pitch = _pitch;
                source.PlayOneShot(clip);
                break;
            }
        }
    }
    IEnumerator DestroyFinishedAudio(AudioSource source,float audioDuration)
    {
        yield return new WaitForSeconds(audioDuration);
        if(source!=null)
            Destroy(source);
    }
}
