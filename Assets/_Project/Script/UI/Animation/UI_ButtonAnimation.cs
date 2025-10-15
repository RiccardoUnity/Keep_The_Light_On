using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    enum ButtonState
    {
        None,
        Hover,
        Click
    }

    [Header("Debug")]
    [SerializeField] private bool _debugEvent;
    [SerializeField] private bool _debugMouse;
    [SerializeField] private bool _debugAnimation;

    [Header("Animation")]
    [SerializeField] private AnimationCurve _curveHover;
    [SerializeField] private float _timeHover;
    [SerializeField] private AnimationCurve _curveClick;
    [SerializeField] private float _timeClick;
    private AnimationCurve _curveCoroutine;
    private float _timeCoroutine;
    private float _currentTime;
    private bool _isInAnimation;
    private IEnumerator _animation;
    private bool _animationVerse;
    private ButtonState _targetState = ButtonState.None;
    private ButtonState _currentState = ButtonState.None;

    [Header("Events")]
    public UnityEvent onEnter;
    public UnityEvent<float> onAnimationHover;
    public UnityEvent onExit;
    public UnityEvent onDown;
    public UnityEvent<float> onAnimationClick;
    public UnityEvent onUp;
    private UnityEvent<float> _eventCoroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_debugMouse)
        {
            Debug.Log("Puntatore entrato", gameObject);
        }
        _targetState = ButtonState.Hover;
        _animationVerse = true;
        StartAnimation();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_debugMouse)
        {
            Debug.Log("Puntatore Uscito", gameObject);
        }
        _targetState = ButtonState.None;
        _animationVerse = false;
        StartAnimation();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_debugMouse)
        {
            Debug.Log("Puntatore Premuto", gameObject);
        }
        _targetState = ButtonState.Click;
        _animationVerse = true;
        StartAnimation();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_debugMouse)
        {
            Debug.Log("Puntatore Rilasciato", gameObject);
        }
        _targetState = ButtonState.Hover;
        _animationVerse = false;
        StartAnimation();
    }

    private void StartAnimation()
    {
        if (_animation == null)
        {
            _animation = Animation();
            StartCoroutine(_animation);
        }
    }

    void OnEnable()
    {
        if (_animation != null)
        {
            _targetState = ButtonState.None;
            _animationVerse = false;
            StartCoroutine(_animation);
        }
    }

    private IEnumerator Animation()
    {
        while (_targetState != _currentState)
        {
            //Eventi di ingresso e settaggi
            if (_targetState > _currentState)
            {
                _currentState++;
                _currentTime = 0f;
                switch (_currentState)
                {
                    case ButtonState.Hover:
                        if (_debugEvent)
                        {
                            Debug.Log("Evento Entrata", gameObject);
                        }
                        onEnter?.Invoke();
                        break;
                    case ButtonState.Click:
                        if (_debugEvent)
                        {
                            Debug.Log("Evento Premuto", gameObject);
                        }
                        onDown?.Invoke();
                        break;
                }
            }
            else
            {
                _currentTime = 1f;
            }

                //Settaggi paramatri animazione
                switch (_currentState)
                {
                    case ButtonState.Hover:
                        _curveCoroutine = _curveHover;
                        _timeCoroutine = _timeHover;
                        _eventCoroutine = onAnimationHover;
                        break;
                    case ButtonState.Click:
                        _curveCoroutine = _curveClick;
                        _timeCoroutine = _timeClick;
                        _eventCoroutine = onAnimationClick;
                        break;
                }

            //Animazione
            _isInAnimation = true;
            while (_isInAnimation)
            {
                _currentTime += Time.deltaTime / _timeCoroutine * (_animationVerse ? 1 : -1);

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

                if (_debugAnimation)
                {
                    Debug.Log($"Avanzamento animazione {_currentTime}", gameObject);
                }
                _eventCoroutine?.Invoke(_curveCoroutine.Evaluate(_currentTime));
                yield return null;
            }

            //Eventi di uscita
            if (_targetState < _currentState)
            {
                _currentState--;
                switch (_currentState)
                {
                    case ButtonState.None:
                        if (_debugEvent)
                        {
                            Debug.Log("Evento Uscito", gameObject);
                        }
                        onExit?.Invoke();
                        break;
                    case ButtonState.Hover:
                        if (_debugEvent)
                        {
                            Debug.Log("Evento Rilasciato", gameObject);
                        }
                        onUp?.Invoke();
                        break;
                }
            }
        }

        _animation = null;
    }
}
