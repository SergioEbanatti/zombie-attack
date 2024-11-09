using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool _gameStarted = false;
    private bool _isGamePaused = false;

    #region Свойства
    public static GameManager Instance { get; private set; }

    public Transform PlayerTransform { get; private set; }

    public bool IsGameStarted => _gameStarted; 
    public bool IsGamePaused => _isGamePaused;
    #endregion

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

    // Регистрация игрока
    public void RegisterPlayer(Transform playerTransform)
    {
        PlayerTransform = playerTransform;
        // При регистрации игрока, сообщаем его DeathHandler, что это именно игрок
        playerTransform.GetComponent<DeathHandler>().SetIsPlayer(true);
    }

    // Метод для начала игры
    public void StartGame()
    {
        if (!_gameStarted)
        {
            _gameStarted = true;
            Time.timeScale = 1; // Запускаем время (игра начинается)
        }
    }

    // Метод для паузы игры
    public void PauseGame()
    {
        if (_gameStarted && !_isGamePaused)
        {
            _isGamePaused = true;
            Time.timeScale = 0; // Останавливаем время
        }
    }

    // Метод для возобновления игры
    public void ResumeGame()
    {
        if (_gameStarted && _isGamePaused)
        {
            _isGamePaused = false;
            Time.timeScale = 1; // Включаем нормальное время
        }
    }

    public void EndGame()
    {
        _gameStarted = false;
        Time.timeScale = 0;
    }

    public void HandlePlayerDeath()
    {
        // Логика обработки смерти игрока
        Debug.Log("Игра завершена! Игрок погиб.");
    }
}
