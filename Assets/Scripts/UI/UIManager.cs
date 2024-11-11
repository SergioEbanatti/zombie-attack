using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _deathpanel;
    [SerializeField] private TextMeshProUGUI bestScoreText;  // Ссылка на UI текст для лучшего счёта
    [SerializeField] private TextMeshProUGUI currentScoreText;  // Ссылка на UI элемент для отображения текущего счёта

    private void OnEnable()
    {
        // Подписываемся на событие, если GameManager уже существует
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerStatusChanged += DeathPanelActivity;
        }

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
        ScoreManager.Instance.OnBestScoreChanged -= UpdateBestScoreUI;
        GameManager.Instance.OnPlayerStatusChanged -= DeathPanelActivity;
        ScoreManager.Instance.OnScoreChanged -= UpdateCurrentScoreUI;
    }

    // Метод для обновления текущего счёта на UI
    private void UpdateCurrentScoreUI()
    {
        int currentScore = ScoreManager.Instance.CurrentScore;
        currentScoreText.text = currentScore.ToString();
    }

    // Метод для обновления UI с лучшим счётом
    private void UpdateBestScoreUI()
    {
        // Получаем лучший счёт из ScoreManager и отображаем его
        int bestScore = ScoreManager.Instance.BestScore;
        bestScoreText.text = bestScore.ToString();
    }

    public void DeathPanelActivity()
    {
        if (_deathpanel != null)
        {
            bool currentActivity = _deathpanel.activeInHierarchy;
            _deathpanel.SetActive(!currentActivity);
        }

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameScene()
    {
        // Сброс текущего счета перед запуском игры
        ScoreManager.Instance.ResetScore();
        SceneManager.LoadScene(1);
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
