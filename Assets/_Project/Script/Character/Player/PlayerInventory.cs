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

    private List<Data_Item> _items = new List<Data_Item>();

    public event Action onInventoryChange;

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
            for (int i = 0; i < _items.Count; ++i)
            {
                if (_items[i].SOItem.ItemType >= datatItem.SOItem.ItemType)
                {
                    if (datatItem.SOItem.ItemTool == ItemTool.None || datatItem.SOItem.ItemCampfire == ItemCampfire.None)
                    {
                        _items.Add(datatItem);
                        break;
                    }
                    else
                    {
                        if (_items[i].SOItem.ItemTool >= datatItem.SOItem.ItemTool)
                        {
                            _items.Add(datatItem);
                            break;
                        }
                        else if (_items[i].SOItem.ItemCampfire >= datatItem.SOItem.ItemCampfire)
                        {
                            _items.Add(datatItem);
                            break;
                        }
                    }
                }
            }

            onInventoryChange?.Invoke();
            TempKey = 0;
            return true;
        }
        TempKey = 0;
        return false;
    }

    public bool RemoveItemInventory(int index)
    {
        if (_items[index].OutInventory(GenerateTempKey()))
        {
            _items.RemoveAt(index);
            onInventoryChange?.Invoke();
            TempKey = 0;
            return true;
        }
        TempKey = 0;
        return false;
    }

    public int GetInventoryCount() => _items.Count;

    public SO_Item ViewInventoryItem(int index, out float condition, out ItemState state)
    {
        condition = _items[index].Condition;
        state = _items[index].State;
        return _items[index].SOItem;
    }
}
