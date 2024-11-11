using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; }
    public int BestScore { get; private set; }

    public event Action OnScoreChanged;
    public event Action OnBestScoreChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBestScore();
        }
        else
            Destroy(gameObject);
    }

    public void AddScore(int points)
    {
        CurrentScore += points;
        OnScoreChanged?.Invoke();
        Debug.Log("Текущий счёт: " + CurrentScore);

        UpdateBestScore();
    }


    private void UpdateBestScore()
    {
        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
            OnBestScoreChanged?.Invoke();
            SaveBestScore();
        }
    }


    public void ResetScore()
    {
        CurrentScore = 0;
        OnScoreChanged?.Invoke();
        Debug.Log("Текуий счёт сброшен");
    }

    private void LoadBestScore()
    {
        BestScore = PlayerPrefs.GetInt("BestScore", 0);
        Debug.Log("Загружен лучший счёт: " + BestScore);
    }

    private void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", BestScore);
        PlayerPrefs.Save();
        Debug.Log("Лучший счёт сохранён: " + BestScore);
    }

    public void ResetBestScore()
    {
        PlayerPrefs.DeleteKey("BestScore"); // Удаляем лучший результат из PlayerPrefs
        BestScore = 0; // Сбрасываем локальное значение BestScore
        OnBestScoreChanged?.Invoke(); // Сигнализируем UI об изменении
        Debug.Log("Лучший счёт сброшен");
    }
}
