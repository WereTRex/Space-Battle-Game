using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public delegate void EnemyNumberChanged();
    public static event EnemyNumberChanged OnEnemyNumberChanged;


    [SerializeField] Vector2 mapBounds;

    [SerializeField] BaseEnemy[] enemyPrefabs;
    [SerializeField] List<GameObject> livingEnemies;

    [SerializeField] bool spawn;

    [Space(5)]
    
    [Header("Waves")]
    [SerializeField] bool useWaves;
    [SerializeField] int waveNumber;
    
    [SerializeField] float waveCooldown;
    float waveCooldownRemaining;

    private void Start()
    {
        waveNumber = 1;
        waveCooldownRemaining = waveCooldown;
    }

    private void Update()
    {
        if (spawn)
        {
            SpawnEnemies(1);
            spawn = false;
        }

        if (useWaves && livingEnemies.Count == 0)
        {
            if (waveCooldownRemaining > 0)
                waveCooldownRemaining -= 1 * Time.deltaTime;
            else
            {
                SpawnEnemies((2 * waveNumber) - 1);
                waveCooldownRemaining = waveCooldown;
            }
        }

        foreach (GameObject enemy in livingEnemies)
        {
            if (enemy == null) { livingEnemies.Remove(enemy); return; }
        }
    }

    public void SpawnEnemies(int _numberToSpawn)
    {
        for (int i = 0; i < _numberToSpawn; i++)
        {
            Vector3 pos = new Vector3(Random.Range(mapBounds.x, -mapBounds.x), Random.Range(mapBounds.y, -mapBounds.y), 12);
            GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length - 1)], pos, Quaternion.Euler(0, 0, 0)).gameObject;

            enemy.transform.parent = GameObject.Find("Blank Level/Rest Of Level").transform;
            livingEnemies.Add(enemy);
        }


        OnEnemyNumberChanged?.Invoke();
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(mapBounds.x, mapBounds.y, 0));
    }
}
