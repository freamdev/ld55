using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChargeEnemy : MonoBehaviour
{
    public float ChargeCooldown;
    public float ChargeDistance;
    public float ChargeSpeed;
    public float ChargeChargeUpTime;
    public float ChargeDazeTime;
    public GameObject ChargeLandGfx;
    public Transform Model;

    bool isCharging;
    float chargeCooldownLeft;

    EnemyController enemyController;
    EnemyMovementController enemyMovementController;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        enemyMovementController = GetComponent<EnemyMovementController>();
    }

    private void Update()
    {
        if (isCharging) return;

        var distance = Vector3.Distance(transform.position, enemyController.Target.transform.position);
        if(chargeCooldownLeft <= 0 && distance < ChargeDistance)
        {
            enemyMovementController.Chase = false;
            StartCoroutine(ChargeAttack());
        }

        chargeCooldownLeft -= Time.deltaTime;
    }

    IEnumerator ChargeAttack()
    {
        chargeCooldownLeft = ChargeCooldown;
        yield return new WaitForSeconds(ChargeChargeUpTime);
        var realChargeDistance = Vector3.Distance(enemyController.Target.transform.position, transform.position);
        var chargeTime = realChargeDistance / ChargeSpeed;
        var t = 0f;

        while(t < chargeTime)
        {
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            transform.position += transform.forward * ChargeSpeed * Time.deltaTime;
        }

        Instantiate(ChargeLandGfx, transform.position + Vector3.up, Quaternion.identity).transform.SetParent(transform);

        t = 0f;
        while (t < ChargeDazeTime)
        {
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
            Model.Rotate(Vector3.up, 1280 * Time.deltaTime);
        }

        isCharging = false;
        enemyMovementController.Chase = true;
        Model.rotation = transform.rotation;
    }

}