using UnityEngine;

[RequireComponent(typeof(ZombieMovement))]

public class Zombie : MonoBehaviour
{
    [Header("��������� �����")]
    [SerializeField] private float _moveSpeed = 2f;  
    [SerializeField] private int _health = 10; 

    [Header("����")]
    [SerializeField] private int scoreValue = 7;

    private ZombieMovement zombieMovement;

    #region ��������
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = Mathf.Max(0, value); } // ������ �� ������������� ��������
    }

    public int Health
    {
        get { return _health; }
        set { _health = Mathf.Max(0, value); } // ������ �� ������������� ��������
    }
    #endregion

    private void Awake()
    {
        zombieMovement = GetComponent<ZombieMovement>();
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject); 
    }
}
