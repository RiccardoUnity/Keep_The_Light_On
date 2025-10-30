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

    public SO_Item SOItem { get => _soItem; }
    private SO_Item _soItem;

    public float Condition { get => _condition; }
    private float _condition = 1;

    public ItemState State { get => _state; }
    private ItemState _state = ItemState.New;

    private PlayerInventory _playerInventory;

    public Data_Item(SO_Item soItem, float condition, ItemState state, Prefab_Item prefabItem = null)
    {
        OutPool(soItem, condition, state, prefabItem);
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

    public bool SetUp(int key, float condition, ItemState state, Prefab_Item prefabItem = null)
    {
        if (_playerInventory == null)
        {
            _playerInventory = GWM.Instance.PlayerManager.PlayerInventory;
        }

        if (key == _key)
        {
            _condition = condition;
            _state = state;
            if (prefabItem == null)
            {
                _prefabItem = GWM.Instance.PoolManager.RemovePrefabItemFromPool(_soItem);
            }
            else
            {
                //New Game
                _prefabItem = prefabItem;
                S_SaveSystem.LockSaveItem(this);
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
        if (_prefabItem != null)
        {
            GWM.Instance.PoolManager.AddPrefabItemToPool(_prefabItem, _key);
        }
        _key = 0;
        _idSaveItem = 0;
        _prefabItem = null;
        return true;
    }

    //Also after Instantiate (like a Start)
    public void OutPool(SO_Item soItem, float condition, ItemState state, Prefab_Item prefabItem = null)
    {
        if (_key == 0)
        {
            _soItem = soItem;
            SetUp(0, condition, state, prefabItem);
            if (S_SaveSystem.HasALoading)
            {

            }
            else
            {

            }
        }
    }
    #endregion

    #region Inventory
    public bool InInventory(int playerKey)
    {
        if (playerKey == _playerInventory.TempKey)
        {
            if (_prefabItem !=  null)
            {
                GWM.Instance.PoolManager.AddPrefabItemToPool(_prefabItem, _key);
                _prefabItem = null;
            }
            _isInInventory = true;
            return true;
        }
        return false;
    }

    public bool OutInventory(int playerKey, bool isDestroy)
    {
        if (playerKey == _playerInventory.TempKey)
        {
            if (isDestroy)
            {
                GWM.Instance.PoolManager.AddDataItemToPool(this);
            }
            else
            {
                _isInInventory = false;
                _prefabItem = GWM.Instance.PoolManager.RemovePrefabItemFromPool(_soItem);
                _prefabItem.MyAwake(this, _key);
            }
            return true;
        }
        return false;
    }
    #endregion
}
