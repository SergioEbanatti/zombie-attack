using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform _shootingPoint;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField]
    [Tooltip("Скорострельность (выстрелов в секунду)")]
    private float _fireRate = 10f;
    private Vector2 _targetPoint;

    private PlayerControls _controls;
    private Player _player;
    private float _fireInterval;
    private float _lastShotTime;

    private void Awake()
    {
        _player = GetComponent<Player>();

        // Инициализация управления и подписка на обновление точки стрельбы
        _controls = new PlayerControls();
        _controls.Gameplay.ChangeFirePoint.performed += ctx => UpdateFirePoint(ctx.ReadValue<Vector2>());

        // Рассчитываем интервал между выстрелами
        CalculateFireInterval();
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _controls.Gameplay.Disable();
    }

    private void Update()
    {

        // Проверяем наличие ссылки на пул пуль на каждом кадре
        if (_bulletPool == null)
        {
            _bulletPool = FindObjectOfType<BulletPool>();
            if (_bulletPool == null)
                Debug.LogError("BulletPool не найден в сцене, повторная попытка подключения.");
        }

        // Проверяем, что точка стрельбы задана и прошло достаточно времени для нового выстрела
        if (_targetPoint != Vector2.zero && Time.time >= _lastShotTime + _fireInterval)
        {
            Shoot();
            _lastShotTime = Time.time;  // Обновляем время последнего выстрела
        }

    }

    /// <summary>
    /// Вычисляет интервал между выстрелами на основе скорострельности.
    /// </summary>
    private void CalculateFireInterval()
    {
        _fireInterval = 1f / _fireRate;
    }

    /// <summary>
    /// Обновляет точку стрельбы и поворачивает игрока к новой точке.
    /// </summary>
    private void UpdateFirePoint(Vector2 newFirePoint)
    {
        _targetPoint = newFirePoint;

        // Поворачиваем игрока к новой точке
        _player.RotateTowards(_targetPoint);
    }

    /// <summary>
    /// Выполняет выстрел пулей в заданном направлении.
    /// </summary>
    private void Shoot()
    {
        if (_bulletPool == null || _shootingPoint == null)
        {
            Debug.LogWarning("Не получается выстрелить: Отсутствуют обязательные компоненты.");
            return;
        }

        // Получаем направление, в котором смотрит игрок
        Vector2 direction = transform.right;

        // Получаем пулю из пула и задаём ей направление
        Bullet bullet = _bulletPool.GetBulletFromPool(_shootingPoint.position);
        bullet.SetDirection(direction);
    }
}
