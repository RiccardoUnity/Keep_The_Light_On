using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = (nameof(SO_ItemManager)), menuName = "Data/" + (nameof(SO_ItemManager)))]
public class SO_ItemManager : ScriptableObject
{
    [System.Serializable]
    private class CraftItem
    {
        public SO_Item SOItemToCraft { get => _soItemToCraft; }
        [SerializeField] private SO_Item _soItemToCraft;
        public SO_Item[] SOItemResources {  get => _soItemResources; }
        [SerializeField] private SO_Item[] _soItemResources;

        public void Craft(PlayerInventory playerInventory)
        {
            playerInventory.CraftItemInInventory(SOItemToCraft, SOItemResources);
        }
    }

    public SO_Item[] SOItems { get => _soItems; }
    [SerializeField] private SO_Item[] _soItems;

    [SerializeField] private CraftItem[] _craftItems;

    public SO_Item[] ArrayCraftItem()
    {
        SO_Item[] soItems = new SO_Item[_craftItems.Length];

        for (int i = 0; i < _craftItems.Length; ++i)
        {
            soItems[i] = _craftItems[i].SOItemToCraft;
        }
        return soItems;
    }

    public SO_Item[] ArrayCraftItemElement(int index) => _craftItems[index].SOItemResources;

    public void CraftItemInInventory (int index, PlayerInventory playerInventory)
    {
        _craftItems[index].Craft(playerInventory);
    }
}
