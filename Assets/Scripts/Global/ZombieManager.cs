using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    #region Свойства
    public static ZombieManager Instance { get; private set; }
    public int CurrentZombieCount { get; private set; }
    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OnZombieSpawned()
    {
        CurrentZombieCount++;
    }

    public void OnZombieDied()
    {
        if (CurrentZombieCount > 0)
            CurrentZombieCount--;
    }
}
