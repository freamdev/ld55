using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageAura : MonoBehaviour
{
    public float TickTimer;
    public float Range;
    public float Damage;

    EnemyController enemyController;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        StartCoroutine(Tick());
    }

    IEnumerator Tick()
    {
        while(true)
        {
            yield return new WaitForSeconds(TickTimer);
            var distance = Vector3.Distance(transform.position, enemyController.Target.transform.position);
            if(distance <= Range)
            {
                enemyController.Target.TakeDamge(Damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}