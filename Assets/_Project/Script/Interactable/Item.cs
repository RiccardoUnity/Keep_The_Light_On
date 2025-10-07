using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    private bool _hasAnError;
    private int _key;

    public SO_Item SOItem { get => _soItem; }
    [SerializeField] private SO_Item _soItem;

    public float Condition { get => _condition; }
    [Range(0f, 1f)][SerializeField] private float _condition = 1;
    [SerializeField] private bool _startWithRandomCondition = true;

    public ItemState State { get => _state; }
    private ItemState _state = ItemState.New;

    private int _idSaveItem;

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

    void Awake()
    {
        if (SOItem == null)
        {
            _hasAnError = true;
            Debug.Log("SO_Item missing", gameObject);
        }

        if (!_hasAnError)
        {
            if (S_SaveSystem.IsLoadingComplete)
            {
                GameWorldManager.Instance.PoolManager.AddItemToPool(this);
            }
            else
            {
                if (_startWithRandomCondition)
                {
                    do
                    {
                        _condition = Random.Range(0f, 1f);
                    }
                    while (_condition > 0f);
                }

                S_SaveSystem.AddItem(this);
            }
        }
    }

    public void InPool()
    {
        gameObject.SetActive(false);
        S_SaveSystem.UnlockSaveItem(_key, _idSaveItem);
        _key = 0;
    }

    public void OutPool()
    {
        gameObject.SetActive(true);
    }

    public bool LoadSaveItem(int key, float condition, ItemState state)
    {
        if (key == _key)
        {
            _condition = condition;
            _state = state;
            return true;
        }
        else
        {
            Debug.LogError("Key not valid", gameObject);
            return false;
        }
    }

    IEnumerator Start()
    {
        yield return null;
        if (_idSaveItem == 0)
        {
            S_SaveSystem.CheckSaveItem();
        }
    }
}
