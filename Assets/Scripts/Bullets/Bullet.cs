using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int damage = 1;
    private Vector2 _direction;

    private void Update()
    {
        transform.position += _speed * Time.deltaTime * (Vector3)_direction;
    }

    public void SetDirection(Vector2 dir)
    {
        _direction = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
            BulletPool.Instance.ReturnBulletToPool(this);
        }
    }

    /// <summary>
    /// Метод, возвращающий пулю в пул, если она вылетела за границы экрана
    /// </summary>
    private void OnBecameInvisible()
    {
        BulletPool.Instance.ReturnBulletToPool(this);
    }
}
