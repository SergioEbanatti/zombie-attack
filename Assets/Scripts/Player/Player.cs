using UnityEngine;

public class Player : MonoBehaviour
{
    private void Start()
    {
        // Регистрируем игрока в GameManager
        if (GameManager.Instance != null)
            GameManager.Instance.RegisterPlayer(transform);
    }

    // Метод для поворота игрока в сторону стрельбы
    public void RotateTowards(Vector2 targetPosition)
    {
        // Преобразуем координаты экрана в мировые координаты
        Vector3 worldTarget = Camera.main.ScreenToWorldPoint(new Vector3(targetPosition.x, targetPosition.y, Camera.main.nearClipPlane));

        Vector2 direction = (worldTarget - transform.position).normalized;

        // Вычисляем угол для поворота по оси Z
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
