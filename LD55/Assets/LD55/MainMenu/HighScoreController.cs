using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Newtonsoft.Json;

public class HighScoreController : MainMenuPanelController
{
    public Button SwapTypeButton;
    public Transform Content;
    public GameObject TextPanelPrefab;

    bool isStoryMode;

    private void Awake()
    {
        BaseAwake();
        isStoryMode = true;
        SwapTypeButton.onClick.AddListener(ToggleMode);
        ToggleMode();
    }

    public override void OpenTab()
    {
        base.OpenTab();
    }

    public override void CloseTab()
    {
        base.CloseTab();
       
    }

    public void ToggleMode()
    {
        isStoryMode = !isStoryMode;
        foreach (Transform child in Content)
        {
            Destroy(child.gameObject);
        }

        if (isStoryMode)
        {
            SwapTypeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Endless";

            if (PlayerPrefs.HasKey(PlayerPrefConsts.STORY_HIGHSCORE))
            {
                var highScores = JsonConvert.DeserializeObject<List<float>>(PlayerPrefs.GetString(PlayerPrefConsts.STORY_HIGHSCORE));
                foreach (var highScore in highScores.OrderBy(o => o))
                {
                    var realScore = Mathf.Max(0, 1000 - highScore);
                    var instanceLeft = Instantiate(TextPanelPrefab, Content);
                    instanceLeft.transform.Find("TextLeft").GetComponent<TextMeshProUGUI>().text = "Score:";
                    instanceLeft.transform.Find("TextRight").GetComponent<TextMeshProUGUI>().text = $"{realScore:0}";
                }
            }
        }
        else
        {
            SwapTypeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Story Mode";

            if (PlayerPrefs.HasKey(PlayerPrefConsts.ENDLESS_HIGHSCORE))
            {
                var highScores = JsonConvert.DeserializeObject<List<float>>(PlayerPrefs.GetString(PlayerPrefConsts.ENDLESS_HIGHSCORE));
                foreach (var highScore in highScores.OrderByDescending(o => o))
                {
                    var instanceLeft = Instantiate(TextPanelPrefab, Content);
                    instanceLeft.transform.Find("TextLeft").GetComponent<TextMeshProUGUI>().text = "Score:";
                    instanceLeft.transform.Find("TextRight").GetComponent<TextMeshProUGUI>().text = $"{highScore:0}";
                }
            }
        }
    }
}