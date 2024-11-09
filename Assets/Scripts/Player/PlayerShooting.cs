using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform _shootingPoint; // Точка, откуда выходит пуля
    [SerializeField] private BulletPool _bulletPool;
    private Vector2 _firePoint; // Точка, куда стреляет игрок

    private PlayerControls _controls;
    private Player _player;

    [SerializeField] private float _fireRate = 0.5f;
    private float _lastShotTime = 0f; // Время последнего выстрела

    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.Gameplay.ChangeFirePoint.performed += ctx => UpdateFirePoint(ctx.ReadValue<Vector2>());

        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void UpdateFirePoint(Vector2 newFirePoint)
    {
        _firePoint = newFirePoint;

        // Поворачиваем игрока к новой точке
        _player.RotateTowards(_firePoint);
    }

    private void Update()
    {
        // Если точка задана и прошло достаточно времени для нового выстрела
        if (_firePoint != Vector2.zero && Time.time >= _lastShotTime + _fireRate)
        {
            Shoot();
            _lastShotTime = Time.time;
        }
    }

    private void Shoot()
    {
        // Получаем направление, в котором смотрит игрок
        Vector2 direction = transform.right;

        // Получаем пулю из пула
        Bullet bullet = _bulletPool.GetBulletFromPool(_shootingPoint.position);
        bullet.SetDirection(direction);
    }
}
