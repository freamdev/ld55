using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioPlayerController : MonoBehaviour
{
    private void Awake()
    {
        var audio = FindObjectsOfType<AudioPlayerController>();
        if(audio.Where(w => w != this).Count() > 0)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }
}