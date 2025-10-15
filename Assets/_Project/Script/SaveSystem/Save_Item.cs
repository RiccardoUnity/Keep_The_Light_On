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

        private Data_Item _dataItem;
        private int _key;

        [SerializeField] private Vector3Save _position;
        [SerializeField] private QuaternionSave _rotation;

        public SO_Item SO_Item { get => _soItem; }
        [SerializeField] private SO_Item _soItem;
        [SerializeField] private float _condition;
        [SerializeField] private ItemState _state;

        public Save_Item(bool loading, Data_Item dataItem = null)
        {
            ++_idCount;
            GenerateKey();
            if (loading)
            {
                
            }
            else
            {
                _id = _idCount;
                if (dataItem != null)
                {
                    OutPool(dataItem);
                }
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
            _position.Update(_dataItem.PrefabItem.transform.position);
            _rotation.Update(_dataItem.PrefabItem.transform.rotation);
            _condition = _dataItem.Condition;
            _state = _dataItem.State;
        }

        public void Load()
        {
            _dataItem.PrefabItem.transform.position = _position.Load();
            _dataItem.PrefabItem.transform.rotation = _rotation.Load();
            _dataItem.SetUp(_key, _condition, _state);
        }

        public void LoadDataItem(Data_Item dataItem)
        {
            if (_dataItem == null && dataItem.SetSaveItem(_key, _id))
            {
                dataItem.SetUp(_key, _condition, _state);
            }
        }

        #region Pool
        public void InPool(int key)
        {
            if(key == _key)
            {
                _dataItem = null;
            }
        }

        public void OutPool(Data_Item dataItem)
        {
            if (_dataItem == null)
            {
                _dataItem = dataItem;
                _dataItem.SetSaveItem(_key, _id);
                _soItem = _dataItem.SOItem;
                Save();
            }
        }
        #endregion
    }

    private static List<Save_Item> _saveItems = new List<Save_Item>();
    private static Queue<Save_Item> _poolSaveItem = new Queue<Save_Item>();

    //From Data_Item
    public static void LockSaveItem(Data_Item dataItem)
    {
        Save_Item saveItem;
        if (_poolSaveItem.Count > 0)
        {
            saveItem = _poolSaveItem.Dequeue();
            saveItem.OutPool(dataItem);
        }
        else
        {
            saveItem = new Save_Item(false, dataItem);
        }
    }

    public static bool UnlockSaveItem(int key, int id)
    {
        foreach (Save_Item saveItem in _saveItems)
        {
            if (saveItem.Id == id)
            {
                saveItem.InPool(key);
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
            Data_Item dataItem = GameWorldManager.Instance.PoolManager.RemoveDataItemFromPool(saveItem.SO_Item, 1f, ItemState.New);
            saveItem.LoadDataItem(dataItem);
        }
        if (_debug)
        {
            Debug.Log("Save_Items loaded in scene");
        }

        _hasALoading = false;
    }
}