using Assets.Scripts.Player;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Utilities;
using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private FieldManager fieldManager;
    [SerializeField] private MonstersManager monstersManager;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 playerSpawnPoint;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject endScreenUI;
    [SerializeField] private TMP_Text statsText;

    public GameData GameData { get; private set; } = new GameData();
    public GameObject Player { get; private set; }

    private bool _timeCounterActive;
    private GameSettingsSO gameSettingsSO;

    public Vector3 PlayerPosition 
    { 
        get 
        { 
            return Player.transform.position; 
        } 
    }

    public static GameManager Instance { get; private set; }

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

    void Start()
    {
        PrepareGame();
        SpawnPlayer();
        StartStopWatch();
        gameSettingsSO = DataStorage.Instance.GameSettings;
        GameData.OnTimeChanged += RecalculateExpAndDifficulty;
    }

    void Update()
    {
        if (_timeCounterActive)
        {
            GameData.AddTime(Time.deltaTime);
        }
    }

    public void StartStopWatch()
    {
        _timeCounterActive = true;
    }

    public void StopStopWatch()
    {
        _timeCounterActive = false;
    }

    void PrepareGame()
    {
        fieldManager.GenerateTerrain();
        fieldManager.BuildBorders();
        var confiner2d = virtualCamera.GetComponent<CinemachineConfiner2D>();
        confiner2d.m_BoundingShape2D = fieldManager.BuildCameraBounds();
        confiner2d.InvalidateCache();
        monstersManager.spawnPosition = fieldManager.CenterOfField;
    }

    void SpawnPlayer()
    {
        playerSpawnPoint = fieldManager.CenterOfField;
        Player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
        virtualCamera.Follow = Player.transform;
        monstersManager.target = Player.transform;
        monstersManager.centralObjectForSpawn = Player.transform;
    }

    void RecalculateExpAndDifficulty(TimeSpan timeFromStartOfGame)
    {
        float powerForHealthAndExpDrop = (float) timeFromStartOfGame.TotalSeconds / gameSettingsSO.TimeToUpDifficulty;

        monstersManager.SetExpDropAndDifficultyPower(powerForHealthAndExpDrop != 0 ? powerForHealthAndExpDrop : 1);
    }


    public void EndOfGame()
    {
        StopStopWatch();

        PlayerData playerData = DataStorage.Instance.PlayerData;

        statsText.text = $"You have killed {GameData.Score} monsters in {GameData.CurrentTime:hh\\:mm\\:ss}";

        if (GameData.Score >= playerData.MaximumScore)
        {
            playerData.MaximumScore = GameData.Score;
            playerData.DurationOfRunWithMaxScore = GameData.CurrentTime;
        }

        Time.timeScale = 0f;
        endScreenUI.SetActive(true);
    }

    public void ChangeSceneTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
