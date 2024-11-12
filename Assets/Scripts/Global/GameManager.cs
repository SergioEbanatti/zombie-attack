using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _gameStarted = false;
    private bool _hasPlayer = false;

    #region Свойства
    public static GameManager Instance { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public bool HasPlayer => _hasPlayer;
    public bool IsGameStarted => _gameStarted;
    #endregion

    public event Action OnPlayerStatusChanged;

    private const float GameTimeScale = 1f;
    private const float PausedTimeScale = 0f;

    private void Awake()
    {
        // Проверка на наличие единственного экземпляра GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Сохраняем объект при смене сцен
        }
        else
        Destroy(gameObject);

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Метод для регистрации игрока
    public void RegisterPlayer(Transform playerTransform)
    {
        if (_hasPlayer) return; // Игрок уже зарегистрирован
        PlayerTransform = playerTransform;
        _hasPlayer = true;
        OnPlayerStatusChanged?.Invoke(); // Срабатывает, когда игрок появляется
    }

    // Метод для начала игры
    public void StartGame()
    {
        if (!_gameStarted)
        {
            _gameStarted = true;
            SetGameTimeScale(GameTimeScale); // Запускаем время (игра начинается)
        }
    }

    public void EndGame()
    {
        _hasPlayer = false;
        _gameStarted = false;
        SetGameTimeScale(PausedTimeScale);
        OnPlayerStatusChanged?.Invoke();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
            StartGame();
    }

    private void SetGameTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
