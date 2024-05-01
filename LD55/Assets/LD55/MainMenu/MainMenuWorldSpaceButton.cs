using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MainMenuWorldSpaceButton : MonoBehaviour
{
    public string SceneToLoad;

    private void OnMouseDown()
    {
        if (SceneToLoad.Contains("GameScene"))
        {
            Destroy(FindObjectOfType<AudioSource>().gameObject);
        }

        SceneManager.LoadScene(SceneToLoad);
    }
}