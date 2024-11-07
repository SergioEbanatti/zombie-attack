using UnityEngine;

public abstract class Zombie : MonoBehaviour
{
    [Header("Zombie Settings")]
    [SerializeField] protected float _moveSpeed = 2f; // ���� moveSpeed ������ protected
    [SerializeField] protected int _health = 10; // ���� health ������ protected

    #region ��������
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    public int Health
    {
        get { return _health; } 
        set { _health = Mathf.Max(0, value); } // ������������� �������� ��������, �� �������� ������������� ��������
    }

    #endregion

    // ����������� ������, ������� ������ ���� ����������� � �������� �������
    public abstract void TakeDamage(int damage);
    public abstract void Die();
}
