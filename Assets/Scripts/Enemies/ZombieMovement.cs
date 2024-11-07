using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class ZombieMovement : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform; // Ссылка на трансформ игрока
    private bool _hasPlayer = false; // Флаг для отслеживания, установлен ли игрок
    private Zombie _zombie; // Ссылка на компонент Zombie

    private void Start()
    {
        _zombie = GetComponent<Zombie>();

        if (_playerTransform != null)
            _hasPlayer = true;
        else
            Debug.LogError("Ссылка на игрока не задана в инспекторе.");
    }

    private void Update()
    {
        if (_hasPlayer)
            MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        // Находим вектор направления от зомби к игроку
        Vector3 direction = (_playerTransform.position - transform.position).normalized;

        // Перемещаем зомби по направлению к игроку с учетом его скорости
        transform.position += direction * _zombie.MoveSpeed * Time.deltaTime;
    }
}
