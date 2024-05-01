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
    public GameObject TeleportEffectPrefab;
    public GameObject AscendedTeleportEffectPrefab;

    PlayerController playerController;

    float blinkCooldownLeft;

    public Vector3 LastMovement { get; set; }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        if (PlayerPrefs.HasKey(PlayerPrefConsts.BONUS))
        {
            var bonus = PlayerPrefs.GetInt(PlayerPrefConsts.BONUS, 1);
            if (bonus > 0)
            {
                TeleportEffectPrefab = AscendedTeleportEffectPrefab;
            }
        }
    }

    private void Update()
    {
        if (playerController.IsDead) return;

        if (BlinkEnabled)
        {
            BlinkAbilityImage.fillAmount = 1 - (blinkCooldownLeft / BlinkCooldown);
            BlinkUnlockText.text = "";
        }
    }

    private void LateUpdate()
    {
        if (playerController.IsDead) return;

        Move();
        Blink();
    }

    private void Blink()
    {
        if (!BlinkEnabled) return;
        if (blinkCooldownLeft <= 0 && Input.GetButton("Jump"))
        {
            Instantiate(TeleportEffectPrefab, transform.position, Quaternion.identity);

            var xInput = Input.GetAxis("Horizontal");
            var zInput = Input.GetAxis("Vertical");

            var dir = (new Vector3(xInput, 0, zInput)).normalized;

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

        LastMovement = dir * MovementSpeed;

        transform.position += dir * MovementSpeed * Time.deltaTime;
    }
}