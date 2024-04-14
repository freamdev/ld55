using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MenuMageLookScreen : MonoBehaviour
{
    private void Update()
    {
        var mousePos = Input.mousePosition;
        var worldMouse = Camera.main.ScreenToWorldPoint(mousePos);

        transform.LookAt(worldMouse, Vector3.up);
    }
}