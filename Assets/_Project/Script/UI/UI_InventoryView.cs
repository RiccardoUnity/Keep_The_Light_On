using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GWM = GameWorldManager;

public class UI_InventoryView : MonoBehaviour
{
    [SerializeField] private bool _debug;

    private bool _isMyAwake;
    private PlayerManager _playerManager;
    private PlayerInventory _playerInventory;
    private PoolManager _poolManager;

    [SerializeField] private UI_Item _uiItemPrefab;
    [SerializeField] private Transform _contenct;
    private Queue<UI_Item> _uiItemPool = new Queue<UI_Item>();
    private List<UI_Item> _uiItemInView = new List<UI_Item>();
    private int _startPool = 5;
    private List<int> _inventoryKeys = new List<int>();
    private int _keyItemSelect;
    private UI_Item _uiItem;

    [SerializeField] private RectTransform _detail;
    [SerializeField] private Image _detailIcon;
    [SerializeField] private Sprite _iconLost;
    [SerializeField] private UI_Button _buttonUse;

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
            _poolManager = GWM.Instance.PoolManager;

            UI_Item uiItem;
            int i;
            for (i = 0; i < _contenct.childCount; ++i)
            {
                uiItem = _contenct.GetChild(i).GetComponent<UI_Item>();
                uiItem.SetUI_InventoryView(this);
                uiItem.InPool();
                _uiItemPool.Enqueue(uiItem);
            }
            for (i = 0; i < _startPool; i++)
            {
                uiItem = Instantiate(_uiItemPrefab, _contenct);
                uiItem.SetUI_InventoryView(this);
                uiItem.InPool();
                _uiItemPool.Enqueue(uiItem);
            }

            _playerInventory.onInventoryChange += InventoryChange;
        }
    }

    void OnEnable()
    {
        if (_isMyAwake)
        {
            DrawView();
        }
    }

     void OnDisable()
    {
        if (_isMyAwake)
        {
            CancelView();
        }
    }

    private void InventoryChange(int key)
    {
        if (gameObject.activeSelf)
        {
            if (key == -1)
            {
                _uiItemInView.Remove(_uiItem);
                _uiItem.InPool();
                _uiItemPool.Enqueue(_uiItem);

                int index = _inventoryKeys.IndexOf(_keyItemSelect);
                _inventoryKeys.Remove(_keyItemSelect);
                if (_inventoryKeys.Count > 0)
                {
                    if (index < _inventoryKeys.Count)
                    {
                        
                    }
                    else
                    {
                        --index;
                    }
                    _keyItemSelect = _inventoryKeys[index];
                    _uiItemInView[index].ForceSelect();
                }
                else
                {
                    _keyItemSelect = 0;
                    _uiItem = null;
                    _detail.gameObject.SetActive(false);
                }
            }
            else
            {
                _inventoryKeys.Add(key);
                GenerateUIItem(key);
            }
        }
    }

    private void GenerateUIItem(int key)
    {
        UI_Item uiItem;
        SO_Item soItem;
        float condition;
        ItemState state;
        soItem = _playerInventory.ViewInventoryItem(key, out condition, out state);

        if (_uiItemPool.Count > 0)
        {
            uiItem = _uiItemPool.Dequeue();
            uiItem.OutPool();
        }
        else
        {
            uiItem = Instantiate(_uiItemPrefab, _contenct);
            uiItem.SetUI_InventoryView(this);
        }
        uiItem.SetUp(key, soItem, condition, state);
        _uiItemInView.Add(uiItem);
    }

    private void DrawView()
    {
        if (_debug)
        {
            Debug.Log("InventoryView open");
        }

        _inventoryKeys = _playerInventory.GetKeyItems();

        if (_inventoryKeys.Count > 0)
        {
            _detail.gameObject.SetActive(true);

            
            foreach (int key in _inventoryKeys)
            {
                GenerateUIItem(key);
            }

            if (_uiItemInView.Count > 0)
            {
                _uiItemInView[0].ForceSelect();
            }
        }
        else
        {
            _detail.gameObject.SetActive(false);
        }
    }

    private void CancelView()
    {
        int index = _uiItemInView.Count - 1;

        UI_Item uiItem;
        for (int i = index; i >= 0; --i)
        {
            uiItem = _uiItemInView[i];
            _uiItemInView.RemoveAt(i);
            uiItem.InPool();
            _uiItemPool.Enqueue(uiItem);
        }

        _inventoryKeys.Clear();

        _keyItemSelect = 0;
    }

    public void SelectItem(int keyItem, UI_Item uiItem)
    {
        if (_uiItem != uiItem)
        {
            if (_uiItem != null)
            {
                _uiItem.Deselect();
            }

            _keyItemSelect = keyItem;
            _uiItem = uiItem;
            SO_Item soItem;
            float condition;
            ItemState state;

            soItem = _playerInventory.ViewInventoryItem(_keyItemSelect, out condition, out state);
            if (soItem.Icon == null)
            {
                _detailIcon.sprite = _iconLost;
            }
            else
            {
                _detailIcon.sprite = soItem.Icon;
            }
            if (soItem.CanUse)
            {
                _buttonUse.gameObject.SetActive(true);
            }
            else
            {
                _buttonUse.gameObject.SetActive(false);
            }
        }
    }

    #region Details
    public void UseItemButton()
    {
        SO_Item soItem;
        float condition;
        ItemState state;
        soItem = _playerInventory.ViewInventoryItem(_keyItemSelect, out condition, out state);
        if (soItem.Use(_playerManager, GWM.Instance.TimeManager.RealSecondToGameSecond, condition, _poolManager))
        {
            _playerInventory.RemoveItemInventory(_keyItemSelect, true);
        }
        else
        {
            if (_debug)
            {
                Debug.Log("Can't use it without tool");
            }
        }
    }

    public void LeavesItemButton()
    {
        _playerInventory.RemoveItemInventory(_keyItemSelect, false);
    }
    #endregion
}
