using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FloatingUp : MonoBehaviour
{

    private void Update()
    {
        transform.position += Vector3.up * 3 * Time.deltaTime;
    }
}