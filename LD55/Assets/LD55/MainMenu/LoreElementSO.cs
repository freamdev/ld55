using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "LD55/Create info asset")]
public class LoreElementSO : ScriptableObject
{
    public Sprite Image;
    public string Title;
    [Multiline]
    public string Text;

}