using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ImageIllnesses : MonoBehaviour
{
    private Image _image;
    [SerializeField] private Image _fillImage;
    public TMP_Text Text { get => _text; }
    [SerializeField] private TMP_Text _text;

    [SerializeField] private Color _highLightColorImage = Color.red;
    private Color _normalColorImage;

    public bool Active { get => _active; }
    private bool _active = true;

    void Awake()
    {
        _image = GetComponent<Image>();
        _normalColorImage = new Color(_normalColorImage.r, _normalColorImage.g, _normalColorImage.b, _image.color.a);
        OnExit();
    }

    void OnEnable()
    {
        OnExit();
    }

    public void SetActive(bool value)
    {
        _active = value;
    }

    public void OnEnter()
    {
        if (_active)
        {
            _image.color = _highLightColorImage;
        }
    }

    public void OnExit()
    {
        if (_active)
        {
            _image.color = _normalColorImage;
        }
    }

    public void OnHover(float value)
    {
        if (_active)
        {
            _image.color = Color.Lerp(_normalColorImage, _highLightColorImage, value);
        }
    }

    public void ChangeColor(Color color)
    {
        _normalColorImage = new Color(color.r, color.g, color.b, _normalColorImage.a);
        _highLightColorImage = new Color(color.r, color.g, color.b, _highLightColorImage.a);
    }

    public void FillImage(float value)
    {
        value = Mathf.Clamp01(value);
        _fillImage.fillAmount = value;
    }
}
