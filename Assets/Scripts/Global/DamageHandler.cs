using System;
using UnityEngine;

public class DamageHandler : MonoBehaviour, IDamageable
{
    [SerializeField] private int _health = 100;
    public event Action OnDeath; // Событие, вызываемое при смерти

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        OnDeath?.Invoke(); // Вызываем событие смерти
        Destroy(gameObject);
    }
}
