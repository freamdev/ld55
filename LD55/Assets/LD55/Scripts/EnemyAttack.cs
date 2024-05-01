using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "LD55/Enemy attack")]
public class EnemyAttack : ScriptableObject
{
    [Range(0, 100)]
    public int Priority;
    public float Cooldown;
    public float BasePower;
    public bool IsMelee;
    public bool IsPredicting;

    public GameObject projetile;
    public int projectileCount;
    public float projectileSpread;
    public float projectileDelay;

    float cooldonwLeft;

    public bool CanBeUsed() => cooldonwLeft <= 0;

    public virtual void Use(EnemyController user)
    {
        cooldonwLeft = Cooldown;
        if (IsMelee)
        {
            user.Target.TakeDamge(BasePower * user.SkillPower);
        }
        else
        {
            user.StartCoroutine(FireProjectiles(user));
        }
    }

    public virtual void Use(PlayerController user)
    {
        cooldonwLeft = Cooldown;
        user.StartCoroutine(FireProjectilesPlayer(user));
    }

    IEnumerator FireProjectilesPlayer(PlayerController user)
    {
        var bulletsPerSide = projectileCount / 2;

        var currentProjectile = -bulletsPerSide;

        for (int i = 0; i < projectileCount; i++)
        {
            yield return new WaitForSeconds(projectileDelay);
            var spread = currentProjectile * projectileSpread;

            var instance = Instantiate(projetile, user.transform.position + Vector3.up, user.transform.rotation);
            instance.transform.Rotate(Vector3.up, spread);
            currentProjectile++;
            if (projectileCount % 2 == 0 && currentProjectile == 0)
            {
                currentProjectile++;
            }
            instance.GetComponent<ProjectileBaseController>().Damage = BasePower * Random.Range(.75f, 1.25f);
        }
    }

    IEnumerator FireProjectiles(EnemyController user)
    {
        var bulletsPerSide = projectileCount / 2;

        var currentProjectile = -bulletsPerSide;

        for (int i = 0; i < projectileCount; i++)
        {
            yield return new WaitForSeconds(projectileDelay);
            var spread = currentProjectile * projectileSpread;



            var instance = Instantiate(projetile, user.transform.position + Vector3.up, user.transform.rotation);
            instance.transform.Rotate(Vector3.up, spread);
            currentProjectile++;
            if (projectileCount % 2 == 0 && currentProjectile == 0)
            {
                currentProjectile++;
            }
            instance.GetComponent<ProjectileBaseController>().Damage = user.SkillPower * BasePower * user.SkillPower;

            if (IsPredicting)
            {
                var targetDirection = user.Target.transform.position - user.transform.position;
                var timeToReachTarget = targetDirection.magnitude / instance.GetComponent<ProjectileBaseController>().Speed;
                var futureTargetPosition = user.Target.transform.position + user.Target.GetComponent<PlayerMovementController>().LastMovement * timeToReachTarget;
                var futureTargetDirection = futureTargetPosition - user.transform.position;
                var targetRotation = Quaternion.LookRotation(futureTargetDirection);
                instance.transform.rotation = Quaternion.RotateTowards(instance.transform.rotation, targetRotation, 99999);
                instance.transform.Rotate(Vector3.up, spread);
            }
        }
    }

    public void Tick()
    {
        cooldonwLeft -= Time.deltaTime;
    }
}