using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Zombie))]
public class ZombieMovement : MonoBehaviour
{
    private Transform _playerTransform;
    private bool _hasPlayer;

    private Zombie _zombie;  // Ссылка на компонент Zombie
    private Vector3 _directionToPlayer; // Направление к игроку

    private void Start()
    {
        _zombie = GetComponent<Zombie>();

        // Получаем ссылку на игрока из GameManager
        if (GameManager.Instance != null && GameManager.Instance.PlayerTransform != null)
        {
            _playerTransform = GameManager.Instance.PlayerTransform;
            _hasPlayer = GameManager.Instance.HasPlayer;
            GameManager.Instance.OnPlayerStatusChanged += UpdatePlayerStatus; // Подписываемся на изменения состояния
        }
        else
            Debug.LogError("Игрок не зарегистрирован в GameManager");
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
        float angle = Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg;

        // Применяем поворот (вращение по оси Z)
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerStatusChanged -= UpdatePlayerStatus; // Отписываемся от события
    }


}
