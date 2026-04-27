using Assets.Scripts.Utilities;
using System;
using TMPro;
using UnityEngine;

public class StopwatchUI : MonoBehaviour
{
    [SerializeField] private TMP_Text stopwatchUIText;

    private GameData gameData;
    void Start()
    {
        gameData = GameManager.Instance.GameData;
        gameData.OnTimeChanged += SetStopwatchText;
    }

    void SetStopwatchText(TimeSpan time)
    {
        stopwatchUIText.text = string.Format($"{time:mm\\:ss}");
    }
}
