using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public class Sound {

    public string name;

    public AudioClip clip; //refernce to audioclip itself

    [Range(0f, 1f)] //Range of sound volume
    public float volume;

    [Range(.1f, 3f)] //Range of pich
    public float pitch;

    public bool loop;

    [HideInInspector] //Don't want it cluttering at runtime esp since it's populated automatically
    public AudioSource source;

}
