using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Toggle : MonoBehaviour
{
    public Image Image { get => _image; }
    [SerializeField] private Image _image;
    public TMP_Text Label { get => _label; }
    [SerializeField] private TMP_Text _label;
    public Toggle Toggle { get => _toggle; }
    [SerializeField] private Toggle _toggle;

    public void SetValue(bool value) => _toggle.isOn = value;
}