using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private int _keyItem;
    private bool _isSelect;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _condition;
    [SerializeField] private TMP_Text _state;
    [SerializeField] private TMP_Text _weight;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _iconLost;

    private Image _background;
    private Color _colorNormal;
    [SerializeField] private Color _colorHover = Color.white;
    [SerializeField] private Color _colorSelect = Color.green;

    private UI_InventoryView _uiInventoryView;

    void Awake()
    {
        _background = GetComponent<Image>();
        _colorNormal = _background.color;
    }

    public void SetUI_InventoryView(UI_InventoryView inventoryView) => _uiInventoryView = inventoryView;

    public void SetUp(int keyItem, SO_Item soItem, float condition, ItemState state)
    {
        _keyItem = keyItem;
        _name.text = soItem.Name;
        _condition.text = condition.ToString("F1");
        _state.text = state.ToString();
        _weight.text = soItem.Weight.ToString();

        if (soItem.Icon == null)
        {
            _image.sprite = _iconLost;
            _image.color = Color.red;
        }
        else
        {
            _image.sprite = soItem.Icon;
            _image.color = soItem.Color;
        }
    }

    #region Pooling
    public void InPool()
    {
        gameObject.SetActive(false);
    }

    public void OutPool()
    {
        gameObject.SetActive(true);
    }
    #endregion

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelect)
        {

        }
        else
        {
            _background.color = _colorHover;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelect)
        {

        }
        else
        {
            _background.color = _colorNormal;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ForceSelect();
    }

    public void ForceSelect()
    {
        _isSelect = true;
        _background.color = _colorSelect;
        _uiInventoryView.SelectItem(_keyItem, this);
    }

    public void Deselect()
    {
        _isSelect = false;
        _background.color = _colorNormal;
    }
}