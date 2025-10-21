using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private bool _isMyAwake;

    [Header("NavigationButton")]
    [SerializeField] private GameObject _navigationButton;
    [SerializeField] private UI_Button _buttonStroke;
    [SerializeField] private UI_Button _buttonInventory;
    [SerializeField] private UI_Button _buttonCraft;
    [SerializeField] private UI_Button _buttonStatistics;

    [Header("Panels")]
    [SerializeField] private UI_Illnesses _illnesses;
    [SerializeField] private UI_InventoryView _inventory;
    [SerializeField] private RectTransform _craft;
    [SerializeField] private RectTransform _statistics;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            DeactiveAllPanels();
            Stats();

            _illnesses.MyAwake();
            _inventory.MyAwake();

            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Stats();
    }

    public void DeactiveAllPanels()
    {
        _illnesses.gameObject.SetActive(false);
        _inventory.gameObject.SetActive(false);
        _craft.gameObject.SetActive(false);
        _statistics.gameObject.SetActive(false);
    }

    public void Stats()
    {
        DeactiveAllPanels();
        _illnesses.gameObject.SetActive(true);
    }

    public void Inventory()
    {
        DeactiveAllPanels();
        _inventory.gameObject.SetActive(true);
    }

    public void Craft()
    {
        DeactiveAllPanels();
        _craft.gameObject.SetActive(true);
    }

    public void Statistics()
    {
        DeactiveAllPanels();
        _statistics.gameObject.SetActive(true);
    }

}
