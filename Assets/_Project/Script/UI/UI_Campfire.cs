using TMPro;
using UnityEngine;
using GWM = GameWorldManager;

public class UI_Campfire : MonoBehaviour
{
    private bool _isMyAwake;

    [SerializeField] private RectTransform _noCampfire;
    [SerializeField] private RectTransform _campfireOff;
    [SerializeField] private RectTransform _campfireOn;

    [Header("CampfireOff")]
    [SerializeField] private UI_OtherButton _trigger;
    [SerializeField] private UI_OtherButton _fuse;
    [SerializeField] private UI_OtherButton _fuel;
    [SerializeField] private UI_Button _start;

    [Header("CampfireOn")]
    [SerializeField] private UI_OtherButton _fuelAdd;
    [SerializeField] private TMP_Text _addTime;
    [SerializeField] private TMP_Text _totalTime;
    [SerializeField] private UI_Button _add;

    private PlayerManager _playerManager;
    private PlayerInventory _playerInventory;
    private Campfire _campfire;

    private int[] _triggerKeys;
    private int _triggerIndexSelect;
    private int[] _fuseKeys;
    private int _fuseIndexSelect;
    private int[] _fuelKeys;
    private int _fuelIndexSelect;

    private string _noItem = "No Item";

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            _playerManager = GWM.Instance.PlayerManager;
            _playerInventory = _playerManager.PlayerInventory;
        }
    }

    void OnEnable()
    {
        if (_isMyAwake)
        {
            SetActiveGameObject();
        }
    }

    private void SetActiveGameObject()
    {
        if (_playerManager.Campfire == null)
        {
            _noCampfire.gameObject.SetActive(true);
            _campfireOff.gameObject.SetActive(false);
            _campfireOn.gameObject.SetActive(false);
        }
        else
        {
            _campfire = _playerManager.Campfire;
            if (_campfire.IsOn)
            {
                _noCampfire.gameObject.SetActive(false);
                _campfireOff.gameObject.SetActive(false);
                _campfireOn.gameObject.SetActive(true);
                SetUpCampfireOn();
            }
            else
            {
                _noCampfire.gameObject.SetActive(false);
                _campfireOff.gameObject.SetActive(true);
                _campfireOn.gameObject.SetActive(false);
                SetUpCampfireOff();
            }
        }
    }

    private void SetUpCampfireOn()
    {
        _fuelKeys = _playerInventory.GetItemCampfire(ItemCampfire.Fuel);
        SetIndexSelect(ref _fuelIndexSelect, ref _fuelKeys, ref _fuelAdd);

        if (_fuelIndexSelect != -1)
        {
            SO_Item soItem = _playerInventory.ViewInventoryItem(_fuelKeys[_fuelIndexSelect]);
            _addTime.text = $"{soItem.MinutesFuel.ToString("F1")} minutes";
            _add.gameObject.SetActive(true);
        }
        else
        {
            _addTime.text = "0 minutes";
            _add.gameObject.SetActive(false);
        }
    }

    private void UpdateCampfire(float timeDelay)
    {
        if (GWM.Instance.IsGamePause)
        {

        }
        else
        {
            if (_campfire.TotalMinutes > 0f)
            {
                _totalTime.text = $"{_campfire.TotalMinutes.ToString("F1")} minutes";
            }
            else
            {
                _totalTime.text = "0 minutes";
                GWM.Instance.TimeManager.onNotPriority1 -= UpdateCampfire;
            }
        }
    }

    private void SetUpCampfireOff()
    {
        _triggerKeys = _playerInventory.GetItemCampfire(ItemCampfire.Trigger);
        _fuseKeys = _playerInventory.GetItemCampfire(ItemCampfire.Fuse);
        _fuelKeys = _playerInventory.GetItemCampfire(ItemCampfire.Fuel);

        SetIndexSelect(ref _triggerIndexSelect, ref _triggerKeys, ref _trigger);
        SetIndexSelect(ref _fuseIndexSelect, ref _fuseKeys, ref _fuse);
        SetIndexSelect(ref _fuelIndexSelect, ref _fuelKeys, ref _fuel);

        if (_triggerKeys.Length == 0 || _fuseKeys.Length == 0 || _fuelKeys.Length == 0)
        {
            _start.gameObject.SetActive(false);
        }
        else
        {
            _start.gameObject.SetActive(true);
        }
    }

    private void SetIndexSelect(ref int indexSelect, ref int[] keys, ref UI_OtherButton button)
    {
        if (keys.Length > 0)
        {
            if (indexSelect >= keys.Length)
            {
                indexSelect = keys.Length - 1;
            }
            else if (indexSelect < 0)
            {
                indexSelect = 0;
            }
            button.Text.text = _playerInventory.GetNameInventoryItem(keys[indexSelect]);
            button.Less.gameObject.SetActive(true);
            button.Great.gameObject.SetActive(true);
        }
        else
        {
            indexSelect = -1;
            button.Text.text = _noItem;
            button.Less.gameObject.SetActive(false);
            button.Great.gameObject.SetActive(false);
        }
    }

    private string Select(ref int indexSelect, int[] keys, bool isNext)
    {
        if (isNext)
        {
            if (indexSelect == keys.Length - 1)
            {
                indexSelect = 0;
            }
            else
            {
                ++indexSelect;
            }
        }
        else
        {
            if (indexSelect == 0)
            {
                indexSelect = keys.Length - 1;
            }
            else
            {
                --indexSelect;
            }
        }
        return _playerInventory.GetNameInventoryItem(keys[indexSelect]);
    }

    public void SelectPreviewTrigger() => _trigger.Text.text = Select(ref _triggerIndexSelect, _triggerKeys, false);
    public void SelectNextTrigger() => _trigger.Text.text = Select(ref _triggerIndexSelect, _triggerKeys, true);
    public void SelectPreviewFuse() => _fuse.Text.text = Select(ref _fuseIndexSelect, _fuseKeys, false);
    public void SelectNextFuse() => _fuse.Text.text = Select(ref _fuseIndexSelect, _fuseKeys, true);
    public void SelectPreviewFuel() => _fuel.Text.text = Select(ref _fuelIndexSelect, _fuelKeys, false);
    public void SelectNextFuel() => _fuel.Text.text = Select(ref _fuelIndexSelect, _fuelKeys, true);

    public void SelectPreviewFuelAdd()
    {
        _fuelAdd.Text.text = Select(ref _fuelIndexSelect, _fuelKeys, false);
        _addTime.text = $"{_playerInventory.ViewInventoryItem(_fuelKeys[_fuelIndexSelect]).MinutesFuel.ToString("F1")} minutes";
    }
    public void SelectNextFuelAdd()
    {
        _fuelAdd.Text.text = Select(ref _fuelIndexSelect, _fuelKeys, true);
        _addTime.text = $"{_playerInventory.ViewInventoryItem(_fuelKeys[_fuelIndexSelect]).MinutesFuel.ToString("F1")} minutes";
    }

    private void ResizeArray(ref int indexSelect,ref int[] keys)
    {
        int[] resize = new int[keys.Length - 1];
        int removeKey = keys[indexSelect];
        int index = 0;
        foreach (int key in keys)
        {
            if (key != removeKey)
            {
                resize[index] = key;
                ++index;
            }
        }
        keys = resize;
    }

    public void StartCampfire()
    {
        _campfire.SetOn(_triggerKeys[_triggerIndexSelect], _fuseKeys[_fuseIndexSelect], _fuelKeys[_fuelIndexSelect]);
        
        ResizeArray(ref _triggerIndexSelect, ref _triggerKeys);
        ResizeArray(ref _fuseIndexSelect, ref _fuseKeys);
        ResizeArray(ref _fuelIndexSelect, ref _fuelKeys);

        SetIndexSelect(ref _triggerIndexSelect, ref _triggerKeys, ref _trigger);
        SetIndexSelect(ref _fuseIndexSelect, ref _fuseKeys, ref _fuse);
        SetIndexSelect(ref _fuelIndexSelect, ref _fuelKeys, ref _fuel);

        _noCampfire.gameObject.SetActive(false);
        _campfireOff.gameObject.SetActive(false);
        _campfireOn.gameObject.SetActive(true);

        SetUpCampfireOn();
        GWM.Instance.TimeManager.onNotPriority1 += UpdateCampfire;
    }

    public void AddFuelCampfire()
    {
        if (_fuelKeys.Length > 0)
        {
            _campfire.AddFuel(_fuelKeys[_fuelIndexSelect]);
            ResizeArray(ref _fuelIndexSelect, ref _fuelKeys);
            SetUpCampfireOn();
        }
    }
}
