using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    private TMP_Text _text;
    private Image _image;

    [Header("Text")]
    [SerializeField] private TMP_FontAsset _normal;
    [SerializeField] private TMP_FontAsset _highLight;
    [SerializeField] private AnimationCurve _scaleText;
    private Color _activeColorText;
    [SerializeField] private Color _deactiveColorText = Color.gray;

    [Header("Image")]
    [SerializeField] private Color _highLightColorImage = Color.red;
    private Color _normalColorImage;

    private bool _active = true;
    public UnityEvent onClick;

    void Awake()
    {
        _image = GetComponent<Image>();
        _normalColorImage = _image.color;
        _text = GetComponentInChildren<TMP_Text>();
        _activeColorText = _text.color;
    }

    void OnEnable()
    {
        OnExit();
        OnHover(0f);
    }

    public void SetActive(bool value)
    {
        _active = value;
        if (value)
        {
            _text.color = _activeColorText;
        }
        else
        {
            _text.color = _deactiveColorText;
        }
    }

    public void OnEnter()
    {
        if (_active)
        {
            _text.font = _highLight;
            _image.color = _highLightColorImage;
        }
    }

    public void OnExit()
    {
        if (_active)
        {
            _text.font = _normal;
            _image.color = _normalColorImage;
        }
    }

    public void OnHover(float value)
    {
        if (_active)
        {
            transform.localScale = Vector3.one * _scaleText.Evaluate(value);
            _image.color = Color.Lerp(_normalColorImage, _highLightColorImage, value);
        }
    }

    public void OnClick()
    {
        if (_active)
        {
            onClick?.Invoke();
        }
    }
}
