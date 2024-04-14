using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RunePanelUI : MonoBehaviour
{
    public Sprite RuneBaseImage;
    public Image Rune;
    public Image RuneBackground;
    public Runes RuneType;

    public Image Boss;
    public Image BossBackground;

    private void Start()
    {
        Rune.sprite = RuneBaseImage;
        RuneBackground.sprite = RuneBaseImage;

        Boss.enabled = false;
        BossBackground.enabled = false;

        Rune.fillAmount = 0;
    }
}

[Serializable]
public enum Runes
{
    Eywas,
    Havoc,
    Urus
}