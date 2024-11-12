using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private int _damage = 1;
    private Vector2 _direction;

    private void Update()
    {
        Move();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(_damage);
            BulletPool.Instance.ReturnBulletToPool(this);
        }
    }

    private void Move()
    {
        transform.position += _speed * Time.deltaTime * (Vector3)_direction;
    }

    /// <summary>
    /// Метод, возвращающий пулю в пул, если она покидает пределы экрана
    /// </summary>
    private void OnBecameInvisible()
    {
        BulletPool.Instance.ReturnBulletToPool(this);
    }
}
