using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;     // Префаб пули с компонентом Bullet
    [SerializeField] private int _poolSize = 10;       // Начальный размер пула пуль
    private List<Bullet> _bulletPool;                  // Список для хранения пуль в пуле

    public static BulletPool Instance { get; private set; }  // Синглтон для доступа к пулу

    private void Awake()
    {
        InitializeSingleton(); 
        InitializePool();
    }

    /// <summary>
    /// Получить пулю из пула, активируя неактивный объект.
    /// Если все объекты активны, создаётся новая пуля.
    /// </summary>
    /// <param name="position">Позиция, в которой должна появиться пуля</param>
    /// <returns>Ссылка на объект пули</returns>
    public Bullet GetBulletFromPool(Vector2 position)
    {
        // Ищем неактивную пулю в пуле
        foreach (var bullet in _bulletPool)
        {
            // Проверяем, что пуля неактивна и что ссылка на пулю всё ещё существует
            if (bullet != null && !bullet.gameObject.activeInHierarchy)
            {
                ActivateBullet(bullet, position);  // Активируем пулю и возвращаем её
                return bullet;
            }
        }

        // Если в пуле нет неактивных пуль, создаём новую и добавляем её в пул
        return CreateNewBullet(position);
    }


    /// <summary>
    /// Возвращает пулю обратно в пул, деактивируя её.
    /// </summary>
    /// <param name="bullet">Пуля, которую нужно вернуть в пул</param>
    public void ReturnBulletToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);

    }

    /// <summary>
    /// Настройка синглтона для объекта BulletPool.
    /// Удаляет дублирующий объект, если такой уже существует.
    /// </summary>
    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Инициализирует пул, создавая заданное количество пуль и добавляя их в список.
    /// </summary>
    private void InitializePool()
    {
        _bulletPool = new List<Bullet>();

        for (int i = 0; i < _poolSize; i++)
        {
            // Создаём пулю, деактивируем её и добавляем в пул
            var bullet = Instantiate(_bulletPrefab);
            bullet.gameObject.SetActive(false);
            _bulletPool.Add(bullet);
        }
    }

    /// <summary>
    /// Устанавливает пулю в нужное положение и активирует её.
    /// </summary>
    /// <param name="bullet">Пуля, которую нужно активировать</param>
    /// <param name="position">Позиция активации пули</param>
    private void ActivateBullet(Bullet bullet, Vector2 position)
    {
        bullet.transform.position = position;
        bullet.gameObject.SetActive(true);
    }

    /// <summary>
    /// Создаёт новую пулю и добавляет её в пул, если все объекты заняты.
    /// </summary>
    /// <param name="position">Позиция создания новой пули</param>
    /// <returns>Ссылка на созданную пулю</returns>
    private Bullet CreateNewBullet(Vector2 position)
    {
        var newBullet = Instantiate(_bulletPrefab, position, Quaternion.identity);
        _bulletPool.Add(newBullet);
        return newBullet;
    }

}
