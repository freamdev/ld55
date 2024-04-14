using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSecondAttackProjectileController : MonoBehaviour
{
    public float Rotate;
    public float SpeedIncrease;

    float speed;

    private void Start()
    {
        speed = 8;
    }

    private void Update()
    {
        speed += Time.deltaTime* SpeedIncrease;
        if(speed > 12)
        {
            speed = 12;
        }
        transform.Rotate(Vector3.up, Rotate * Time.deltaTime);
        transform.position += Time.deltaTime * speed * transform.forward;
    }
}