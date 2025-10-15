using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using GWM = GameWorldManager;

public class Data_Item
{
    private int _key;
    private int _idSaveItem;

    public bool IsInInventory { get => _isInInventory; }
    private bool _isInInventory;

    public Prefab_Item PrefabItem { get =>  _prefabItem; }
    private Prefab_Item _prefabItem;

    public SO_Item SOItem { get; }
    private SO_Item _soItem;

    public float Condition { get => _condition; }
    private float _condition = 1;

    public ItemState State { get => _state; }
    private ItemState _state = ItemState.New;

    public Data_Item(SO_Item soItem, float condition, ItemState state)
    {
        OutPool(soItem, condition, state);
    }

    public bool SetSaveItem(int key, int id)
    {
        if (_key == 0)
        {
            _key = key;
            _idSaveItem = id;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SetUp(int key, float condition, ItemState state)
    {
        if (key == _key)
        {
            _condition = condition;
            _state = state;
            if (_prefabItem != null)
            {
                _prefabItem.MyAwake(this, _key);
            }
            return true;
        }
        else
        {
            Debug.LogError("Key not valid", _prefabItem.gameObject);
            return false;
        }
    }

    //New Game
    public int SetFromPrefabItem(Prefab_Item prefabItem)
    {
        if (_key == 0)
        {
            _prefabItem = prefabItem;
            S_SaveSystem.LockSaveItem(this);
            return _key;
        }
        return 0;
    }

    #region Pooling
    public bool InPool()
    {
        S_SaveSystem.UnlockSaveItem(_key, _idSaveItem);
        GWM.Instance.PoolManager.AddPrefabItemToPool(_prefabItem, _key);
        _key = 0;
        _idSaveItem = 0;
        _prefabItem = null;
        return true;
    }

    //Also after Instantiate (like a Start)
    public void OutPool(SO_Item soItem, float condition, ItemState state)
    {
        if (_key == 0)
        {
            _soItem = soItem;
            SetUp(0, condition, state);
            _prefabItem = GWM.Instance.PoolManager.RemovePrefabItemFromPool(_soItem);
            if (S_SaveSystem.HasALoading)
            {

            }
            else
            {
                S_SaveSystem.LockSaveItem(this);
                _prefabItem.MyAwake(this, _key);
            }
        }
    }
    #endregion
}
