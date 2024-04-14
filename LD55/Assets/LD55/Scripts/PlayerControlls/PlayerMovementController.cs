using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMovementController : MonoBehaviour
{
    public float MovementSpeed;
    public float BlinkDistance;
    public float BlinkCooldown;

    public bool BlinkEnabled;
    public Image BlinkAbilityImage;
    public TextMeshProUGUI BlinkUnlockText;

    float blinkCooldownLeft;

    private void Awake()
    {
    }

    private void Update()
    {
        if (BlinkEnabled)
        {
            BlinkAbilityImage.fillAmount = 1 - (blinkCooldownLeft / BlinkCooldown);
            BlinkUnlockText.text = "";
        }
    }

    private void LateUpdate()
    {
        Move();
        Blink();
    }

    private void Blink()
    {
        if (!BlinkEnabled) return;
        if (blinkCooldownLeft <= 0 && Input.GetAxis("Jump") == 1)
        {
            var xInput = Input.GetAxis("Horizontal");
            var zInput = Input.GetAxis("Vertical");

            var dir = new Vector3(xInput, 0, zInput);

            if (dir == Vector3.zero)
            {
                dir = transform.forward;
            }

            blinkCooldownLeft = BlinkCooldown;
            transform.position += dir * BlinkDistance;
        }

        blinkCooldownLeft -= Time.deltaTime;
    }

    private void Move()
    {
        var xInput = Input.GetAxis("Horizontal");
        var zInput = Input.GetAxis("Vertical");

        var dir = new Vector3(xInput, 0, zInput);

        transform.position += dir * MovementSpeed * Time.deltaTime;
    }
}