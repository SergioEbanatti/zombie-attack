using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuUI;  // ������� ����
    [SerializeField] private GameObject _gameUI;    // UI ����

    private void Start()
    {
        _mainMenuUI.SetActive(true); // ���������� ������� ����
        _gameUI.SetActive(false);  // �������� ������� ���������
    }

    // ����� ��� ������ ����
    public void StartGame()
    {
        _mainMenuUI.SetActive(false); // �������� ����
        _gameUI.SetActive(true);    // ���������� ������� ���������
    }
}
