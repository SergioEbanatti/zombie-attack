using UnityEngine;

[RequireComponent(typeof(ZombieMovement))]
[RequireComponent(typeof(HealthComponent))]

public class Zombie : MonoBehaviour
{
    [Header("Параметры Зомби")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private int _damage = 1;

    [Header("Очки")]
    [SerializeField] private int _scoreValue = 7;

    private HealthComponent _damageHandler;

    #region Свойства
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = Mathf.Max(0, value); // Защита от отрицательных значений
    }

    public int Damage => _damage;

    #endregion

    private void Awake()
    {
        _damageHandler = GetComponent<HealthComponent>();
    }

    private void OnEnable()
    {
        _damageHandler.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        UnsubscribeFromDeathEvent();
    }


    private void OnDestroy()
    {
        UnsubscribeFromDeathEvent(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(_damage);
    }

    /// <summary>
    /// Отписка от события
    /// </summary>
    private void UnsubscribeFromDeathEvent()
    {
        // Отписка от события
        _damageHandler.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        AddScoreToManager();
        ZombieManager.Instance.OnZombieDied(); // Обработка смерти зомби
    }

    private void AddScoreToManager()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(_scoreValue);
        else
            Debug.LogWarning("ScoreManager не найден!");
    }

}
