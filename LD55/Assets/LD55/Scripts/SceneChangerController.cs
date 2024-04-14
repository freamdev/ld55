using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneChangerController : MonoBehaviour
{
    public string SceneName;
    public Button LoadSceneButton;

    private void Start()
    {
        LoadSceneButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneName);
        });
    }
}