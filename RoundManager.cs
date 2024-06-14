using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array de prefabs de enemigos
    public Transform[] spawnPoints; // Array de puntos de generación
    public int initialEnemyCount = 5; // Número inicial de enemigos por ronda

    public int enemiesAlive = 0; // Contador de enemigos vivos

    public static RoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(StartRound());
    }

    private IEnumerator StartRound()
    {
        GameData.Instance.NextRound();
        yield return new WaitForSeconds(1.5f);

        GameData.Instance.UpdateMultiplier();
        float multiplier = GameData.Instance.multiplier;
        int enemiesToSpawn = Mathf.Clamp(Mathf.RoundToInt(initialEnemyCount * multiplier), 5, 10);

        Debug.Log("Round: " + GameData.Instance.currentRound + ", Multiplier: " + multiplier + ", Enemies to Spawn: " + enemiesToSpawn);

        enemiesAlive = enemiesToSpawn;
        SpawnEnemies(enemiesToSpawn);
    }

    private void SpawnEnemies(int count)
    {
        List<int> usedSpawnPoints = new();

        for (int i = 0; i < count; i++)
        {
            int enemyIndex = Random.Range(0, enemyPrefabs.Length);

            int spawnPointIndex;
            do
            {
                spawnPointIndex = Random.Range(0, spawnPoints.Length);
            }
            while (usedSpawnPoints.Contains(spawnPointIndex));

            usedSpawnPoints.Add(spawnPointIndex);
            Transform spawnPoint = spawnPoints[spawnPointIndex];
            GameObject enemy = Instantiate(enemyPrefabs[enemyIndex], spawnPoint.position, spawnPoint.rotation);

            // Ajustar estadísticas del enemigo
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.AdjustStats(GameData.Instance.multiplier);
                enemyScript.OnEnemyDeath += HandleEnemyDeath;
                //Debug.Log("Spawned Enemy: " + enemy.name);
            }

            // Si se acaban los puntos de generación disponibles, reiniciamos la lista para permitir la reutilización
            if (usedSpawnPoints.Count == spawnPoints.Length)
            {
                usedSpawnPoints.Clear();
            }
        }
    }

    private void HandleEnemyDeath()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            StartCoroutine(StartRound());
        }
    }
}