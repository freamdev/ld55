using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OptionsController : MainMenuPanelController
{
    public AudioMixer Mixer;

    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider EffectsSlider;

    public Button SaveButton;

    float openMasterValue;
    float openMusicValue;
    float openEffectsValue;

    private void Awake()
    {
        BaseAwake();

        MasterSlider.onValueChanged.AddListener((f) => { SliderChanged(f, "MasterVolume"); });
        MusicSlider.onValueChanged.AddListener((f) => { SliderChanged(f, "MusicVolume"); });
        EffectsSlider.onValueChanged.AddListener((f) => { SliderChanged(f, "EffectsVolume"); });

        SaveButton.onClick.AddListener(Save);
    }

    private void Start()
    {
        SetAudioValueIfExsist(PlayerPrefConsts.MASTER_AUDIO, "MasterVolume", MasterSlider);
        SetAudioValueIfExsist(PlayerPrefConsts.MUSIC_AUDIO, "MusicVolume", MusicSlider);
        SetAudioValueIfExsist(PlayerPrefConsts.EFFECT_AUDIO, "EffectsVolume", EffectsSlider);
    }

    public override void OpenTab()
    {
        base.OpenTab();
        openMasterValue = MasterSlider.value;
        openMusicValue = MusicSlider.value;
        openEffectsValue = EffectsSlider.value;

    }

    public override void CloseTab()
    {
        MasterSlider.value = openMasterValue;
        MusicSlider.value = openMusicValue;
        EffectsSlider.value = openEffectsValue;
        base.CloseTab();
    }

    private void SetAudioValueIfExsist(string palyerPrefsKey, string mixerKey, Slider slider)
    {
        if (PlayerPrefs.HasKey(palyerPrefsKey))
        {
            var value = PlayerPrefs.GetFloat(palyerPrefsKey);
            slider.value = value;
            Mixer.SetFloat(mixerKey, (value * 100) - 80);
        }
    }

    private void SliderChanged(float v, string channel)
    {
        var slidedValue = (v * 100) - 80;
        Mixer.SetFloat(channel, slidedValue);
    }

    public void Save()
    {
        PlayerPrefs.SetFloat(PlayerPrefConsts.MASTER_AUDIO, MasterSlider.value);
        PlayerPrefs.SetFloat(PlayerPrefConsts.MUSIC_AUDIO, MusicSlider.value);
        PlayerPrefs.SetFloat(PlayerPrefConsts.EFFECT_AUDIO, EffectsSlider.value);

        PlayerPrefs.Save();

        base.CloseTab();
    }
}