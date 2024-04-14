using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();
    public int WaveLength;

    int currentWave;

    private void Start()
    {
        currentWave = 1;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (true)
        {
            var relevantWaves = waves.Where(w => w.FirstWave <= currentWave).ToList();
            foreach (var wave in relevantWaves)
            {
                var numberToSpawn = wave.Count + ((currentWave / wave.FirstWave) - 1);
                for(int i = 0; i < numberToSpawn; i++)
                {
                    var instance = Instantiate(wave.Enemy);
                    instance.transform.position = new Vector3(Random.Range(-12,12), 0, Random.Range(-12,12));
                }
            }

            currentWave++;
            yield return new WaitForSeconds(WaveLength);
        }
    }
}

[Serializable]
public class Wave
{
    public EnemyController Enemy;
    public int FirstWave;
    public int Count;
}