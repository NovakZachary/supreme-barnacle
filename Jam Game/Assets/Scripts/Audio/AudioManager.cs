using UnityEngine.Audio;
using System;
using UnityEngine;


public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    // Awake is called before Start
    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    //Searches through sounds[] and 
    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); //Using anonymous function
        s.source.Play(); //Will throw error if no sound found!
    }
}
