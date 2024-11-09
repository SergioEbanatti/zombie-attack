using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(Zombie))]
public class ZombieMovement : MonoBehaviour
{
    private Transform _playerTransform;
    private bool _hasPlayer = false;  // Флаг для отслеживания, установлен ли игрок в инспекторе

    private Zombie _zombie;  // Ссылка на компонент Zombie
    private Vector3 _directionToPlayer; // Направление к игроку

    private void Start()
    {
        _zombie = GetComponent<Zombie>();

        // Получаем ссылку на игрока из GameManager
        if (GameManager.Instance != null && GameManager.Instance.PlayerTransform != null)
        {
            _playerTransform = GameManager.Instance.PlayerTransform;
            _hasPlayer = true;
        }
        else
            Debug.LogError("Игрок не зарегистрирован в GameManager");
    }

    private void Update()
    {
        if (_hasPlayer)
        {
            CalculateDirectionToPlayer();
            MoveTowardsPlayer();
            LookAtPlayer();
        }

    }


    private void CalculateDirectionToPlayer()
    {
        _directionToPlayer = (_playerTransform.position - transform.position).normalized;
    }

    private void MoveTowardsPlayer()
    {
        transform.position += _zombie.MoveSpeed * Time.deltaTime * _directionToPlayer;
    }

    private void LookAtPlayer()
    {
        float angle = Mathf.Atan2(_directionToPlayer.y, _directionToPlayer.x) * Mathf.Rad2Deg;

        // Применяем поворот (вращение по оси Z)
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }


}
