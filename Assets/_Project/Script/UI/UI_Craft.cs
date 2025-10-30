using UnityEngine;
using GWM = GameWorldManager;

public class UI_Craft : MonoBehaviour
{
    private bool _isMyAwake;

    [SerializeField] private UI_ItemCraft _uiItemCraftPrefab;
    [SerializeField] private Transform _contenct;

    private UI_ItemCraft[] _uiItemsCraft;

    [SerializeField] private UI_Campfire _uiCampfire;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            int i;
            if (_contenct.childCount > 0)
            {
                Transform child;
                for (i = 0; i < _contenct.childCount; ++i)
                {
                    child = _contenct.GetChild(i);
                    Destroy(child.gameObject);
                }
            }
            _uiItemsCraft = new UI_ItemCraft[GWM.Instance.SOItemManager.SOItemCraft.Length];
            for (i = 0; i < _uiItemsCraft.Length; ++i)
            {
                _uiItemsCraft[i] = Instantiate(_uiItemCraftPrefab, _contenct);
                _uiItemsCraft[i].MyAwake(i, this);
            }

            _uiCampfire.MyAwake();
        }
    }

    void OnEnable()
    {
        if (_isMyAwake)
        {
            ReCheckAll();
        }
    }

    public void ReCheckAll()
    {
        foreach (UI_ItemCraft uiCraft in _uiItemsCraft)
        {
            uiCraft.CheckSOItemInInventory();
        }
    }
}
