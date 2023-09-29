using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioManager audioManager;
 
    void Start()
    {
        audioManager = AudioManager.instance;   
    }

    public void PlayFootstepSound()
    {
        if (!audioManager)
            return;

        audioManager.Play("Footsteps");
    }

    public void PlayWhipSound()
    {
        if (!audioManager)
            return;

        audioManager.Play("Whip");
    }
}
