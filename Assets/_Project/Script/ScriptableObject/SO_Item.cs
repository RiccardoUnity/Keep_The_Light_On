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

    public float HarvestTime { get => _harvestTime; }
    [SerializeField] private float _harvestTime = 0.5f;

    public float Height { get => _height; }
    [SerializeField] private float _height = 0.1f;

    public ItemType ItemType { get => _itemType; }
    [SerializeField] private ItemType _itemType;

    public ItemTool ItemTool { get => _itemTool; }
    [SerializeField] private ItemTool _itemTool;

    public ItemCampfire ItemCampfire { get => _itemCampfire; }
    [SerializeField] private ItemCampfire _itemCampfire;
}
