using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class AscendScript : MonoBehaviour
{
    public GameObject Normal;
    public GameObject Ascended;
    public Color AscendedTextColor;
    public TextMeshProUGUI MenuOnly;

    private void Start()
    {
        MenuOnly.enabled = false;
        var bonus = PlayerPrefs.GetInt("Bonus");
        if(bonus == 1)
        {
            Normal.SetActive(false);
            Ascended.SetActive(true);
            MenuOnly.enabled = true;
            GameObject.FindObjectsOfType<TextMeshProUGUI>().ToList().ForEach(f => f.color = AscendedTextColor); 
        }
    }
}