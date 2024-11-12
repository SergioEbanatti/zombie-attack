using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class ZombieMovement : MonoBehaviour
{
    private Transform _playerTransform;
    private bool _hasPlayer;

    private Zombie _zombie;
    private Vector3 _directionToPlayer;

    private void Start()
    {
        _zombie = GetComponent<Zombie>();
    }

    private void OnEnable()
    {
        // Подписываемся на изменение состояния игрока
        if (GameManager.Instance != null)
        {
            _hasPlayer = GameManager.Instance.HasPlayer;
            _playerTransform = GameManager.Instance.PlayerTransform;
            GameManager.Instance.OnPlayerStatusChanged += UpdatePlayerStatus;
        }
        else
            Debug.LogError("Игрок не зарегистрирован в GameManager");
    }

    private void OnDisable()
    {
        UnsubscribeFromDeathEvent();
    }


    private void OnDestroy()
    {
        UnsubscribeFromDeathEvent();
    }



    private void Update()
    {
        if (_hasPlayer && _playerTransform != null)
        {
            CalculateDirectionToPlayer();
            MoveTowardsPlayer();
            LookAtPlayer();
        }

    }

    /// <summary>
    /// Отписка от события
    /// </summary>
    private void UnsubscribeFromDeathEvent()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerStatusChanged -= UpdatePlayerStatus; // Отписываемся от события
    }

    private void UpdatePlayerStatus()
    {
        _hasPlayer = GameManager.Instance.HasPlayer;
    }

    private void CalculateDirectionToPlayer()
    {
        if (_hasPlayer && _playerTransform != null)
            _directionToPlayer = (_playerTransform.position - transform.position).normalized;
    }

    private void MoveTowardsPlayer()
    {
        if (_hasPlayer && _playerTransform != null)
            transform.position += _zombie.MoveSpeed * Time.deltaTime * _directionToPlayer;
    }

    private void LookAtPlayer()
    {
        // Вычисляем угол поворота, чтобы смотреть на игрока
        if (_directionToPlayer.sqrMagnitude > 0.001f) // Предотвращаем малые значения
        {
            float angle = Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
