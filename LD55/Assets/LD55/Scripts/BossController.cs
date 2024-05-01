using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossController : MonoBehaviour
{
    public List<Phases> Phases;

    EnemyController controller;
    int currentPhase;

    private void Awake()
    {
        currentPhase = 0;
        controller = GetComponent<EnemyController>();
    }

    private void Start()
    {
        controller.SwapAbilities(Phases[currentPhase].AttacksGained);
    }

    private void Update()
    {
        if (currentPhase == Phases.Count - 1)
        {
            return;
        }

        if (Phases[currentPhase + 1].HealthPercentage / 100f >= controller.HealthPercentage)
        {
            currentPhase++;
            controller.SwapAbilities(Phases[currentPhase].AttacksGained);
            if (Phases[currentPhase].Gfx != null)
            {
                var effect = Instantiate(Phases[currentPhase].Gfx, transform);
                effect.transform.position += Vector3.up * 1;
            }
        }
    }
}

[Serializable]
public class Phases
{
    public float HealthPercentage;
    public List<EnemyAttack> AttacksGained;
    public GameObject Gfx;
}