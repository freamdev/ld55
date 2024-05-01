using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class StoryScript : MonoBehaviour
{
    [Multiline]
    public string TextToPrint;
    public TextMeshProUGUI TextUI;
    public float WordDelay;
    public float ParagraphDelay;

    public AudioMixer AudioMixer;

    private void Awake()
    {
        StartCoroutine(PrintTextWithDelay());
    }


    private void Start()
    {
        SetAudioValueIfExsist(PlayerPrefConsts.MASTER_AUDIO, "MasterVolume");
        SetAudioValueIfExsist(PlayerPrefConsts.MUSIC_AUDIO, "MusicVolume");
        SetAudioValueIfExsist(PlayerPrefConsts.EFFECT_AUDIO, "EffectsVolume");
    }

    private void SetAudioValueIfExsist(string palyerPrefsKey, string mixerKey)
    {
        if (PlayerPrefs.HasKey(palyerPrefsKey))
        {
            var value = PlayerPrefs.GetFloat(palyerPrefsKey);
            AudioMixer.SetFloat(mixerKey, (value * 100)-80);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

    IEnumerator PrintTextWithDelay()
    {
        string[] words = TextToPrint.Split('\n');

        for (int i = 0; i < words.Length; i++)
        {
            TextUI.text += words[i] + "\n";

            yield return new WaitForSeconds(WordDelay);
        }
    }
}