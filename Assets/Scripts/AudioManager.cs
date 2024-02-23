using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    private void Awake() 
    { 
        
        if (instance != null && instance != this) 
        { 
            Destroy(this);
            return;
        } 
        instance = this;
    }

    public AudioSource audioSource;
    public AudioClip selectClip;
    public AudioClip discoverClip;

    public void PlaySelectClip()
    {
        audioSource.PlayOneShot(selectClip);
    }
    public void PlayDiscoverClip()
    {
        audioSource.PlayOneShot(discoverClip);
    }


}
