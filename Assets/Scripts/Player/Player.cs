using UnityEngine;

public class Player : MonoBehaviour
{
    private DamageHandler _damageHandler;



    private void Awake()
    {
        _damageHandler = GetComponent<DamageHandler>(); // Получаем ссылку на DamageHandler
        _damageHandler.OnDeath += HandleDeath; // Подписываемся на событие смерти
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(TryRegisterPlayer), 0f, 0.1f);
    }

    private void TryRegisterPlayer()
    {
        // Проверяем, доступен ли GameManager и существует ли его Instance
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterPlayer(transform);
            CancelInvoke(nameof(TryRegisterPlayer)); // Прекращаем вызов после успешной регистрации
        }
    }

    private void RegisterPlayer()
    {
        GameManager.Instance.RegisterPlayer(transform);
    }

    // Метод для поворота игрока в сторону стрельбы
    public void RotateTowards(Vector2 targetPosition)
    {
        // Преобразуем координаты экрана в мировые координаты
        Vector3 worldTarget = Camera.main.ScreenToWorldPoint(new Vector3(targetPosition.x, targetPosition.y, Camera.main.nearClipPlane));

        Vector2 direction = (worldTarget - transform.position).normalized;

        // Вычисляем угол для поворота по оси Z
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void HandleDeath()
    {
        // Логика окончания игры
        GameManager.Instance.EndGame();
    }

    private void OnDestroy()
    {
        _damageHandler.OnDeath -= HandleDeath; // Отписка от события, если объект уничтожен
    }
}
