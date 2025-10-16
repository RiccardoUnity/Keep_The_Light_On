using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fader : Singleton_Generic<Fader>
{
    protected override bool ShouldBeDestroyOnLoad() => false;

    static Fader()
    {
        _useResources = true;
        _resourcesPath = "Fader";
    }

    [SerializeField] private Image _background;
    [SerializeField] private Image _foreground;
    public float TimeCoroutine { get => _timeCoroutine; }
    [Range(1f, 5f)] [SerializeField] private float _timeCoroutine = 2f;

    private Color _alpha0 = new Color(0f, 0f, 0f, 0f);
    private Color _alpha1 = new Color(0f, 0f, 0f, 1f);

    private bool _isToBlack;
    private IEnumerator _effect;
    private bool _isInAnimation;
    private float _currentTime = 1f;
    private int _scene = -1;

    void OnEnable()
    {
        if (_effect != null)
        {
            StartCoroutine(_effect);
        }
    }

    void Start()
    {
        _background.color = _alpha1;
        _foreground.color = _alpha1;
        ToScene(0, true);
    }

    public void ToScene(int scene, bool bypass = false)
    {
        if (!bypass)
        {
            _scene = scene;
        }
        if (_effect == null)
        {
            _effect = Effect(bypass? 1 : 2);
            StartCoroutine(_effect);
        }
    }

    IEnumerator Effect(int indexMax)
    {
        for (int i = 0; i < indexMax; ++i)
        {
            _isInAnimation = true;
            while (_isInAnimation)
            {
                _currentTime += Time.deltaTime / _timeCoroutine * (_isToBlack ? 1 : -1);

                if (_currentTime > 1f && _isToBlack)
                {
                    _currentTime = 1f;
                    _isInAnimation = false;
                }
                else if (_currentTime < 0f && !_isToBlack)
                {
                    _currentTime = 0f;
                    _isInAnimation = false;
                }

                if (_currentTime < 0.5f)
                {
                    _foreground.color = Color.Lerp(_alpha0, _alpha1, _currentTime * 2f);
                    _background.color = _alpha0;
                }
                else
                {
                    _background.color = Color.Lerp(_alpha0, _alpha1, (_currentTime - 0.5f) * 2f);
                    _foreground.color = _alpha1;
                }

                yield return null;
            }

            if (_isToBlack)
            {
                _foreground.color = _alpha1;
                _background.color = _alpha1;
                _isToBlack = false;
            }
            else
            {
                _foreground.color = _alpha0;
                _background.color = _alpha0;
                _isToBlack = true;
            }

            if (_scene > 0)
            {
                SceneManager.LoadScene(_scene);
                _scene = -1;
            }
        }

        _effect = null;
    }
}