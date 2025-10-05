using UnityEngine;
using System;
using Save;

[Serializable]
public class Save_Item : Save_Transform
{
    //_id riferimento in una lista di ScriptableObject in cui sono raccolti tutti gli item
    [SerializeField] private int _id;

    public Save_Item(Transform transform, int id) : base(transform)
    {
        _id = id;
    }
}