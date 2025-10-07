using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    #region LikeSingleton
    private PoolManager() { }

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

    private Dictionary<SO_Item, Queue<Item>> _allPool = new Dictionary<SO_Item, Queue<Item>>();

    public void MyStart()
    {

    }

    public void AddItemToPool(Item item)
    {
        if (_allPool.ContainsKey(item.SOItem))
        {
            _allPool[item.SOItem].Enqueue(item);
            item.InPool();
        }
        else
        {
            Queue<Item> newPool = new Queue<Item>();
            newPool.Enqueue(item);
            item.InPool();
            _allPool.Add(item.SOItem, newPool);
        }
    }

    public Item RemoveItemFromPool(SO_Item soItem)
    {
        if (_allPool.ContainsKey(soItem))
        {
            if (_allPool[soItem].Count == 0)
            {
                Object.Instantiate(soItem.Prefab);
            }
        }
        else
        {
            Object.Instantiate(soItem.Prefab);
        }
        Item item = _allPool[soItem].Dequeue();
        item.OutPool();
        return item;
    }
}