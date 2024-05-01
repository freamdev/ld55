using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayController : MainMenuPanelController
{
    public Button PlayNormalButton;
    public Button PlayEndlessButton;
    public string SceneToLoad;

    private void Awake()
    {
        BaseAwake();

        PlayNormalButton.onClick.AddListener(() => StartGame(GameModeConsts.STORY));
        PlayEndlessButton.onClick.AddListener(() => StartGame(GameModeConsts.ENDLESS));
    }

    void StartGame(string type)
    {
        PlayerPrefs.SetString(PlayerPrefConsts.GAME_MODE, type);
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneToLoad);
    }
}