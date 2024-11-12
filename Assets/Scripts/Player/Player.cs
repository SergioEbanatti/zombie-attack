using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class Player : MonoBehaviour
{
    private HealthComponent _healthComponent;

    private void Awake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnDeath += HandleDeath; // Подписываемся на событие смерти
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(TryRegisterPlayer), 0f, 0.1f); // Регистрация игрока в GameManager
    }


    private void OnDestroy()
    {
        if (_healthComponent != null)
            _healthComponent.OnDeath -= HandleDeath;
    }

    /// <summary>
    /// Пытается зарегистрировать игрока в GameManager.
    /// Останавливает повторный вызов после успешной регистрации.
    /// </summary>
    private void TryRegisterPlayer()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterPlayer(transform);
            CancelInvoke(nameof(TryRegisterPlayer));
        }
    }

    /// <summary>
    /// Поворачивает игрока в сторону цели, указанной координатами targetPosition.
    /// </summary>
    /// <param name="targetPosition">Целевая позиция на экране.</param>
    public void RotateTowards(Vector2 targetPosition)
    {
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera не найдена", this);
            return;
        }

        Vector3 worldTarget = Camera.main.ScreenToWorldPoint(new Vector3(targetPosition.x, targetPosition.y, Camera.main.nearClipPlane));
        Vector2 direction = (worldTarget - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    /// <summary>
    /// Обрабатывает событие смерти игрока, вызывая завершение игры.
    /// </summary>
    private void HandleDeath()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.EndGame();
    }

}
