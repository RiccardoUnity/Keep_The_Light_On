using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = (nameof(SO_ItemManager)), menuName = "Data/" + (nameof(SO_ItemManager)))]
public class SO_ItemManager : ScriptableObject
{
    private bool _isMyAwake;

    public SO_Item[] SOItems { get => _soItems; }
    [SerializeField] private SO_Item[] _soItems;

    public bool MyAwake()
    {
        if (_isMyAwake)
        {
            return false;
        }
        else
        {
            _isMyAwake = true;

            return true;
        }
    }
}
