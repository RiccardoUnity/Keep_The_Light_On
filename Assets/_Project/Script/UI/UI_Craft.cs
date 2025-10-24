using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public class UI_Craft : MonoBehaviour
{
    private bool _isMyAwake;
    private PlayerInventory _playerInventory;

    [SerializeField] private UI_ItemCraft _uiItemCraftPrefab;
    [SerializeField] private Transform _contenct;

    private SO_Item[] _itemsToCraft;
    private UI_ItemCraft[] _uiItemsCraft;

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

            int i;
            if (_contenct.childCount > 0)
            {
                Transform child;
                for (i = 0; i < _contenct.childCount; ++i)
                {
                    child = _contenct.GetChild(i);
                    Destroy(child);
                }
            }
            _itemsToCraft = GWM.Instance.SOItemManager.ArrayCraftItem();
            _uiItemsCraft = new UI_ItemCraft[_itemsToCraft.Length];
            for (i = 0; i < _uiItemsCraft.Length; ++i)
            {
                _uiItemsCraft[i] = Instantiate(_uiItemCraftPrefab, _contenct);
                _uiItemsCraft[i].MyAwake(i);
            }
        }
    }
}
