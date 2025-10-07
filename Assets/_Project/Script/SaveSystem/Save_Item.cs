using Save;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static partial class S_SaveSystem
{
    [Serializable]
    private class Save_Item
    {
        public int Id { get => _id; }
        [SerializeField] private int _id;
        private static int _idCount;

        private Item _item;
        private int _key;

        [SerializeField] private Vector3Save _position;
        [SerializeField] private QuaternionSave _rotation;

        public SO_Item SO_Item { get => _soItem; }
        [SerializeField] private SO_Item _soItem;
        [SerializeField] private float _condition;
        [SerializeField] private ItemState _state;

        public Save_Item(bool loading, Item item = null)
        {
            ++_idCount;
            GenerateKey();
            if (loading)
            {

            }
            else
            {
                _id = _idCount;
                _item = item;
                _item.SetSaveItem(_key, _id);

                _soItem = _item.SOItem;
                Save();
            }
        }

        private void GenerateKey()
        {
            do
            {
                _key = Random.Range(int.MinValue, int.MaxValue);
            }
            while (_key != 0);
        }

        public void Save()
        {
            _position.Update(_item.transform.position);
            _rotation.Update(_item.transform.rotation);
            _condition = _item.Condition;
            _state = _item.State;
        }

        public void Load()
        {
            _item.transform.position = _position.Load();
            _item.transform.rotation = _rotation.Load();
            _item.LoadSaveItem(_key, _condition, _state);
        }

        public void SetItem(Item item)
        {
            if (_item == null && item.SetSaveItem(_key, _id))
            {
                item.LoadSaveItem(_key, _condition, _state);
            }
        }

        public void IntoPool(int key)
        {
            if(key == _key)
            {
                _item = null;
            }
        }
    }

    private static Queue<Item> _items = new Queue<Item>();
    private static List<Save_Item> _saveItems = new List<Save_Item>();
    private static Queue<Save_Item> _poolSaveItem = new Queue<Save_Item>();

    //In Awake New Game or Instantiate New Item
    public static bool AddItem(Item item)
    {
        if (_items.Contains(item))
        {
            return false;
        }
        else
        {
            _items.Enqueue(item);
            return true;
        }
    }

    //In Start New Game or Instantiate New Item
    public static void CheckSaveItem()
    {
        Item tempItem;
        Save_Item saveItem;
        while (_items.Count > 0)
        {
            tempItem = _items.Dequeue();
            if (_poolSaveItem.Count > 0)
            {
                saveItem = _poolSaveItem.Dequeue();
                saveItem.SetItem(tempItem);
            }
            else
            {
                saveItem = new Save_Item(false, tempItem);
            } 
            _saveItems.Add(saveItem);
        }
    }

    public static bool UnlockSaveItem(int key, int id)
    {
        foreach (Save_Item saveItem in _saveItems)
        {
            if (saveItem.Id == id)
            {
                saveItem.IntoPool(key);
                _poolSaveItem.Enqueue(saveItem);
                return true;
            }
        }
        return false;
    }

    //In Start Load Game
    public static void LoadItems()
    {
        Save_Item saveItem;
        while (_toLoadItems.Count > 0)
        {
            saveItem = _toLoadItems.Dequeue();
            Item item = GameWorldManager.Instance.PoolManager.RemoveItemFromPool(saveItem.SO_Item);
            saveItem.SetItem(item);
        }
        if (_debug)
        {
            Debug.Log("Save_Items loaded in scene");
        }
        _isLoadingComplete = false;
    }
}