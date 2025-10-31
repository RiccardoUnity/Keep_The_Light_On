using UnityEngine;
using GWM = GameWorldManager;

public class CactusDrop : Interactable
{
    private PlayerInventory _playerInventory;
    private ItemTool _itemToUse = ItemTool.Crowbar;
    [SerializeField] private SO_Item _soItemInside;
    [SerializeField] private int _cactusDrop = 2;
    [SerializeField] private int _realMinutesToWork = 45;
    [SerializeField] private float _realSecondAwait = 3;

    void Start()
    {
        _playerInventory = GWM.Instance.PlayerManager.PlayerInventory;
    }

    public void Check()
    {
        if (_playerInventory.HasToolInInventory(_itemToUse))
        {
            GWM.Instance.TimeManager.onEndAceleration += OnEndAcceleration;
            GWM.Instance.TimeManager.SetGamePlayAccelerate(_realMinutesToWork * 60f, _realSecondAwait);
        }
    }

    private void OnEndAcceleration()
    {
        GWM.Instance.TimeManager.onEndAceleration -= OnEndAcceleration;

        Data_Item item;
        for (int i = 0; i < _cactusDrop; ++i)
        {
            float condition = Random.value;
            item = GWM.Instance.PoolManager.RemoveDataItemFromPool(_soItemInside, condition, ItemState.New);
            item.PrefabItem.transform.position = transform.position + Random.insideUnitSphere;
            item.PrefabItem.Leaves();
        }

        gameObject.SetActive(false);
    }
}
