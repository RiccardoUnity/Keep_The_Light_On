using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    private TMP_Text _text;
    private Image _image;
    [SerializeField] private UI_Button[] _uiButtons;
    private UI_Button _button;

    [Header("Text")]
    [SerializeField] private TMP_FontAsset _normal;
    [SerializeField] private TMP_FontAsset _highLight;
    [SerializeField] private AnimationCurve _scaleText;
    private Color _activeColorText;
    [SerializeField] private Color _deactiveColorText = Color.gray;

    [Header("Image")]
    [SerializeField] private Color _highLightColorImage = Color.red;
    private Color _normalColorImage;

    public bool Active { get => _active; }
    private bool _active = true;
    public UnityEvent onClick;

    void Awake()
    {
        Reference();
    }

    private void Reference()
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
            if (_text == null)
            {
                Reference();
            }
            _text.color = _activeColorText;
        }
        else
        {
            if (_text == null)
            {
                Reference();
            }
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
        else
        {
            if (_button != null)
            {
                _button.OnExit();
                _button = null;
            }
        }
    }

    public void OnHover(float value)
    {
        if (_active)
        {
            _text.transform.localScale = Vector3.one * _scaleText.Evaluate(value);
            _image.color = Color.Lerp(_normalColorImage, _highLightColorImage, value);
        }
        else
        {
            if (_button != null)
            {
                _button.OnHover(value);
            }
        }
    }

    public void OnClick()
    {
        if (_active)
        {
            onClick?.Invoke();
            if (_uiButtons.Length > 0)
            {
                _active = false;
                foreach (UI_Button button in _uiButtons)
                {
                    if (!button.Active)
                    {
                        _button = button;
                    }
                    button.SetActive(true);
                }
            }
        }
    }
}
