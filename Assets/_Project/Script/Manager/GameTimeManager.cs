using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeManager : Singleton_Generic<GameTimeManager>
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
    public float realSecondToGameSecond {  get; private set; }
    public int currentSecondDay { get; private set; }
    public int currentDay { get; private set; }
    private const int _secondDelay = 2;
    public int SecondDelay { get => _secondDelay; }

    private bool _alreadyStart;
    private bool _isGameTimeOn = true;
    private bool _isGamePause;
    private IEnumerator _gameTime;
    private WaitForSeconds _delay;

    [SerializeField] private Light _mainLight;
    public Transform mainLight { get => _mainLight.transform; }
    private Vector3 _angleSecondLightV3;

    public DayTime DayTime { get; private set; }
    private int[] _daylyBand = new int[4];

    public event Action onSecondDayChange;
    public event Action onDayChange;
    public event Action onDawn;
    public event Action onDay;
    public event Action onDusk;
    public event Action onNight;

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
            currentSecondDay = _gameDayInRealSeconds * ((_startHourDay * 60 * 60) + _startMinutesDay * 60) / (24 * 60 * 60);
        }
        realSecondToGameSecond = _gameDayInRealSeconds / (24 * 60 * 60);
        SetStartRotationMainLight();
        SetDailyBand();

        _delay = new WaitForSeconds(_secondDelay);
        _gameTime = GameTime();
        StartCoroutine(_gameTime);
        _alreadyStart = true;
    }

    private void SetDailyBand()
    {
        int secondHour = _gameDayInRealSeconds / 24;
        _daylyBand[0] = secondHour * 5;
        _daylyBand[1] = secondHour * 7;
        _daylyBand[2] = secondHour * 17;
        _daylyBand[3] = secondHour * 19;

        if (currentSecondDay < _daylyBand[0])
        {
            DayTime = DayTime.Night;
        }
        else if (currentDay < _daylyBand[1])
        {
            DayTime = DayTime.Dawn;
        }
        else if (currentDay < _daylyBand[2])
        {
            DayTime = DayTime.Day;
        }
        else if (currentDay < _daylyBand[3])
        {
            DayTime = DayTime.Dusk;
        }
        else
        {
            DayTime = DayTime.Night;
        }
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
        currentSecondDay *= _secondDelay;
        onSecondDayChange?.Invoke();
        //A day is passed
        if (currentSecondDay >= _gameDayInRealSeconds)
        {
            ++currentDay;
            currentSecondDay -= _gameDayInRealSeconds;
            onDayChange?.Invoke();
        }

        //Main Light change direction
        _mainLight.transform.eulerAngles += _angleSecondLightV3 * _secondDelay;

        SetDayTime();
    }

    //00.00 => 270°; 06.00 => 180°; 12.00 => 90°; 18.00 => 0°;
    private void SetStartRotationMainLight()
    {
        float angleSecondLight = 360f / _gameDayInRealSeconds;
        float angleXLight = (angleSecondLight * _gameDayInRealSeconds * 0.75f) - (angleSecondLight * currentSecondDay);
        _mainLight.transform.eulerAngles = new Vector3(angleXLight, _mainLight.transform.eulerAngles.y, _mainLight.transform.eulerAngles.z);
        _angleSecondLightV3 = new Vector3(angleSecondLight, 0f, 0f);
    }

    private void SetDayTime()
    {
        if (DayTime == DayTime.Night && currentSecondDay > _daylyBand[0])
        {
            DayTime = DayTime.Dawn;
            onDawn?.Invoke();
        }
        else if (DayTime == DayTime.Dawn && currentSecondDay > _daylyBand[1])
        {
            DayTime = DayTime.Day;
            onDay?.Invoke();
        }
        else if (DayTime == DayTime.Day && currentSecondDay > _daylyBand[2])
        {
            DayTime = DayTime.Dusk;
            onDusk?.Invoke();
        }
        else if (DayTime == DayTime.Dusk && currentSecondDay > _daylyBand[3])
        {
            DayTime = DayTime.Night;
            onNight?.Invoke();
        }
    }

}
