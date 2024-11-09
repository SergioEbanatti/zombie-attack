using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    private GameManager _gameManager;
    private bool _isPlayer = false;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void SetIsPlayer(bool isPlayer)
    {
        _isPlayer = isPlayer;
    }

    // Функция смерти объекта
    public void Die()
    {
        Destroy(gameObject);

        if (_isPlayer)
            _gameManager.HandlePlayerDeath(); // Уведомляем менеджер игры об смерти игрока
    }
}
