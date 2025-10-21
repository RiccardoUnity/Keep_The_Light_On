using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public class UI_InventoryView : MonoBehaviour
{
    private bool _isMyAwake;
    private PlayerInventory _playerInventory;

    [SerializeField] private UI_Item _uiItemPrefab;
    private Queue<UI_Item> _uiItemPool = new Queue<UI_Item>();
    private Queue<UI_Item> _uiItemInView = new Queue<UI_Item>();
    private int _startPool = 5;
    private int _inventoryCount;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            _playerInventory = GWM.Instance.PlayerManager.PlayerInventory;
            _playerInventory.onInventoryChange += InventoryChange;

            UI_Item uiItem;
            for (int i = 0; i < _startPool; i++)
            {
                uiItem = Instantiate(_uiItemPrefab, transform.parent);
                _uiItemPool.Enqueue(uiItem);
            }
        }
    }

    void OnEnable()
    {
        DrawView();
    }

     void OnDisable()
    {
        CancelView();
    }

    private void InventoryChange()
    {
        if (gameObject.activeSelf)
        {
            DrawView();
        }
    }

    private void DrawView()
    {
        _inventoryCount = _playerInventory.GetInventoryCount();

        UI_Item uiItem;
        SO_Item soItem;
        float condition;
        ItemState state;
        for (int i = 0; i < _inventoryCount; ++i)
        {
            soItem = _playerInventory.ViewInventoryItem(i, out condition, out state);

            if (_uiItemPool.Count > 0)
            {
                uiItem = _uiItemPool.Dequeue();
                uiItem.OutPool();
            }
            else
            {
                uiItem = Instantiate(_uiItemPrefab, transform.parent);
            }
            _uiItemInView.Enqueue(uiItem);
            uiItem.SetUp(i, soItem, condition,state);
        }

        _inventoryCount = 0;
    }

    private void CancelView()
    {
        _inventoryCount = _uiItemInView.Count;

        UI_Item uiItem;
        for (int i = 0; i < _inventoryCount; ++i)
        {
            uiItem = _uiItemInView.Dequeue();
            uiItem.InPool();
            _uiItemPool.Enqueue(uiItem);
        }

        _inventoryCount = 0;
    }
}
