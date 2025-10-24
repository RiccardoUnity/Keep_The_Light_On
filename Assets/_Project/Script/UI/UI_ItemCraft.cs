using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GWM = GameWorldManager;

public class UI_ItemCraft : MonoBehaviour
{
    private bool _isMyAwake;
    private int _index;
    private PlayerInventory _playerInventory;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _element0;
    [SerializeField] private TMP_Text _element1;
    [SerializeField] private TMP_Text _element2;
    [SerializeField] private TMP_Text _element3;
    [SerializeField] private UI_Button _button;

    [SerializeField] private Image _image;
    [SerializeField] private Sprite _iconLost;

    private Image _background;
    private Color _colorNormal;
    [SerializeField] private Color _colorHover = Color.white;

    private SO_Item[] _resources;

    public void MyAwake(int index)
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;
            _index = index;

            _playerInventory = GWM.Instance.PlayerManager.PlayerInventory;
            _background = GetComponent<Image>();
            _colorNormal = _background.color;

            _resources = GWM.Instance.SOItemManager.ArrayCraftItemElement(index);
            _element0.text = _resources[0].name;
            _element1.text = _resources[1].name;
            _element2.text = _resources[2].name;
            _element3.text = _resources[3].name;

            _button.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        if (_isMyAwake)
        {
            CheckSOItemInInventory();
        }
    }

    private void CheckSOItemInInventory()
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
        _button.gameObject.SetActive(true);
        foreach (bool value in check)
        {
            if (!value)
            {
                _button.gameObject.SetActive(false);
            }
        }
    }

    public void Craft()
    {
        GWM.Instance.SOItemManager.CraftItemInInventory(_index, _playerInventory);
        CheckSOItemInInventory();
    }
}
