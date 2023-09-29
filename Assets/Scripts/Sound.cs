using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip[] clips;

    public AudioMixerGroup mixer;
    [Range(0, 1)]
    public float volume = 1f;
    [Range(0.1f, 3)]
    public float pitch = 1f;

    [HideInInspector]
    public AudioSource source;
}
