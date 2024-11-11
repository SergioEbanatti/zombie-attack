using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _gameStarted = false;
    private bool _isGamePaused = false;
    private bool _hasPlayer = false;

    #region Свойства
    public static GameManager Instance { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public bool HasPlayer => _hasPlayer;
    public bool IsGameStarted => _gameStarted; 
    public bool IsGamePaused => _isGamePaused;
    #endregion

    public event Action OnPlayerStatusChanged;

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

        // Подписываемся на событие загрузки сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
            StartGame();
    }

    // Метод для регистрации игрока
    public void RegisterPlayer(Transform playerTransform)
    {
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
            SetGameTimeScale(1); // Запускаем время (игра начинается)
        }
    }

    // Метод для паузы игры
    public void PauseGame()
    {
        if (_gameStarted && !_isGamePaused)
        {
            _isGamePaused = true;
            SetGameTimeScale(0); // Останавливаем время
        }
    }

    // Метод для возобновления игры
    public void ResumeGame()
    {
        if (_gameStarted && _isGamePaused)
        {
            _isGamePaused = false;
            SetGameTimeScale(1); // Включаем нормальное время
        }
    }

    public void EndGame()
    {
        _hasPlayer = false;
        OnPlayerStatusChanged?.Invoke(); // Срабатывает, когда игрок умирает
        _gameStarted = false;
        SetGameTimeScale(0);
    }

    public void HandlePlayerDeath()
    {
        // Логика обработки смерти игрока
        Debug.Log("Игра завершена! Игрок погиб.");
    }

    private void SetGameTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
