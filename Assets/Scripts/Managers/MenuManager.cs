using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private TMP_Text maxScoreTMP;

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
        maxScoreTMP.text = $"Max score: {DataStorage.Instance.PlayerData.MaximumScore}";
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenUpgrades()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }
}
