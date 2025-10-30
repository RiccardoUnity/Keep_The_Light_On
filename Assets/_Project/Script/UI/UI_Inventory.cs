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
    public UI_Craft UICraft { get => _craft; }
    [SerializeField] private UI_Craft _craft;
    [SerializeField] private UI_Statistics _statistics;

    [Header("Others")]
    [SerializeField] private Image _background;
    [SerializeField] private Image _background2;
    public UI_Bed UIBed { get => _uiBed; }
    [SerializeField] private UI_Bed _uiBed;
    public UI_Start UIStart { get => _uiStart; }
    [SerializeField] private UI_Start _uiStart;

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
            _statistics.MyAwake();
            _uiBed.MyAwake();
            _uiStart.MyAwake();
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
        _uiStart.gameObject.SetActive(false);
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

    public void CloseUIInventory() => gameObject.SetActive(false);

    public void OpenUIStart()
    {
        _background.gameObject.SetActive(false);
        _background2.gameObject.SetActive(false);
        _navigationButton.gameObject.SetActive(false);
        _illnesses.gameObject.SetActive(false);
        _uiStart.gameObject.SetActive(true);
    }
}
