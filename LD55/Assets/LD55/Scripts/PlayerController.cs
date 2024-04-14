using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public LayerMask layers;
    public float Health;
    public Image healthBar;
    public List<SummonStateController> runes;
    public Image DeathPanel;
    public TextMeshProUGUI DeathText;
    public TextMeshProUGUI WinText;
    float maxHealth;

    bool healthRegen;

    int playerLevel;
    int oldLevel;
    bool dead;
    bool win;
    int bossDeathCoutner;

    int bossCounter;
    bool allBossSameTime;

    private void Start()
    {
        DeathPanel.color = new Color(1, 1, 1, 0);
        DeathText.color = new Color(1, 0, 0, 0);
        WinText.color = new Color(1, 0, 0, 0);

        maxHealth = Health;
        oldLevel = playerLevel = 1;
    }

    private void Update()
    {
        bossCounter = GameObject.FindObjectsOfType<BossController>().Count();
        if (bossCounter == 3)
        {
            allBossSameTime = true;
        }

        if (bossDeathCoutner >= 3 && !win)
        {
            win = true;
            StartCoroutine(Win());
        }

        if (win)
        {
            return;
        }

        healthBar.fillAmount = Health / maxHealth;

        //TODO: fix this
        var runeLevel = runes.Sum(s => s.SummonValuePercentage);
        playerLevel = 1 + (int)((runeLevel / 3f) * 10f);

        if (oldLevel != playerLevel)
        {
            oldLevel = playerLevel;
            LevelUp();
        }

        if (healthRegen)
        {
            Heal(1.2f * Time.deltaTime);
        }

        if (Health <= 0 || transform.position.y < -20)
        {
            if (!dead)
            {
                dead = true;
                StartCoroutine(Dying());
            }
        }
    }

    public void BossDied()
    {
        bossDeathCoutner++;
    }

    public void Heal(float value)
    {
        Health = (MathF.Min(Health + value, maxHealth));

    }

    IEnumerator Dying()
    {
        var t = 2f;
        while (t > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            t -= Time.deltaTime;
            DeathPanel.color = new Color(0, 0, 0, 1 - t / 2f);
            DeathText.color = new Color(1, 0, 0, 1 - t / 2f);
        }
        SceneManager.LoadScene("MenuScene");
    }

    IEnumerator Win()
    {
        var t = 9f;
        while (t > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            t -= Time.deltaTime;
            DeathPanel.color = new Color(0, 0, 0, 1 - t / 9f);
            WinText.color = new Color(1, 0, 0, 1 - t / 9f);
        }
        SceneManager.LoadScene("MenuScene");

        if (allBossSameTime)
        {
            PlayerPrefs.SetInt("Bonus", 1);
            PlayerPrefs.Save();
        }
    }

    public void TakeDamge(float value)
    {
        Health -= value;
        if (Health < 0)
        {
            //TODO: This is where i die.
        }
    }

    private void LevelUp()
    {
        if (playerLevel == 2)
        {
            GetComponent<PlayerMovementController>().BlinkEnabled = true;
        }
        if (playerLevel == 4 || playerLevel == 7)
        {
            GetComponent<PlayerAttackController>().BaseProjectileCount++;
        }
        if (playerLevel == 4)
        {
            GetComponent<PlayerAttackController>().SecondAttack = true;
        }
        if (playerLevel == 5)
        {
            GetComponent<PlayerMovementController>().MovementSpeed += 0.4f;
            //GetComponent<PlayerAttackController>().BaseAttackCooldown -= 0.05f;
        }
        if (playerLevel == 6)
        {
            GetComponent<PlayerMovementController>().BlinkCooldown -= 0.25f;
        }
        if (playerLevel == 8)
        {
            GetComponent<PlayerAttackController>().SecondaryAbility.BasePower *= 1.4f;
        }
        if (playerLevel == 9)
        {
            healthRegen = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<SummonStateController>()?.PlayerEnter();
        if (other.transform.parent != null)
        {
            if (other.transform.parent.GetComponent<ProjectileBaseController>() != null)
            {
                TakeDamge(other.transform.parent.GetComponent<ProjectileBaseController>().Damage);
                Destroy(other.gameObject.transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<SummonStateController>()?.PlayerLeave();
    }
}