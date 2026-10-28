
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public string enemyName;
        public GameObject prefab;

        [Min(1)]
        public int unlockWave = 1;

        [Min(0)]
        public int baseCount = 5;

        [Min(0)]
        public int extraPerWave = 1;

        public float baseSpeed = 3f;
    }

    [Header("Enemy Types")]
    public List<EnemyType> enemyTypes = new List<EnemyType>();

    [Header("Boss")]
    public GameObject bossPrefab;
    public int bossWave = 10;

    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Wave Settings")]
    public int totalWaves = 10;
    public float timeBetweenWaves = 5f;
    public float spawnInterval = 0.8f;

    [Header("Difficulty")]
    public float healthMultiplierPerWave = 0.15f;
    public float speedMultiplierPerWave = 0.05f;

    [Header("Player")]
    public int playerLives = 20;
    public int currentWave = 0;

    private int activeEnemies = 0;
    private bool isSpawning = false;

    private void Start()
    {
        StartCoroutine(StartWaves());
    }

    private IEnumerator StartWaves()
    {
        while (currentWave < totalWaves)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            currentWave++;

            Debug.Log("Starting Wave " + currentWave);

            yield return StartCoroutine(SpawnWave(currentWave));

            // Wacht tot alle enemies dood zijn of het einde bereiken.
            while (activeEnemies > 0)
                yield return null;

            Debug.Log("Wave " + currentWave + " completed!");
        }

        Debug.Log("ALL WAVES COMPLETED!");
    }

    private IEnumerator SpawnWave(int wave)
    {
        isSpawning = true;

        // Spawn alle unlocked enemy types.
        foreach (EnemyType enemyType in enemyTypes)
        {
            if (enemyType.prefab == null || wave < enemyType.unlockWave)
                continue;

            int count = enemyType.baseCount
                + (wave - enemyType.unlockWave)
                * enemyType.extraPerWave;

            for (int i = 0; i < count; i++)
            {
                SpawnEnemy(enemyType.prefab, wave, enemyType.baseSpeed);
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        // Boss verschijnt in de laatste wave, samen met normale enemies.
        if (wave == bossWave && bossPrefab != null)
        {
            SpawnEnemy(bossPrefab, wave, 2f);
        }

        isSpawning = false;
    }

    private void SpawnEnemy(GameObject prefab, int wave, float baseSpeed)
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to WaveManager!");
            return;
        }

        GameObject enemy = Instantiate(
            prefab,
            waypoints[0].position,
            Quaternion.identity
        );

        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();

        if (movement != null)
        {
            movement.SetWaypoints(waypoints);

            float speed = baseSpeed
                * (1f + (wave - 1) * speedMultiplierPerWave);

            movement.SetSpeed(speed);
        }

        if (health != null)
        {
            // Initialize past de HP aan op basis van de wave.
            // De multiplier wordt in EnemyHealth ingesteld.
            health.Initialize(this, wave);
        }

        activeEnemies++;
    }

    public void EnemyDefeated(EnemyHealth enemy)
    {
        activeEnemies = Mathf.Max(0, activeEnemies - 1);
        Debug.Log("Enemy defeated!");
    }

    public void EnemyReachedEnd(EnemyHealth enemy)
    {
        activeEnemies = Mathf.Max(0, activeEnemies - 1);

        playerLives -= enemy.damageToPlayer;

        Debug.Log("Enemy escaped! Lives remaining: " + playerLives);

        if (playerLives <= 0)
        {
            playerLives = 0;
            StopAllCoroutines();
            Debug.Log("GAME OVER!");
        }
    }
}
