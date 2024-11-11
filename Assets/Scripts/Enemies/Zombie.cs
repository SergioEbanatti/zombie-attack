using UnityEngine;

[RequireComponent(typeof(ZombieMovement))]

public class Zombie : MonoBehaviour
{
    [Header("��������� �����")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private int _damage = 1;

    [Header("����")]
    [SerializeField] private int _scoreValue = 7;

    private DamageHandler _damageHandler;

    #region ��������
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = Mathf.Max(0, value); } // ������ �� ������������� ��������
    }

    public int Damage => _damage;

    #endregion

    private void Awake()
    {
        _damageHandler = GetComponent<DamageHandler>(); // �������� ������ �� DamageHandler
        _damageHandler.OnDeath += HandleDeath; // ������������� �� ������� ������
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
            Debug.LogWarning("ScoreManager �� ������!");
        }
        ZombieManager.Instance.OnZombieDied(); // ��������� ������ �����
    }

    private void OnDestroy()
    {
        //ScoreManager.Instance.AddScore(_scoreValue);
        _damageHandler.OnDeath -= HandleDeath; // ������� �� �������, ���� ������ ���������
    }
}
