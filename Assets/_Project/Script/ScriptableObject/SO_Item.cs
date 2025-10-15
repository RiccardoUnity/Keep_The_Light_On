using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = (nameof(SO_Item)), menuName = "Data/" + (nameof(SO_Item)))]

public class SO_Item : ScriptableObject
{
    public string Name { get => _name; }
    [SerializeField] private string _name;

    public string Description { get => _description; }
    [SerializeField] [TextArea(2, 4)] private string _description;

    public Sprite Icon { get => _icon; }
    [SerializeField] private Sprite _icon;

    public Prefab_Item Prefab { get => _prefab; }
    [SerializeField] public Prefab_Item _prefab;
}
