using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class StoryScript : MonoBehaviour
{
    [Multiline]
    public string TextToPrint;
    public TextMeshProUGUI TextUI;
    public float WordDelay;
    public float ParagraphDelay;

    private void Awake()
    {
        StartCoroutine(PrintTextWithDelay());
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