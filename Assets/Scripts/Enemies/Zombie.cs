using UnityEngine;

[RequireComponent(typeof(ZombieMovement))]

public class Zombie : MonoBehaviour
{
    [Header("Параметры Зомби")]
    [SerializeField] private float _moveSpeed = 2f;  
    [SerializeField] private int _health = 10; 

    [Header("Очки")]
    [SerializeField] private int scoreValue = 7;

    #region Свойства
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = Mathf.Max(0, value); } // Защита от отрицательных значений
    }

    public int Health
    {
        get { return _health; }
        set { _health = Mathf.Max(0, value); } // Защита от отрицательных значений
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {

        
        if (collision.gameObject.TryGetComponent<DeathHandler>(out var deathHandler))
        {
            deathHandler.Die();
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
            Die();
    }

    private void Die()
    {
        ZombieManager.Instance.OnZombieDied();
        Destroy(gameObject);
    }
}
