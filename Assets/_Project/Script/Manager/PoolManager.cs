using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public class PoolManager
{
    #region LikeSingleton
    private PoolManager()
    {
        if (GWM.Instance.MainDebug)
        {
            Debug.Log("PoolManager Initialized");
        }
    }

    public static PoolManager Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new PoolManager();
        }
        return null;
    }

    public static Key Key = new Key();
    #endregion

    private Dictionary<SO_Item, Queue<Prefab_Item>> _allPoolPrefabItem = new Dictionary<SO_Item, Queue<Prefab_Item>>();
    //private Dictionary<SO_Item, Queue<Data_Item>> _allPoolDataItem = new Dictionary<SO_Item, Queue<Data_Item>>();
    private Queue<Data_Item> _dataItems = new Queue<Data_Item>();

    public void MyStart()
    {

    }

    #region Pool Prefab_Item
    public void AddPrefabItemToPool(Prefab_Item prefabItem, int key)
    {
        if (_allPoolPrefabItem.ContainsKey(prefabItem.SOItem))
        {
            
        }
        else
        {
            AddNewPool(prefabItem.SOItem);
        }

        if (prefabItem.InPool(key))
        {
            _allPoolPrefabItem[prefabItem.SOItem].Enqueue(prefabItem);
        }
    }

    public Prefab_Item RemovePrefabItemFromPool(SO_Item soItem)
    {
        Prefab_Item prefabItem;
        if (_allPoolPrefabItem.ContainsKey(soItem))
        {
            if (_allPoolPrefabItem[soItem].Count > 0)
            {
                prefabItem = _allPoolPrefabItem[soItem].Dequeue();
            }
            else
            {
                prefabItem = Object.Instantiate(soItem.Prefab);
            }
        }
        else
        {
            AddNewPool(soItem);
            prefabItem = Object.Instantiate(soItem.Prefab);
        }
        prefabItem.OutPool();
        return prefabItem;
    }

    private void AddNewPool(SO_Item soItem)
    {
        Queue<Prefab_Item> newPool = new Queue<Prefab_Item>();
        _allPoolPrefabItem.Add(soItem, newPool);
    }
    #endregion

    #region Pool Data_Item
    public bool AddDataItemToPool(Data_Item dataItem)
    {
        if (dataItem.InPool())
        {
            _dataItems.Enqueue(dataItem);
            return true;
        }
        return false;
    }

    public Data_Item RemoveDataItemFromPool(SO_Item soItem, float condition, ItemState state)
    {
        Data_Item dataItem;
        if (_dataItems.Count > 0)
        {
            dataItem = _dataItems.Dequeue();
            dataItem.OutPool(soItem, condition, state);
        }
        else
        {
            dataItem = new Data_Item(soItem, condition, state);
        }
        return dataItem;
    }
    #endregion
}