using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{
    public SO_Item SOItem { get => _soItem; }
    [SerializeField] private SO_Item _soItem;

    void Awake()
    {
        S_SaveSystem.AddItem(this);
    }
}
