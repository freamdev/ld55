using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyMovementController))]
public class EnemyController : MonoBehaviour
{
    public float Health;
    public float Range;
    public float AttackRange;
    public float SkillPower;
    public PlayerController Target;
    public bool IsBoss;
    [Range(0f, 0.3f)]
    public float ExperienceWorth;

    public TextMeshProUGUI DamageTextPrefab;
    public TextMeshProUGUI HealTextPrefab;
    public Canvas HeadoverCanvas;
    public GameObject DieEffect;

    float highliteTime;
    bool isHighlighted;

    EnemyAttackBase attacks;
    float maxHealth;
    public float HealthPercentage => Health / maxHealth;
    public Image DiedImage { get; set; }

    private void Awake()
    {
        maxHealth = Health;
        Target = FindObjectOfType<PlayerController>();
        attacks = GetComponent<EnemyAttackBase>();
    }

    private void Start()
    {

    }

    public void SwapAbilities(List<EnemyAttack> newAbilities)
    {
        attacks.SwapAbilities(newAbilities);
    }

    public void Heal(float value)
    {
        Health = MathF.Min(Health + value, maxHealth);
        var damageText = Instantiate(DamageTextPrefab, HeadoverCanvas.transform);
        damageText.color = Color.green;
        damageText.text = value.ToString("0");
    }

    public void TakeDamage(float value)
    {
        Health -= value;
        var damageText = Instantiate(DamageTextPrefab, HeadoverCanvas.transform);
        damageText.text = value.ToString("0");

        isHighlighted = true;
        highliteTime = 0.2f;
        transform.GetChild(1).GetComponentInChildren<MeshRenderer>().material.color = Color.red;
    }

    private void Update()
    {
        highliteTime -= Time.deltaTime;

        if (Health <= 0)
        {
            Target.Experience += ExperienceWorth;
            HeadoverCanvas.transform.SetParent(null);
            Destroy(HeadoverCanvas.gameObject, 0.5f);
            Instantiate(DieEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            if (IsBoss)
            {
                Target.BossDied();
                DiedImage?.gameObject.SetActive(true);
            }
            return;
        }

        if (Vector3.Distance(transform.position, Vector3.zero) > 32)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }

        if (transform.position.y < -1)
        {
            Health = -999;
        }

        if (highliteTime <= 0 && isHighlighted)
        {
            isHighlighted = false;
            transform.GetChild(1).GetComponentInChildren<MeshRenderer>().material.color = Color.white;
        }

        var distance = Vector3.Distance(transform.position, Target.transform.position);
        if (distance < AttackRange)
        {
            var attack = attacks.GetAttack();
            if (attack != null)
            {
                attack.Use(this);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == Target.gameObject)
        {
            var meleeAttack = attacks.GetMeleeAttack();
            if (meleeAttack != null && meleeAttack.CanBeUsed())
            {
                meleeAttack.Use(this);
            }
        }
        else
        {
            if (other.gameObject.transform.parent != null && other.transform.parent.gameObject.GetComponent<ProjectileBaseController>() != null)
            {
                Destroy(other.gameObject);
                Destroy(other.transform.parent.gameObject);
                TakeDamage(other.transform.parent.GetComponent<ProjectileBaseController>().Damage);
                other.transform.parent.GetComponent<ProjectileBaseController>().Collided();
            }
        }
    }
}