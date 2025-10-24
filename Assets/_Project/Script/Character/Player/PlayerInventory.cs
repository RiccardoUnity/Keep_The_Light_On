using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;
using Random = UnityEngine.Random;

public class PlayerInventory
{
    #region LikeSingleton
    private PlayerInventory() { }
    public static Key Key = new Key();
    public static PlayerInventory Instance(int key, out Func<bool, bool> myAwake)
    {
        if (key == Key.GetKey())
        {
            PlayerInventory playerInventory = new PlayerInventory();
            myAwake = playerInventory.MyAwake;
            return playerInventory;
        }
        myAwake = null;
        return null;
    }
    #endregion

    private bool _debug;
    private bool _isMyAwake;

    public bool CanViewOnPopUp { get => _isLoadingFinish; }
    private bool _isLoadingFinish;

    private PlayerManager _playerManager;
    public int TempKey { get; private set; }

    private int _countKeyItems;
    private Dictionary<int, Data_Item> _items = new Dictionary<int, Data_Item>();

    private float _playerRadius;

    public SO_Item[] SOItemInInventory { get => _soItemInInventory.ToArray(); }
    private List<SO_Item> _soItemInInventory = new List<SO_Item>();

    public event Action<int> onInventoryChange;

    private bool MyAwake(bool debug)
    {
        if (_isMyAwake)
        {
            return false;
        }
        else
        {
            _isMyAwake = true;
            _debug = debug;

            _playerManager = GWM.Instance.PlayerManager;
            _playerRadius = _playerManager.CapsuleCollider.radius;

            return true;
        }
    }

    public void EndLoading() => _isLoadingFinish = true;

    private int GenerateTempKey()
    {
        do
        {
            TempKey = Random.Range(int.MinValue, int.MinValue);
        }
        while (TempKey == 0);
        return TempKey;
    }

    public bool AddItemInventory(Data_Item datatItem)
    {
        if (datatItem.InInventory(GenerateTempKey()))
        {
            ++_countKeyItems;
            _items.Add(_countKeyItems, datatItem);

            bool hasItem = false;
            foreach (SO_Item soItem in _soItemInInventory)
            {
                if (soItem == datatItem.SOItem)
                {
                    hasItem = true;
                    break;
                }
            }
            if (!hasItem)
            {
                _soItemInInventory.Add(datatItem.SOItem);
            }

            onInventoryChange?.Invoke(_countKeyItems);
            TempKey = 0;
            if (_debug)
            {
                Debug.Log("Item add");
            }
            return true;
        }
        TempKey = 0;
        return false;
    }

    public bool RemoveItemInventory(int index, bool isDestroy)
    {
        if (_items[index].OutInventory(GenerateTempKey(), isDestroy))
        {
            if (isDestroy)
            {

            }
            else
            {
                Vector2 random = Random.insideUnitCircle * _playerRadius;
                _items[index].PrefabItem.transform.position = _playerManager.transform.position + new Vector3 (random.x, 0f, random.y);
            }

            bool hasItem = false;
            foreach (Data_Item dataItem in _items.Values)
            {
                if (dataItem.SOItem == _items[index].SOItem && dataItem != _items[index])
                {
                    hasItem = true;
                    break;
                }
            }
            if (!hasItem)
            {
                _soItemInInventory.Remove(_items[index].SOItem);
            }

            _items.Remove(index);
            onInventoryChange?.Invoke(-1);
            TempKey = 0;
            return true;
        }
        TempKey = 0;
        return false;
    }

    public List<int> GetKeyItems()
    {
        List<int> keys = new List<int>(_items.Count);
        foreach (int key in _items.Keys)
        {
            keys.Add(key);
        }
        return keys;
    }

    public SO_Item ViewInventoryItem(int index, out float condition, out ItemState state)
    {
        condition = _items[index].Condition;
        state = _items[index].State;
        return _items[index].SOItem;
    }

    public bool HasToolInInventory(ItemTool tool)
    {
        foreach (Data_Item dataItem in _items.Values)
        {
            if (dataItem.SOItem.ItemTool == tool)
            {
                return true;
            }
        }
        return false;
    }

    public void CraftItemInInventory(SO_Item craft, SO_Item[] removes)
    {
        int i;
        bool[] removesSOItem = new bool[removes.Length];
        int[] dataItemsKeyToRemove = new int[removes.Length];
        foreach (int dataItemKey in _items.Keys)
        {
            for(i = 0; i < removes.Length; ++i)
            {
                if (!removesSOItem[i] && _items[dataItemKey].SOItem == removes[i])
                {
                    dataItemsKeyToRemove[i] = dataItemKey;
                    removesSOItem[i] = true;
                }
            }
        }
        foreach(int key in dataItemsKeyToRemove)
        {
            RemoveItemInventory(key, true);
        }

        Data_Item dataItem = GWM.Instance.PoolManager.RemoveDataItemFromPool(craft, 1f, ItemState.New);
        AddItemInventory(dataItem);
    }
}
