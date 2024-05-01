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
    public GameObject ascendedPlayerProjectile;
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
    PlayerController playerController;

    int fireCount;
    public bool EnableDoubleFire;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        SecondaryAbility = Instantiate(SecondaryAbility);
        if (PlayerPrefs.HasKey(PlayerPrefConsts.BONUS))
        {
            var bonus = PlayerPrefs.GetInt(PlayerPrefConsts.BONUS, 1);
            if (bonus > 0)
            {
                playerProjectile = ascendedPlayerProjectile;
                SecondaryAbility.projetile = ascendedPlayerProjectile;
            }
        }

    }

    private void Update()
    {
        if (playerController.IsDead) return;

        if (baseAttackCooldownLeft <= 0)
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

            if (fireCount > 7 && EnableDoubleFire)
            {
                fireCount = 0;
                for (int j = -1; j < 2; j++)
                {
                    var bullet = Instantiate(playerProjectile, BulletSpawnPosition.position, transform.rotation);
                    bullet.transform.Rotate(Vector3.up, j * 10);
                    bullet.GetComponent<ProjectileBaseController>().Damage = BaseAttackDamage * Random.Range(.75f, 1.25f);
                }
            }
            else
            {
                var bullet = Instantiate(playerProjectile, BulletSpawnPosition.position, transform.rotation);
                bullet.GetComponent<ProjectileBaseController>().Damage = BaseAttackDamage * Random.Range(.75f, 1.25f);
            }
            fireCount++;

        }
    }
}