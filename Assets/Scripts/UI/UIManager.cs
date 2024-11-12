using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private TextMeshProUGUI _currentScoreText;

    private void OnEnable()
    {
        // Подписываемся на событие, если GameManager уже существует
        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerStatusChanged += DeathPanelActivity;

        // Подписка на событие изменения лучшего счёта
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnBestScoreChanged += UpdateBestScoreUI;
            ScoreManager.Instance.OnScoreChanged += UpdateCurrentScoreUI;
        }
        // Обновляем лучший счёт, когда меню загружается
        UpdateBestScoreUI();
    }

    private void OnDisable()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.OnBestScoreChanged -= UpdateBestScoreUI;
            ScoreManager.Instance.OnScoreChanged -= UpdateCurrentScoreUI;
        }

        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerStatusChanged -= DeathPanelActivity;
    }

    // Метод для обновления текущего счёта
    private void UpdateCurrentScoreUI()
    {
        UpdateScoreUI(_currentScoreText, ScoreManager.Instance.CurrentScore);
    }

    // Метод для обновления лучшего счёта
    private void UpdateBestScoreUI()
    {
        UpdateScoreUI(_bestScoreText, ScoreManager.Instance.BestScore);
    }

    // Универсальный метод для обновления UI текста
    private void UpdateScoreUI(TextMeshProUGUI scoreText, int score)
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
        else
            Debug.LogWarning($"{scoreText.name} не назначен в инспекторе!");
    }

    public void DeathPanelActivity()
    {
        if (_deathPanel != null)
            _deathPanel.SetActive(!_deathPanel.activeSelf);
        else
            Debug.LogWarning("Панель смерти не назначена в инспекторе!");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameScene()
    {
        // Сброс текущего счета перед запуском игры
        SceneManager.LoadScene(1);
        ScoreManager.Instance.ResetScore();
    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public void OnResetBestScoreButtonClicked()
    {
        ScoreManager.Instance.ResetBestScore();
    }

}
