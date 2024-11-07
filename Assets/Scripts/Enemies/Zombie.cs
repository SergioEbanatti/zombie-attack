using UnityEngine;

public abstract class Zombie : MonoBehaviour
{
    [Header("Zombie Settings")]
    [SerializeField] protected float _moveSpeed = 2f; // Поле moveSpeed теперь protected
    [SerializeField] protected int _health = 10; // Поле health теперь protected

    #region Свойства
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    public int Health
    {
        get { return _health; } 
        set { _health = Mathf.Max(0, value); } // Устанавливаем значение здоровья, не позволяя отрицательных значений
    }

    #endregion

    // Абстрактные методы, которые должны быть реализованы в дочерних классах
    public abstract void TakeDamage(int damage);
    public abstract void Die();
}
