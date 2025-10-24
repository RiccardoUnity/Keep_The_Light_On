using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    private bool _isMyAwake;

    [Header("NavigationButton")]
    [SerializeField] private GameObject _navigationButton;
    [SerializeField] private UI_Button _buttonIllnesses;
    [SerializeField] private UI_Button _buttonInventory;
    [SerializeField] private UI_Button _buttonCraft;
    [SerializeField] private UI_Button _buttonStatistics;
    private UI_Button[] _navigationUIButton;

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

            _navigationUIButton = new UI_Button[] { _buttonIllnesses, _buttonInventory, _buttonCraft, _buttonStatistics };
            DeactiveAllPanels();
            ResetNavigationButton();

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
        ResetNavigationButton();
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

    private void ResetNavigationButton()
    {
        foreach (UI_Button button in _navigationUIButton)
        {
            button.SetActive(true);
            button.OnExit();
        }
        _buttonIllnesses.OnEnter();
        _buttonIllnesses.OnHover(1f);
    }
}
