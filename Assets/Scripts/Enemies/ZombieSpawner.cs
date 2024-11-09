using UnityEngine;

[System.Serializable]
public class ZombieSpawnInfo
{
    [SerializeField] private GameObject _zombiePrefab; // Префаб зомби
    [SerializeField]
    [Range(0, 100)]
    private float _chance; // Вероятность спавна (в процентах)

    #region Свойства
    public GameObject Prefab => _zombiePrefab;
    public float Chance => _chance;
    #endregion
}

public class ZombieSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ZombieSpawnInfo[] _zombieSpawnInfos; // Массив информации о спавне зомби

    [SerializeField]
    [Tooltip("Рекомендуемое значение: 10")]
    private int _maxZombies = 10;

    [SerializeField]
    [Tooltip("Рекомендуемое значение: 2")]
    private float _spawnInterval = 2f;

    [SerializeField]
    [Tooltip("Рекомендуемое значение: 0.1")]
    private float _decreasingSpawnInterval = 0.1f; //На сколько будет уменьшаться интервал спавна после каждого спавна

    [SerializeField]
    [Tooltip("Рекомендуемое значение: 1")]
    private float edgePadding = 1f;  // Смещение спавна от краев экрана

    private float[] _cumulativeChances; // Кумулятивный массив вероятностей
    private float _timeSinceLastSpawn = 0f; // Время с последнего спавна
    private Camera _mainCamera;

    public enum SpawnEdge { Left, Right, Top }

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        // Спавним зомби каждые _spawnInterval секунд, если на сцене их меньше максимального количества
        if (ZombieManager.Instance.CurrentZombieCount < _maxZombies)
        {
            _timeSinceLastSpawn += Time.deltaTime;
            if (_timeSinceLastSpawn >= _spawnInterval)
            {
                SpawnZombie();
                _timeSinceLastSpawn = 0f;
                _spawnInterval = Mathf.Max(0.5f, _spawnInterval - _decreasingSpawnInterval);
            }
        }
    }

    private void SpawnZombie()
    {
        GameObject zombie = GetZombieWithChance();
        SpawnEdge randomEdge = (SpawnEdge)Random.Range(0, 3);       //выбираем случайную сторону
        Vector3 spawnPosition = GetRandomSpawnPosition(randomEdge);

        Instantiate(zombie, spawnPosition, Quaternion.identity);
        // Уведомляем ZombieManager о спавне нового зомби
        ZombieManager.Instance.OnZombieSpawned();
    }

    /// <summary>
    /// Метод для получения случайной позиции для спавна
    /// </summary>
    /// <returns>Точка спавна</returns>
    private Vector3 GetRandomSpawnPosition(SpawnEdge spawnEdge)
    {
        float halfCameraHeight = _mainCamera.orthographicSize; 
        float halfCameraWidth = halfCameraHeight * _mainCamera.aspect;

        // Позиции на разных краях экрана
        return spawnEdge switch
        {
            SpawnEdge.Left => new Vector3(-halfCameraWidth - edgePadding, Random.Range(-halfCameraHeight, halfCameraHeight), 0f),
            SpawnEdge.Right => new Vector3(halfCameraWidth + edgePadding, Random.Range(-halfCameraHeight, halfCameraHeight), 0f),
            SpawnEdge.Top => new Vector3(Random.Range(-halfCameraWidth, halfCameraWidth), halfCameraHeight + edgePadding, 0f),
            _ => Vector3.zero,
        };
    }

    /// <summary>
    /// Выбирает случайный тип зомби
    /// </summary>
    /// <returns>Выбранный алгоритмом тип зомби</returns>
    private GameObject GetZombieWithChance()
    {
        var randomValue = Random.Range(0f, 1f);
        for (int i = 0; i < _cumulativeChances.Length; i++)
        {
            if (randomValue <= _cumulativeChances[i])
                return _zombieSpawnInfos[i].Prefab;
        }

        Debug.LogWarning("Произошла непредвиденная ситуация, будет призван первый тип зомби");
        return _zombieSpawnInfos[0].Prefab; // На случай непредвиденной ситуации
    }

    #region Группа методов срабатываемая в редакторе
    private void OnValidate()
    {
        ValidateSpawnChances();
        InitializeCumulativeChances();
    }

    /// <summary>
    /// Метод для вычисления общей суммы вероятностей на соответствие 100%
    /// </summary>
    private void ValidateSpawnChances()
    {
        var totalChance = 0f;
        foreach (var zombieInfo in _zombieSpawnInfos)
            totalChance += zombieInfo.Chance;

        if (Mathf.Abs(totalChance - 100f) > 0.01f)
            Debug.LogError("Сумма вероятностей должна быть равна 100%. Убедитесь, что это так.");
        else
            Debug.Log("Всё отлично! Сумма вероятностей ровно 100%");


    }

    /// <summary>
    /// Вычисление кумулятивных вероятностей для всех типов зомби
    /// </summary>
    private void InitializeCumulativeChances()
    {
        _cumulativeChances = new float[_zombieSpawnInfos.Length];
        var cumulative = 0f;

        for (int i = 0; i < _zombieSpawnInfos.Length; i++)
        {
            cumulative += _zombieSpawnInfos[i].Chance / 100f; // Переводим проценты в доли
            _cumulativeChances[i] = cumulative;
        }
    }
    #endregion
}
