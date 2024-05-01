using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealingAura : MonoBehaviour
{
    public float TickTimer;
    public float Range;
    public float Heal;
    public GameObject AuraGfx;

    private void Start()
    {
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(TickTimer);
            Instantiate(AuraGfx, transform.position, Quaternion.identity);
            var enemies = GameObject.FindObjectsOfType<EnemyController>();
            foreach (var enemy in enemies)
            {
                var distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= Range)
                {
                    enemy.Heal(Heal);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}