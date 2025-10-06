using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = (nameof(SO_ItemManager)), menuName = "Data/" + (nameof(SO_ItemManager)))]
public class SO_ItemManager : ScriptableObject
{
    public SO_Item[] SOItems { get => _SOitems; }
    [SerializeField] private SO_Item[] _SOitems;


}
