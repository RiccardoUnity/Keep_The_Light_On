using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour
{
    private int _index;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _condition;
    [SerializeField] private TMP_Text _state;
    [SerializeField] private TMP_Text _height;
    [SerializeField] private Image _image;

    

    public void SetUp(int index, SO_Item soItem, float condition, ItemState state)
    {
        _index = index;
        _name.text = soItem.name;
        _condition.text = condition.ToString("F1");
        _state.text = state.ToString();
        _height.text = soItem.Height.ToString();
        _image.sprite = soItem.Icon;
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
}