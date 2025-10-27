using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private UI_Craft _craft;
    [SerializeField] private RectTransform _statistics;

    [Header("Others")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _background2;
    public UI_Bed UIBed { get => _uiBed; }
    [SerializeField] private UI_Bed _uiBed;

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
            _craft.MyAwake();
            _uiBed.MyAwake();

            _background.gameObject.SetActive(true);
            _background2.gameObject.SetActive(true);
            _navigationButton.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        if (_isMyAwake)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameWorldManager.Instance.UIStats.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        if (_isMyAwake)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            DeactiveAllPanels();
            Stats();
            ResetNavigationButton();
            GameWorldManager.Instance.UIStats.gameObject.SetActive(true);
        }
    }

    public void DeactiveAllPanels()
    {
        _background.gameObject.SetActive(true);
        _background2.gameObject.SetActive(true);
        _navigationButton.gameObject.SetActive(true);
        _illnesses.gameObject.SetActive(false);
        _inventory.gameObject.SetActive(false);
        _craft.gameObject.SetActive(false);
        _statistics.gameObject.SetActive(false);
        _uiBed.gameObject.SetActive(false);
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

    public void OpenUIBed()
    {
        _background.gameObject.SetActive(false);
        _background2.gameObject.SetActive(false);
        _navigationButton.gameObject.SetActive(false);
        _illnesses.gameObject.SetActive(false);
        _uiBed.gameObject.SetActive(true);
    }

    public void CloseUIBed() => gameObject.SetActive(false);
}
