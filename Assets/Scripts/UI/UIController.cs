using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuUI;  // Главное меню
    [SerializeField] private GameObject _gameUI;    // UI игры

    private void Start()
    {
        _mainMenuUI.SetActive(true); // Показываем главное меню
        _gameUI.SetActive(false);  // Скрываем игровой интерфейс
    }

    // Метод для старта игры
    public void StartGame()
    {
        _mainMenuUI.SetActive(false); // Скрываем меню
        _gameUI.SetActive(true);    // Показываем игровой интерфейс
    }
}
