using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GWM = GameWorldManager;

public class UI_ItemCraft : MonoBehaviour
{
    private bool _isMyAwake;
    private int _index;
    private PlayerManager _playerManager;
    private PlayerInventory _playerInventory;
    private UI_Craft _uiCraft;

    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _element0;
    [SerializeField] private TMP_Text _element1;
    [SerializeField] private TMP_Text _element2;
    [SerializeField] private TMP_Text _element3;
    [SerializeField] private TMP_Text _time;
    [SerializeField] private UI_Button _button;
    private TMP_Text[] _arrayElement;

    [SerializeField] private Image _image;
    [SerializeField] private Sprite _iconLost;

    private Color _colorNormal;
    [SerializeField] private Color _colorHover = Color.white;

    private SO_Item[] _resources;
    private bool _useCampfire;
    private float _realSecondsToCraft;

    public void MyAwake(int index, UI_Craft uiCraft)
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;
            _index = index;
            _uiCraft = uiCraft;

            _playerManager = GWM.Instance.PlayerManager;
            _playerInventory = _playerManager.PlayerInventory;
            _colorNormal = _background.color;

            _name.text = GWM.Instance.SOItemManager.SOItemCraft[index].SOItemToCraft[0].name;
            _icon.sprite = GWM.Instance.SOItemManager.SOItemCraft[index].SOItemToCraft[0].Icon;
            _icon.color = GWM.Instance.SOItemManager.SOItemCraft[index].SOItemToCraft[0].Color;

            _arrayElement = new TMP_Text[] { _element0, _element1, _element2, _element3 };
            _resources = GWM.Instance.SOItemManager.SOItemCraft[index].SOItemResources;
            for (int i = 0; i < _arrayElement.Length; ++i)
            {
                if (i < _resources.Length)
                {
                    _arrayElement[i].text = _resources[i].name;
                }
                else
                {
                    _arrayElement[i].gameObject.SetActive(false);
                }
            }
            _useCampfire = GWM.Instance.SOItemManager.SOItemCraft[index].UseCampfire;
            _realSecondsToCraft = GWM.Instance.SOItemManager.SOItemCraft[index].RealSecondsToCraft;
            _time.text = (_realSecondsToCraft / 60f).ToString("F0") + " Minutes";

            gameObject.SetActive(false);
        }
    }

    public void CheckSOItemInInventory()
    {
        bool canCraft;
        if (_useCampfire)
        {
            if (_playerManager.Campfire != null)
            {
                if (_playerManager.Campfire.IsOn)
                {
                    canCraft = true;
                }
                else
                {
                    canCraft = false;
                }
            }
            else
            {
                canCraft = false;
            }
        }
        else
        {
            canCraft = true;
        }

        if (canCraft)
        {
            bool[] check = new bool[_resources.Length];
            for (int i = 0; i < _resources.Length; ++i)
            {
                foreach (SO_Item soItemInInventory in _playerInventory.SOItemInInventory)
                {
                    if (_resources[i] == soItemInInventory)
                    {
                        check[i] = true;
                        break;
                    }
                }
            }

            bool active = true;
            foreach (bool value in check)
            {
                if (!value)
                {
                    active = false;
                }
            }
            gameObject.SetActive(active);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Craft()
    {
        GWM.Instance.TimeManager.onEndAceleration += RealCraft;
        GWM.Instance.TimeManager.SetGamePlayAccelerate(_realSecondsToCraft, 3f);
    }

    private void RealCraft()
    {
        GWM.Instance.TimeManager.onEndAceleration -= RealCraft;
        _playerInventory.CraftItemInInventory(GWM.Instance.SOItemManager.SOItemCraft[_index].SOItemToCraft, _resources);
        _uiCraft.ReCheckAll();
    }
}
