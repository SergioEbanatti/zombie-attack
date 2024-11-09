using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;     // Префаб пули с компонентом Bullet
    [SerializeField] private int _poolSize = 10;  // Размер пула
    private List<Bullet> bulletPool;

    // Синглтон для доступа к пулу пуль
    public static BulletPool Instance { get; private set; }

    private void Awake()
    {
        // Проверяем, есть ли уже экземпляр BulletPool
        if (Instance == null)
            Instance = this; // Устанавливаем этот объект как единственный экземпляр
        else
            Destroy(gameObject); // Удаляем второй экземпляр

        bulletPool = new List<Bullet>();

        // Инициализируем пул
        for (int i = 0; i < _poolSize; i++)
        {
            Bullet bullet = Instantiate(_bulletPrefab);  // Инстанцируем пулю
            bullet.gameObject.SetActive(false);         // Делаем пулю неактивной
            bulletPool.Add(bullet);                     // Добавляем в пул
        }
    }

    // Получаем пулю из пула
    public Bullet GetBulletFromPool(Vector2 position)
    {
        foreach (var bullet in bulletPool)
        {
            if (!bullet.gameObject.activeInHierarchy)  // Если пуля неактивна
            {
                bullet.transform.position = position;  // Устанавливаем её позицию
                bullet.gameObject.SetActive(true);      // Активируем пулю
                return bullet;
            }
        }

        // Если пул пуст, создаём новую пулю
        Bullet newBullet = Instantiate(_bulletPrefab, position, Quaternion.identity);
        bulletPool.Add(newBullet);  // Добавляем в пул
        return newBullet;
    }

    // Возвращаем пулю обратно в пул
    public void ReturnBulletToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
}
