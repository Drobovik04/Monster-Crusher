using Assets.Scripts.Monster;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstersManager : MonoBehaviour
{
    public enum SpawnCenterType
    {
        Position,
        Player
    }

    [SerializeField] private int maxMonsterCount;
    [Tooltip("Spawn rate in a quantity in second")]
    [SerializeField] private float spawnRate; // кол-во в секунду
    [SerializeField] private List<GameObject> monsterPrefabs;
    [SerializeField] private List<GameObject> monsters;

    [Header("Spawn Settings")]
    [SerializeField] private SpawnCenterType spawnCenterType = SpawnCenterType.Position;
    public Vector3 spawnPosition;
    public Transform centralObjectForSpawn;
    [SerializeField] private float radius;
    [SerializeField] private float innerSafeRadius;
    [SerializeField] private bool spawning;
    [SerializeField] private FieldManager fieldManager;

    [Header("Target Settings")]
    [Tooltip("Target to move")]
    public Transform target;


    private float passedTime = 0;
    private System.Random rand = new System.Random();
    private GameData gameData;
    private GameSettingsSO gameSettingsSO;
    private float currentHealthEnemiesMultiplier;
    private float currentExperienceEnemiesMultiplier;
    private float currentMoveSpeedEnemiesMultiplier;

    public static MonstersManager Instance { get; private set; }
    public List<GameObject> Monsters => monsters;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        gameData = GameManager.Instance.GameData;
        gameSettingsSO = DataStorage.Instance.GameSettings;
        currentHealthEnemiesMultiplier = gameSettingsSO.HealthEnemiesMultiplier;
        currentExperienceEnemiesMultiplier = gameSettingsSO.ExperienceEnemiesMultiplier;
        currentMoveSpeedEnemiesMultiplier = gameSettingsSO.BasicMonstersMoveSpeedMultiplier;
    }

    void Update()
    {
        if (spawning && monsters.Count < maxMonsterCount && passedTime >= 1 / spawnRate)
        {
            SpawnMob();
            passedTime = 0;
        }

        passedTime += Time.deltaTime;
    }

    void SpawnMob()
    {
        GameObject monsterToSpawn = monsterPrefabs[rand.Next(monsterPrefabs.Count)];

        Vector3 spawnCenter = GetSpawnCenter();

        Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
        //float randomRadius = UnityEngine.Random.Range(innerSafeRadius, radius);
        //Vector3 offsetVector = new Vector3(randomDir.x, randomDir.y, 0) * randomRadius;
        //var newMob = Instantiate(monsterToSpawn, spawnCenter + offsetVector, Quaternion.identity);
        
        var newMob = Instantiate(monsterToSpawn, GetSafeSpawnPosition(), Quaternion.identity);

        MonsterController newMobController = newMob.GetComponent<MonsterController>();
        newMobController.SetTarget(target);
        newMobController.SetSpeedMultiplier(currentMoveSpeedEnemiesMultiplier);
        var enemyComponent = newMob.GetComponent<Enemy>();
        enemyComponent.SetExpDrop((int)(gameSettingsSO.BasicDropingExpereinceFromMonster * currentExperienceEnemiesMultiplier));
        enemyComponent.SetHealth(currentHealthEnemiesMultiplier * enemyComponent.Health);
        monsters.Add(newMob);
    }

    private Vector3 GetSafeSpawnPosition()
    {
        const int maxAttempts = 10;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            attempts++;

            Vector3 spawnCenter = GetSpawnCenter();

            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;

            float randomRadius = UnityEngine.Random.Range(innerSafeRadius, radius);

            Vector3 position = spawnCenter + new Vector3(randomDir.x, randomDir.y, 0) * randomRadius;

            position = fieldManager.ClampInsideField(position);

            if (Vector2.Distance(position, target.position) >= innerSafeRadius)
            {
                return position;
            }
        }

        return fieldManager.ClampInsideField(target.position + (Vector3)(UnityEngine.Random.insideUnitCircle.normalized * radius));
    }

    private Vector3 GetSpawnCenter()
    {
        switch (spawnCenterType)
        {
            case SpawnCenterType.Player:
                if (centralObjectForSpawn != null)
                {
                    return centralObjectForSpawn.position;
                }
                else
                {
                    Debug.LogWarning("Player target is not set! Using coordinates instead.");
                    return spawnPosition;
                }
            case SpawnCenterType.Position:
            default:
                return spawnPosition;
        }
    }

    public void SetExpDropAndDifficultyPower(float power)
    {
        currentExperienceEnemiesMultiplier = (float) Math.Pow(gameSettingsSO.ExperienceEnemiesMultiplier, power);
        currentHealthEnemiesMultiplier = (float) Math.Pow(gameSettingsSO.HealthEnemiesMultiplier, power);
        currentMoveSpeedEnemiesMultiplier = (float) Math.Pow(gameSettingsSO.MonsterMoveSpeedMultiplier, power);
    }

    public void RemoveMonster(GameObject monster)
    {
        if (monsters.Contains(monster))
        {
            monsters.Remove(monster);
        }
    }

    public void IncreaseKillCounterBy(int count)
    {
        gameData.Score += count;
    }

    public bool TryFindNearestMonsterToPoint(Transform point, out GameObject nearest)
    {
        List<GameObject> enemies = Instance.Monsters;
        nearest = null;

        if (enemies.Count == 0)
        {
            return false;
        }

        float minDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(enemy.transform.position, point.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }

        return true;
    }
}
