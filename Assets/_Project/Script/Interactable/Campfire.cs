using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public class Campfire : Interactable
{
    private bool _isMyAwake;

    public bool IsOn {  get; private set; }
    public float TotalMinutes { get; private set; }

    private PlayerInventory _playerInventory;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            _playerInventory = GWM.Instance.PlayerManager.PlayerInventory;
        }
    }

    public void SetOn(int keyTrigger, int keyFuse, int keyFuel)
    {
        AddFuel(keyFuel);
        IsOn = true;
        GWM.Instance.TimeManager.onNotPriority1 += UpdateNotPriority;

        _playerInventory.RemoveItemInventory(keyTrigger, true);
        _playerInventory.RemoveItemInventory(keyFuse, true);
        _playerInventory.RemoveItemInventory(keyFuel, true);
    }

    public void AddFuel(int keyFuel)
    {
        SO_Item soItem = _playerInventory.ViewInventoryItem(keyFuel);
        TotalMinutes += soItem.MinutesFuel;
    }

    private void UpdateNotPriority(float timeDelay)
    {
        TotalMinutes -= timeDelay;
        if (TotalMinutes <= 0)
        {
            IsOn = false;
        }
    }
}
