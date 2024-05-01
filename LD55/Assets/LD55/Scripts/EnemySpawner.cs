using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();
    public float WaveLength;
    public float WaveIncrease = 3;
    public float MaxWaveLenght = 30;
    public GameObject MobPortal;
    public List<BossController> Bosses;
    public int BossWaves;
    public int BonusBossWave;
    public TextMeshProUGUI WaveText;

    int currentWave;
    float currentWaveLenght;
    bool spawnDone;
    bool isSurvival;

    public int CurrentWave => currentWave;

    private void Start()
    {
        isSurvival = PlayerPrefs.GetString(PlayerPrefConsts.GAME_MODE, GameModeConsts.STORY) == GameModeConsts.ENDLESS;
        spawnDone = false;
        currentWave = 1;
        StartCoroutine(SpawnWave());
        WaveText.text = "";
    }

    public void Update()
    {
        var enemies = GameObject.FindObjectsOfType<EnemyController>();
        if (!enemies.Any() && currentWaveLenght >= 3 && spawnDone)
        {
            currentWaveLenght = -999;
        }
    }



    IEnumerator SpawnWave()
    {
        var bossCount = 1;

        while (true)
        {
            spawnDone = false;
            var t = 3f;
            while (t > 0f)
            {
                if (isSurvival)
                {
                    WaveText.text = $"Wave: {currentWave}\nIn {t:0} seconds...";
                }
                t -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            WaveText.text = "";

            if (currentWave%BonusBossWave == 0)
            {
                bossCount++;
            }

            var relevantWaves = waves.Where(w => w.FirstWave <= currentWave && currentWave % w.Divider == 0).ToList();
            foreach (var wave in relevantWaves)
            {
                var numberToSpawn = Mathf.Min(wave.Count + ((currentWave / wave.FirstWave) - 1), wave.MaxCount);
                for (int i = 0; i < numberToSpawn; i++)
                {
                    StartCoroutine(SpawnMob(wave.Enemy));
                }
            }

            if (isSurvival && (currentWave % BossWaves == 0))
            {
                for (int i = 0; i < bossCount; i++)
                {
                    StartCoroutine(SpawnMob(Bosses.OrderBy(o => Guid.NewGuid()).First().GetComponent<EnemyController>()));
                    currentWaveLenght += 8f;
                }
            }

            currentWave++;
            WaveLength += WaveIncrease;
            currentWaveLenght = Mathf.Min(WaveLength, MaxWaveLenght);

            while (currentWaveLenght > 0)
            {
                currentWaveLenght -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }

    IEnumerator SpawnMob(EnemyController enemy)
    {
        var spawnPoint = new Vector3(Random.Range(-12, 12), 0, Random.Range(-12, 12));
        var portalInstance = Instantiate(MobPortal);
        portalInstance.transform.position = spawnPoint;
        portalInstance.transform.position += Vector3.up * 1.9f;
        portalInstance.transform.LookAt(GameObject.FindObjectOfType<PlayerController>().transform);
        yield return new WaitForSeconds(.4f);
        var instance = Instantiate(enemy);
        instance.transform.position = spawnPoint;
        spawnDone = true;
    }
}

[Serializable]
public class Wave
{
    public EnemyController Enemy;
    public int FirstWave;
    public int Divider;
    public int Count;
    public int MaxCount;
}