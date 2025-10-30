using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GWM = GameWorldManager;

public class UI_Scope : MonoBehaviour
{
    [SerializeField] private Image _border;
    [SerializeField] private Image _mask;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _icon;

    private Color _alpha;
    private Color _iconColor;

    private bool _isMyAwake;
    private bool _isInAnimation;
    private bool _animationVerse;
    private float _currentTime;
    private IEnumerator _animation;
    private float _timeShowScope = 0.5f;
    private event Action _onAnimation;
    private event Action _onLateAnimation;

    private Data_Item _dataItem;
    private Action<Data_Item> _callbackDataItem;
    private Interactable _interactable;
    private Action<Interactable> _callbackInteractable;

    private bool _showScopeVerse;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            _iconColor = _icon.color;
            _alpha = _iconColor;
            _alpha.a = 0f;
            _icon.color = _alpha;

            _border.gameObject.SetActive(false);
            _mask.gameObject.SetActive(false);
            _fill.gameObject.SetActive(false);
            _icon.gameObject.SetActive(false);

            GWM.Instance.TimeManager.onPause += OnPause;
            GWM.Instance.TimeManager.onResume += OnResume;
        }
    }

    private void OnPause() => gameObject.SetActive(false);
    private void OnResume() => gameObject.SetActive(true);

    public void StartShowScope(bool isShowScope)
    {
        _showScopeVerse = isShowScope;
        if (_animation == null)
        {
            SecureStartShowScope();
        }
        //If the animation hasn't finished
        else
        {
            _onLateAnimation += SecureStartShowScope;
        }
    }

    private void SecureStartShowScope()
    {
        _animationVerse = _showScopeVerse;
        _currentTime = _showScopeVerse ? 0f : 1f;
        _icon.gameObject.SetActive(true);
        _onAnimation = AnimationShowScope;
        _onLateAnimation = IconOff;
        _animation = Animation(_timeShowScope);
        StartCoroutine(_animation);
    }

    private void AnimationShowScope() => _icon.color = Color.Lerp(_alpha, _iconColor, _currentTime);
    private void IconOff()
    {
        if (!_animationVerse)
        {
            _icon.gameObject.SetActive(false);
        }
    }

    public void StartSelect(Data_Item dataItem, Action<Data_Item> callback)
    {
        _dataItem = dataItem;
        _callbackDataItem = callback;
        if (_animation == null)
        {
            SecureStartSelect();
        }
        //If the animation hasn't finished
        else
        {
            _onLateAnimation += SecureStartSelect;
        }
    }

    public void StartSelect(Interactable interactable, Action<Interactable> callback)
    {
        _interactable = interactable;
        _callbackInteractable = callback;
        if (_animation == null)
        {
            SecureStartSelect();
        }
        //If the animation hasn't finished
        else
        {
            _onLateAnimation += SecureStartSelect;
        }
    }

    private void SecureStartSelect()
    {
        _border.gameObject.SetActive(true);
        _mask.gameObject.SetActive(true);
        _fill.gameObject.SetActive(true);
        _fill.fillAmount = 0f;
        _animationVerse = true;
        _currentTime = 0f;
        _onAnimation = SetFill;
        _onLateAnimation = CallbackReference;
        _onLateAnimation += EndSelect;
        if (_dataItem != null)
        {
            _animation = Animation(_dataItem.SOItem.HarvestTime);
        }
        else
        {
            _animation = Animation(0.5f);
        }
        StartCoroutine(_animation);
    }

    private void CallbackReference()
    {
        if (_dataItem != null)
        {
            _callbackDataItem?.Invoke(_dataItem);
        }
        else if (_interactable != null)
        {
            _callbackInteractable?.Invoke(_interactable);
        }
    }

    private void SetFill()
    {
        _fill.fillAmount = _currentTime;
        if (_dataItem != null)
        {
            if (GWM.Instance.PlayerManager.TrySelectInteractable || GWM.Instance.PlayerManager.TryUseInteractable)
            {
                _animationVerse = true;
            }
            else
            {
                _animationVerse = false;
            }
        }
        else
        {
            if (GWM.Instance.PlayerManager.TryUseInteractable)
            {
                _animationVerse = true;
            }
            else
            {
                _animationVerse = false;
            }
        }

        if (_currentTime == 0f)
        {
            _isInAnimation = false;
            _onLateAnimation -= CallbackReference;
            GWM.Instance.PlayerManager.Fail();
            EndSelect();
        }
    }

    private void EndSelect()
    {
        _border.gameObject.SetActive(false);
        _mask.gameObject.SetActive(false);
        _fill.gameObject.SetActive(false);
        _fill.fillAmount = 0f;

        _dataItem = null;
        _callbackDataItem = null;
        _interactable = null;
        _callbackInteractable = null;
    }

    public void OverrideSelectFill()
    {
        _onLateAnimation -= CallbackReference;
        EndSelect();
    }

    private IEnumerator Animation(float timeCoroutine)
    {
        _isInAnimation = true;
        while (_isInAnimation)
        {
            _currentTime += Time.deltaTime / timeCoroutine * (_animationVerse ? 1 : -1);

            if (_currentTime > 1 && _animationVerse)
            {
                _currentTime = 1f;
                _isInAnimation = false;
            }
            else if (_currentTime < 0 && !_animationVerse)
            {
                _currentTime = 0f;
                _isInAnimation = false;
            }

            _onAnimation?.Invoke();

            yield return null;
        }

        _onAnimation = null;
        _animation = null;

        _onLateAnimation?.Invoke();
    }

    void OnEnable()
    {
        if (_animation != null)
        {
            StartCoroutine(_animation);
        }
    }
}
