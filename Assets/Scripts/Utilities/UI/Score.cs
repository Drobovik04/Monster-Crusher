using Assets.Scripts.Utilities;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private GameData gameData;
    void Start()
    {
        gameData = GameManager.Instance.GameData;
        gameData.OnScoreChanged += SetScore;
    }

    public void SetScore(int scoreToSet)
    {
        scoreText.text = $"Score: {scoreToSet}";
    }
}
