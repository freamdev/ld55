using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerLookController : MonoBehaviour
{
    public LayerMask layers;

    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController.IsDead) return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, 10000f, layers))
        {
            var dir = hitInfo.point - transform.position;
            dir.y = 0;//transform.position.y;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}