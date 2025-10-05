using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class S_TimeManager
{
    private static int _gameDayInRealSeconds = -1;
    public static float realSecondToGameSecond {  get; private set; }
    public static int currentSecondDay { get; private set; }
    public static int currentDay { get; private set; }
    private const int _secondDelay = 2;
    public static int SecondDelay { get => _secondDelay; }

    private static bool _isStartTimeSetted;
    private static bool _isGameTimeOn = true;
    private static bool _isGamePause;
    private static IEnumerator _gameTime;
    private static WaitForSeconds _delay;

    
    private static Vector3 _angleSecondLightV3;

    public static DayTime DayTime { get; private set; }
    private static int[] _daylyBand = new int[4];

    public static event Action onSecondDayChange;
    public static event Action onDayChange;
    public static event Action onDawn;
    public static event Action onDay;
    public static event Action onDusk;
    public static event Action onNight;

    public static void SetUp()
    {
        if (GameSceneManager.Instance.isNewGame)
        {
            int startHours = GameSceneManager.Instance.startHours;
            int startMinutes = GameSceneManager.Instance.startMinutes;
            currentSecondDay = _gameDayInRealSeconds * ((startHours * 60 * 60) + startMinutes * 60) / (24 * 60 * 60);
        }
    }

    public static bool StartTime(int currentSecondDay, int currentDay)
    {
        if (_isStartTimeSetted)
        {
            Debug.LogError("Time has already setted");
            return false;
        }
        else
        {
            _isStartTimeSetted = true;
            S_TimeManager.currentSecondDay = currentSecondDay;
            S_TimeManager.currentDay = currentDay;
            return true;
        }
    }

    private static void SetGameDayInRealSeconds()
    {
        _gameDayInRealSeconds = GameSceneManager.Instance.gameDayInRealMinutes * 60;
    }

    //00.00 => 270°; 06.00 => 180°; 12.00 => 90°; 18.00 => 0°;
    public static void SetStartRotationMainLight(ref Light mainLight)
    {
        if (_gameDayInRealSeconds < 0)
        {
            SetGameDayInRealSeconds();
        }

        float angleSecondLight = 360f / _gameDayInRealSeconds;
        float angleXLight = (angleSecondLight * _gameDayInRealSeconds * 0.75f) - (angleSecondLight * currentSecondDay);
        mainLight.transform.eulerAngles = new Vector3(angleXLight, mainLight.transform.eulerAngles.y, mainLight.transform.eulerAngles.z);
        _angleSecondLightV3 = new Vector3(angleSecondLight, 0f, 0f);
    }

    

    void Start()
    {
        

        //SetUp StartTime
        _gameDayInRealSeconds = _gameDayInRealMinutes * 60;
        
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

    

    private static void SetDayTime()
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
