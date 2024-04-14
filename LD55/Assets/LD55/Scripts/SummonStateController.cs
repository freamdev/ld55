using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SummonStateController : MonoBehaviour
{
    const float MAX_SUMMON_VALUE = 100;

    public float CurrentSummonValue;
    public Runes RuneType;

    public float StartOffset;
    public float EndOffset;

    public EnemyController Boss;

    public float SummonValuePercentage => CurrentSummonValue / MAX_SUMMON_VALUE;

    RunePanelUI RunePanel;
    PlayerController PlayerController;
    bool isCharging;
    bool isCharged;

    LineRenderer lineRenderer;

    EnemyController summonedBoss;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        lineRenderer.enabled = false;
        RunePanel = FindObjectsOfType<RunePanelUI>().First(f => f.RuneType == RuneType);
        PlayerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        RunePanel.Rune.fillAmount = CurrentSummonValue / MAX_SUMMON_VALUE;
        if (isCharging && !isCharged)
        {
            lineRenderer.SetPosition(0, transform.position + StartOffset * Vector3.up);
            lineRenderer.SetPosition(1, PlayerController.transform.position + EndOffset * Vector3.up);
            CurrentSummonValue += Time.deltaTime * 1.6f;
            if (CurrentSummonValue > MAX_SUMMON_VALUE)
            {
                CurrentSummonValue = MAX_SUMMON_VALUE;
            }

            PlayerController.Heal(2 * Time.deltaTime);
        }

        if (CurrentSummonValue >= MAX_SUMMON_VALUE && !isCharged)
        {
            isCharged = true;
            summonedBoss = Instantiate(Boss);
            summonedBoss.transform.position = transform.position;

            RunePanel.Rune.enabled = false;
            RunePanel.RuneBackground.enabled = false;

            RunePanel.Boss.enabled = true;
            RunePanel.BossBackground.enabled = true;

            Destroy(lineRenderer);
        }

        if (summonedBoss != null)
        {
            RunePanel.Boss.fillAmount = summonedBoss.HealthPercentage;
        }
    }

    public void PlayerEnter()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
        }
        isCharging = true;
    }

    public void PlayerLeave()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
        isCharging = false;
    }
}