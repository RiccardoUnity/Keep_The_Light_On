using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public class Campfire : Interactable
{
    public bool IsOn {  get; private set; }
    public float TotalMinutes { get; private set; }

    private PlayerInventory _playerInventory;

    [SerializeField] private GameObject[] _wood;
    [SerializeField] private ParticleSystem _fire;

    void Start()
    {
        _playerInventory = GWM.Instance.PlayerManager.PlayerInventory;
    }

    public void OpenUI()
    {
        GWM.Instance.UIInventory.gameObject.SetActive(true);
        GWM.Instance.UIInventory.Craft();
        GWM.Instance.UIStats.gameObject.SetActive(false);
    }

    public void SetOn(int keyTrigger, int keyFuse, int keyFuel)
    {
        AddFuel(keyFuel);
        IsOn = true;
        GWM.Instance.TimeManager.onNotPriority1 += UpdateNotPriority;

        _playerInventory.RemoveItemInventory(keyTrigger, true);
        _playerInventory.RemoveItemInventory(keyFuse, true);

        _fire.gameObject.SetActive(true);
        foreach (GameObject gameObject in _wood)
        {
            gameObject.SetActive(true);
        }
    }

    public void AddFuel(int keyFuel)
    {
        SO_Item soItem = _playerInventory.ViewInventoryItem(keyFuel);
        TotalMinutes += soItem.MinutesFuel;
        _playerInventory.RemoveItemInventory(keyFuel, true);
    }

    private void UpdateNotPriority(float timeDelay)
    {
        TotalMinutes -= timeDelay;
        if (TotalMinutes <= 0)
        {
            SetOff();
        }
    }

    private void SetOff()
    {
        IsOn = false;
        GWM.Instance.TimeManager.onNotPriority1 -= UpdateNotPriority;

        _fire.gameObject.SetActive(false);
        foreach (GameObject gameObject in _wood)
        {
            gameObject.SetActive(false);
        }
    }
}
