using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovementController : MonoBehaviour
{
    public float MovementSpeed;
    public bool DistanceKeeper;
    public bool Orbiting;

    EnemyController enemyController;

    private void Awake()
    {

    }

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();

    }

    private void Update()
    {
        var dir = enemyController.Target.transform.position - transform.position;
        dir.y = transform.position.y;
        var distance = Vector3.Distance(transform.position, enemyController.Target.transform.position);
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

        if (distance > enemyController.Range)
        {
            transform.position += transform.forward * Time.deltaTime * MovementSpeed;
        }
        else if (DistanceKeeper && distance < enemyController.Range)
        {
            transform.position -= transform.forward * Time.deltaTime * MovementSpeed;
        }

        if (distance <= enemyController.Range && Orbiting)
        {
            transform.position += transform.right * Time.deltaTime * MovementSpeed;
        }
    }
}