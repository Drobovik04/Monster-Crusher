using Assets.Scripts.ScriptableObjects;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public static DataStorage Instance { get; private set; }

    public PlayerData PlayerData;
    public GameSettingsSO GameSettings;
    public List<WeaponSettingsSO> WeaponsSettings;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }
    private void SaveData()
    {
        SaveSystem.Save(PlayerData);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void LoadData()
    {
        PlayerData data = SaveSystem.Load();
        if (data == null)
        {
            data = new PlayerData();
        }

        PlayerData = data;
    }
}
