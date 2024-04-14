using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyAttackBase : MonoBehaviour
{
    public List<EnemyAttack> enemyAttacks;
    EnemyController enemyController;

    private void Awake()
    {
        SwapAbilities(enemyAttacks);
    }

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    public void SwapAbilities(List<EnemyAttack> abilities)
    {
        enemyAttacks = abilities.Select(s => Instantiate(s)).ToList();
    }

    private void Update()
    {
        enemyAttacks.ForEach(f => f.Tick());
    }

    public EnemyAttack GetAttack()
    {
        return enemyAttacks.Where(w => w.CanBeUsed() && !w.IsMelee).OrderBy(o => o.Priority).FirstOrDefault();
    }

    public EnemyAttack GetMeleeAttack()
    {
        return enemyAttacks.FirstOrDefault(w => w.IsMelee);
    }
}


