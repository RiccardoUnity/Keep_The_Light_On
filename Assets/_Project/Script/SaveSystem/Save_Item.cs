using Save;
using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class S_SaveSystem
{
    [Serializable]
    private class Save_Item : Save_Transform
    {
        [SerializeField] private SO_Item _soItem;

        public Save_Item(Transform transform, SO_Item soItem) : base(transform)
        {
            _soItem = soItem;
        }

        public override void Save()
        {
            base.Save();

        }

        public override void Load()
        {
            base.Load();
        }
    }

    private static Queue<Item> _items = new Queue<Item>();

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

    public static bool LoadItems()
    {
        if (_loadingComplete)
        {

            if (_debug)
            {
                Debug.Log("Save_Items loaded in scene");
            }
            return true;
        }
        else
        {
            Debug.LogWarning("There isn't Save_Items to load");
            return false;
        }
    }
}