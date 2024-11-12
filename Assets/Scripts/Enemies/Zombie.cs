using UnityEngine;

[RequireComponent(typeof(ZombieMovement))]
[RequireComponent(typeof(HealthComponent))]

public class Zombie : MonoBehaviour
{
    [Header("��������� �����")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private int _damage = 1;

    [Header("����")]
    [SerializeField] private int _scoreValue = 7;

    private HealthComponent _damageHandler;

    #region ��������
    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = Mathf.Max(0, value); // ������ �� ������������� ��������
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
    /// ������� �� �������
    /// </summary>
    private void UnsubscribeFromDeathEvent()
    {
        // ������� �� �������
        _damageHandler.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        AddScoreToManager();
        ZombieManager.Instance.OnZombieDied(); // ��������� ������ �����
    }

    private void AddScoreToManager()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(_scoreValue);
        else
            Debug.LogWarning("ScoreManager �� ������!");
    }

}
