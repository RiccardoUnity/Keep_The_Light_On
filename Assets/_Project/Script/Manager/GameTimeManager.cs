using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : SingletonGeneric<GameTimeManager>
{
    #region Singleton
    protected override bool ShouldBeDestroyOnLoad() => true;

    static GameTimeManager()
    {
        _useResources = false;
        _resourcesPath = "";
    }
    #endregion

    [SerializeField] private bool _isNewGame = true;
    [SerializeField] private int _startHourDay;   //Only new game
    [SerializeField] private int _startMinutesDay;   //Only new game
    [SerializeField] private int _gameDayInRealMinutes = 30;
    private int _gameDayInRealSeconds;
    private int _secondDay;
    private int _day;
    private const int _secondDelay = 5;

    private bool _alreadyStart;
    private bool _isGameTimeOn = true;
    private bool _isGamePause;
    private IEnumerator _gameTime;
    private WaitForSeconds _delay;

    [SerializeField] private Light _mainLight;
    private float _angleSecondLight;
    private float _angleXLight;

    public event Action onSecondDayChange;
    public event Action onDayChange;

    void OnEnable()
    {
        if (_alreadyStart)
        {
            SetStartRotationMainLight();
            StartCoroutine(_gameTime);
        }
    }

    void Start()
    {
        if (_mainLight == null)
        {
            Debug.LogError("Main Light missing", gameObject);
        }

        //SetUp StartTime
        _gameDayInRealSeconds = _gameDayInRealMinutes * 60;
        if (_isNewGame)
        {
            _isNewGame = false;
            _secondDay = _gameDayInRealSeconds * ((_startHourDay * 60 * 60) + _startMinutesDay * 60) / (24 * 60 * 60);
        }
        SetStartRotationMainLight();

        _delay = new WaitForSeconds(_secondDelay);
        _gameTime = GameTime();
        StartCoroutine(_gameTime);
        _alreadyStart = true;
    }

    private void SetIsGameTimeOn(bool value) => _isGameTimeOn = value;
    private void SetIsGamePause(bool value) => _isGamePause = value;

    private IEnumerator GameTime()
    {
        while (_isGameTimeOn)
        {
            yield return _delay;
            //Game in Pause
            if (_isGamePause)
            {

            }
            //Game in Play
            else
            {
                GameInPlay();
            }
        }
    }

    private void GameInPlay()
    {
        _secondDay *= _secondDelay;
        onSecondDayChange?.Invoke();
        //A day is passed
        if (_secondDay >= _gameDayInRealSeconds)
        {
            ++_day;
            _secondDay -= _gameDayInRealSeconds;
            onDayChange?.Invoke();
        }

        //Main Light change direction
        _angleXLight -= _angleSecondLight * _secondDelay; //==>Controlla!!!
        if (_angleXLight < 0f)
        {
            _angleXLight += 360f;
        }
        _mainLight.transform.eulerAngles = new Vector3(_angleXLight, _mainLight.transform.eulerAngles.y, _mainLight.transform.eulerAngles.z);
    }

    //00.00 => 270°; 06.00 => 180°; 12.00 => 90°; 18.00 => 0°;
    private void SetStartRotationMainLight()
    {
        _angleSecondLight = _gameDayInRealSeconds / 360f;
        _angleXLight = (_angleSecondLight * _gameDayInRealSeconds * 0.75f) - (_angleSecondLight * _secondDay);
        _mainLight.transform.eulerAngles = new Vector3(_angleXLight, _mainLight.transform.eulerAngles.y, _mainLight.transform.eulerAngles.z);
    }
}
