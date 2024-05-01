using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioIntroWithLoopController : MonoBehaviour
{
    public AudioClip Intro;
    public AudioClip Loop;

    private void Start()
    {
        GetComponent<AudioSource>().loop = true;
        StartCoroutine(PlayAudioLoop());
    }

    IEnumerator PlayAudioLoop()
    {
        GetComponent<AudioSource>().clip = Intro;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(GetComponent<AudioSource>().clip.length);
        GetComponent<AudioSource>().clip = Loop;
        GetComponent<AudioSource>().Play();
    }
}