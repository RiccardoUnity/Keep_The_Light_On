using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Slider : MonoBehaviour
{
    public Image Image { get => _image; }
    [SerializeField] private Image _image;
    public TMP_Text Label { get => _label; }
    [SerializeField] private TMP_Text _label;
    public Slider Slider { get => _slider; }
    [SerializeField] private Slider _slider;

    public void SetValue(float value)
    {
        value = Mathf.Clamp01(value);
        _slider.value = value;
    }
}