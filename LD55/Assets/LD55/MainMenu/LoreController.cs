using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoreController : MainMenuPanelController
{
    public Button NextButton;
    public Button PrevButton;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public Image Image;
    public List<LoreElementSO> Lores;

    int currentLoreIndex;

    private void Awake()
    {
        BaseAwake();
        Lores = Resources.LoadAll<LoreElementSO>("LoreElements").ToList();

        NextButton.onClick.AddListener(() => Next());
        PrevButton.onClick.AddListener(() => Prev());
    }

    public void Next()
    {
        currentLoreIndex++;
        RefreshLore();
    }

    public void Prev()
    {
        currentLoreIndex--;
        RefreshLore();
    }

    private void RefreshLore()
    {
        var currentLore = Lores[Mathf.Abs(currentLoreIndex) % Lores.Count];
        Title.text = currentLore.Title;
        Description.text = currentLore.Text;
        Image.sprite = currentLore.Image;
    }
}