using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerAttackController : MonoBehaviour
{
    public GameObject playerProjectile;
    public float BaseAttackDamage;
    public GameObject secondAttackPrefab;
    public bool SecondAttack;
    public Transform BulletSpawnPosition;

    public int BaseProjectileCount = 0;

    public float BaseAttackCooldown;
    public Image BaseAttackImage;

    public float SecondAttackCooldown;
    public Image SecondAttackImage;

    public TextMeshProUGUI SpecialAttackText;
    float baseAttackCooldownLeft;
    float secondAttackCooldownLeft;

    public EnemyAttack SecondaryAbility;

    private void Awake()
    {
        SecondaryAbility = Instantiate(SecondaryAbility);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && baseAttackCooldownLeft <= 0)
        {
            baseAttackCooldownLeft = BaseAttackCooldown;
            StartCoroutine(FireBaseAttack());
        }

        if (Input.GetMouseButtonDown(1) && SecondAttack && secondAttackCooldownLeft <= 0)
        {
            secondAttackCooldownLeft = SecondAttackCooldown;
            SecondaryAbility.Use(GetComponent<PlayerController>());
        }

        if (SecondAttack)
        {
            SpecialAttackText.text = "";
            secondAttackCooldownLeft -= Time.deltaTime; 
            SecondAttackImage.fillAmount = 1 - (secondAttackCooldownLeft / SecondAttackCooldown);
        }

        BaseAttackImage.fillAmount = 1 - (baseAttackCooldownLeft / BaseAttackCooldown);
        baseAttackCooldownLeft -= Time.deltaTime;
    }

    IEnumerator FireBaseAttack()
    {
        for (int i = 0; i < BaseProjectileCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            var bullet = Instantiate(playerProjectile, BulletSpawnPosition.position, transform.rotation);
            bullet.GetComponent<ProjectileBaseController>().Damage = BaseAttackDamage * Random.Range(.75f, 1.25f);
        }
    }
}