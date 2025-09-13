using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationListener : MonoBehaviour
{
    [SerializeField] AudioSource footStepSource;
    [SerializeField] AudioClip[] footStepClips;
    static bool soundsEnabled = false;
    public void Footstep()
    {
        if (footStepSource && footStepSource.clip && soundsEnabled)
        {
            //pick a random clip
            footStepSource.pitch = Random.Range(0.9f, 1.1f);
            footStepSource.clip = footStepClips[Random.Range(0, footStepClips.Length)];
            footStepSource.Play();
        }
    }

    public void NarvyDefaultMove()
    {
        
    }
}
