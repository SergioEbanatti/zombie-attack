using UnityEngine;

[RequireComponent(typeof(ZombieMovement))]

public class Zombie : MonoBehaviour
{
    [Header("Параметры Зомби")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private int _damage = 1;

    [Header("Очки")]
    [SerializeField] private int _scoreValue = 7;

    private DamageHandler _damageHandler;

    #region Свойства
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = Mathf.Max(0, value); } // Защита от отрицательных значений
    }

    public int Damage => _damage;

    #endregion

    private void Awake()
    {
        _damageHandler = GetComponent<DamageHandler>(); // Получаем ссылку на DamageHandler
        _damageHandler.OnDeath += HandleDeath; // Подписываемся на событие смерти
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(_damage);
    }


    private void HandleDeath()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(_scoreValue);
        }
        else
        {
            Debug.LogWarning("ScoreManager не найден!");
        }
        ZombieManager.Instance.OnZombieDied(); // Обработка смерти зомби
    }

    private void OnDestroy()
    {
        //ScoreManager.Instance.AddScore(_scoreValue);
        _damageHandler.OnDeath -= HandleDeath; // Отписка от события, если объект уничтожен
    }
}
