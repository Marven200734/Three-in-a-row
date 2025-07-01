using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject winScreenPanel;
    [SerializeField] private GameObject loseScreenPanel;

    [Header("Game Dependencies")]
    [SerializeField] private ActionBar actionBar;
    [SerializeField] private ShapeCreator shapeCreator;

    private int _activeFigurinesOnField = 0;
    private bool _isGameActive = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        winScreenPanel?.SetActive(false);
        loseScreenPanel?.SetActive(false);
        _isGameActive = true;
    }

    private void OnEnable()
    {
        ActionBar.OnGameLose += HandleGameLose;
    }

    private void OnDisable()
    {
        ActionBar.OnGameLose -= HandleGameLose;
    }

    public void RegisterSpawnedFigurine()
    {
        if (!_isGameActive) return;
        _activeFigurinesOnField++;
    }

    public void RegisterCollectedFigurine()
    {
        Debug.Log("" +  _activeFigurinesOnField);
        if (!_isGameActive) return;

        _activeFigurinesOnField--;

        if (_activeFigurinesOnField <= 0)
        {
            if (actionBar != null && actionBar.IsGameOver())
            {
                return;
            }

            if (_isGameActive)
            {
                HandleGameWin();
            }
        }
    }

    private void HandleGameWin()
    {
        if (!_isGameActive) return;

        _isGameActive = false;
        winScreenPanel?.SetActive(true);
    }

    private void HandleGameLose()
    {
        if (!_isGameActive) return;

        _isGameActive = false;
        loseScreenPanel?.SetActive(true);
    }

    public bool IsGameActive()
    {
        return _isGameActive;
    }

    public void RestartGame()
    {
        _isGameActive = true;
        _activeFigurinesOnField = 0;

        winScreenPanel?.SetActive(false);
        loseScreenPanel?.SetActive(false);

        actionBar?.ResetBar();

        Figurine[] allFigurinesOnField = FindObjectsOfType<Figurine>();
        foreach (Figurine fig in allFigurinesOnField)
        {
            Destroy(fig.gameObject);
        }

        shapeCreator?.TriggerSpawning();
    }
}