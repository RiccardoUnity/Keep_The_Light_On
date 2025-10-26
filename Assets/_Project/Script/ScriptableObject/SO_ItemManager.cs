using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = (nameof(SO_ItemManager)), menuName = "Data/" + (nameof(SO_ItemManager)))]
public class SO_ItemManager : ScriptableObject
{
    public SO_Item[] SOItems { get => _soItems; }
    [SerializeField] private SO_Item[] _soItems;

    public SO_ItemCraft[] SOItemCraft { get => _soItemCraft; }
    [SerializeField] private SO_ItemCraft[] _soItemCraft;
}
