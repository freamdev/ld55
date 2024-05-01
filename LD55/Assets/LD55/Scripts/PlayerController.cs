using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public LayerMask layers;
    public float Health;
    public Image healthBar;
    public TextMeshProUGUI HealthText;
    public List<SummonStateController> runes;
    public Image DeathPanel;
    public TextMeshProUGUI DeathText;
    public TextMeshProUGUI WinText;
    public GameObject PlayerDeathEffect;
    public GameObject PlayerModel;
    public GameObject LevelUpGfx;
    public AudioClip LevelUpSound;
    public AudioSource PlayerSource;
    public Slider ExpBar;
    public TextMeshProUGUI LevelText;

    float maxHealth;

    bool healthRegen;

    int playerLevel;
    int oldLevel;
    bool dead;
    bool win;
    int bossDeathCoutner;

    int bossCounter;
    bool allBossSameTime;
    bool isSurvival;

    public float Experience;

    public bool IsDead { get { return dead; } }

    private void Awake()
    {
        isSurvival = PlayerPrefs.GetString(PlayerPrefConsts.GAME_MODE, GameModeConsts.STORY) == GameModeConsts.ENDLESS;

        if (PlayerPrefs.HasKey(PlayerPrefConsts.BONUS))
        {
            var bonus = PlayerPrefs.GetInt(PlayerPrefConsts.BONUS, 1);
            if (bonus > 0)
            {
                PlayerModel.transform.Find("normalModel").gameObject.SetActive(false);
                PlayerModel.transform.Find("ascendedModel").gameObject.SetActive(true);
            }
        }
    }

    private void Start()
    {
        DeathPanel.color = new Color(1, 1, 1, 0);
        DeathText.color = new Color(1, 0, 0, 0);
        WinText.color = new Color(1, 0, 0, 0);

        maxHealth = Health;
        oldLevel = playerLevel = 1;
    }

    public void EnemyDied(float expValue)
    {
        Experience += expValue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }

        if (IsDead) return;

        bossCounter = GameObject.FindObjectsOfType<BossController>().Count();
        if (bossCounter == 3)
        {
            allBossSameTime = true;
        }

        if (bossDeathCoutner >= 3 && !win && !isSurvival)
        {
            win = true;
            StartCoroutine(Win());
        }

        if (win)
        {
            return;
        }

        healthBar.fillAmount = Health / maxHealth;
        HealthText.text = $"{Health:0}/{maxHealth:0}";

        if (!isSurvival)
        {
            Experience = runes.Sum(s => s.SummonValuePercentage);
        }

        var maxRuneLevel = 10f;

        playerLevel = 1 + (int)((Experience / 3f) * maxRuneLevel);

        var thisLevelNumber = (playerLevel - 1) / maxRuneLevel * 3f;
        var nextLevelNumber = (playerLevel) / maxRuneLevel * 3f;

        var percentage = (Experience - thisLevelNumber) / (nextLevelNumber - thisLevelNumber);
        ExpBar.value = percentage;

        if (oldLevel != playerLevel)
        {
            oldLevel = playerLevel;
            LevelUp();
        }
        LevelText.text = $"{playerLevel}";

        if (healthRegen)
        {
            Heal(1.2f * Time.deltaTime);
        }

        if (Health <= 0 || transform.position.y < -20)
        {
            if (!dead)
            {
                Instantiate(PlayerDeathEffect, transform);
                PlayerModel.GetComponent<Animator>().CrossFade("Die", 0.1f);
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
        if (isSurvival)
        {
            var score = ((int)Experience * 10) + GameObject.FindObjectOfType<EnemySpawner>().CurrentWave * 10;
            DeathText.text += $"\n\n Score: {score}";

            if (PlayerPrefs.HasKey(PlayerPrefConsts.ENDLESS_HIGHSCORE))
            {
                var times = JsonConvert.DeserializeObject<List<float>>(PlayerPrefs.GetString(PlayerPrefConsts.ENDLESS_HIGHSCORE));
                times.Add(score);
                times = times.OrderByDescending(x => x).Take(10).ToList();
                PlayerPrefs.SetString(PlayerPrefConsts.ENDLESS_HIGHSCORE, JsonConvert.SerializeObject(times));
            }
            else
            {
                var times = new List<float>()
                    {
                        score
                    };
                PlayerPrefs.SetString(PlayerPrefConsts.ENDLESS_HIGHSCORE, JsonConvert.SerializeObject(times));
            }
        }

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
        var time = Time.timeSinceLevelLoad;

        if (allBossSameTime)
        {
            time /= 2;
        }

        if (PlayerPrefs.HasKey(PlayerPrefConsts.STORY_HIGHSCORE))
        {
            var times = JsonConvert.DeserializeObject<List<float>>(PlayerPrefs.GetString(PlayerPrefConsts.STORY_HIGHSCORE));
            times.Add(time);
            times = times.OrderByDescending(x => x).Take(10).ToList();
            PlayerPrefs.SetString(PlayerPrefConsts.STORY_HIGHSCORE, JsonConvert.SerializeObject(times));
        }
        else
        {
            var times = new List<float>()
            {
                time
            };
            PlayerPrefs.SetString(PlayerPrefConsts.STORY_HIGHSCORE, JsonConvert.SerializeObject(times));
        }

        PlayerPrefs.Save();

        WinText.text += $"\n\nYour score: {(1000 - time):0}";

        var t = 9f;
        while (t > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            t -= Time.deltaTime;
            DeathPanel.color = new Color(0, 0, 0, 1 - t / 9f);
            WinText.color = new Color(1, 0, 0, 1 - t / 9f);
        }

        if (allBossSameTime)
        {
            PlayerPrefs.SetInt(PlayerPrefConsts.BONUS, 1);
            PlayerPrefs.Save();
        }

        SceneManager.LoadScene("MenuScene");
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
        var gfx = Instantiate(LevelUpGfx, transform);
        PlayerSource.clip = LevelUpSound;
        PlayerSource.Play();

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
            GetComponent<PlayerAttackController>().EnableDoubleFire = true;
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
        if(playerLevel >= 10 && isSurvival && playerLevel%2 == 0) 
        {
            maxHealth += 3;
            Heal(3);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Slash_fire_long"))
        {
            TakeDamge(68f * Time.deltaTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<SummonStateController>()?.PlayerLeave();
    }
}