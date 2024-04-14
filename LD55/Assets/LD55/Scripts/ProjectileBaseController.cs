using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileBaseController : MonoBehaviour
{
    public float Speed = 8f;
    public float Damage;

    private void Awake()
    {
        Destroy(gameObject, 8f);
    }

    private void Update()
    {
        transform.position += Time.deltaTime * Speed * transform.forward;
    }
}