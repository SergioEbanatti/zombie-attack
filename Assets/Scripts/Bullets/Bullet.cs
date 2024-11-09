using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 _direction;
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int damage = 1; 

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
        Zombie zombie = collision.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombie.TakeDamage(damage);
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
