using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
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

    public TextMeshProUGUI DamageTextPrefab;
    public TextMeshProUGUI HealTextPrefab;
    public Canvas HeadoverCanvas;

    EnemyAttackBase attacks;
    float maxHealth;
    public float HealthPercentage => Health / maxHealth;

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
    }

    private void Update()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
            if (IsBoss)
            {
                Target.BossDied();
            }
            return;
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
            }
        }
    }
}