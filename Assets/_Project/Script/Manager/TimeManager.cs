using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public class TimeManager
{
    #region LikeSingleton
    private TimeManager()
    {
        if (GWM.Instance.MainDebug)
        {
            Debug.Log("TimeManager Initialized");
        }
    }

    public static TimeManager Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new TimeManager();
        }
        return null;
    }
    
    public static Key Key = new Key();
    #endregion

    private int _gameDayInRealSeconds = -1;
    public float RealSecondToGameSecond {  get; private set; }
    public int CurrentSecondDay { get; private set; }
    public int CurrentDay { get; private set; }
    public int SecondDelay { get => _secondDelay; }
    private const int _secondDelay = 2;

    private bool _isStartTimeSetted;
    private bool _isStarted;
    private bool _isTimeGameOn = true;
    private bool _isGamePause;
    private IEnumerator _gameTime;
    private WaitForSeconds _delay;

    private Transform _mainLight;
    private Vector3 _angleSecondLightV3;

    public DayTime DayTime { get; private set; }
    private int[] _daylyBand = new int[4];

    public event Action onSecondDayChange;
    public event Action onDayChange;
    public event Action onDawn;
    public event Action onDay;
    public event Action onDusk;
    public event Action onNight;

    //In Loading
    public bool StartTime(int currentSecondDay, int currentDay)
    {
        if (_isStartTimeSetted)
        {
            Debug.LogError("Time has already setted");
            return false;
        }
        else
        {
            _isStartTimeSetted = true;
            CurrentSecondDay = currentSecondDay;
            CurrentDay = currentDay;
            return true;
        }
    }

    //In Start of GameSceneManager
    public bool MyStart(Light mainLight, out IEnumerator time)
    {
        if (_isStarted)
        {
            Debug.LogError("Time has already started");
            time = null;
            return false;
        }
        else
        {
            _isStarted = true;

            _gameDayInRealSeconds = GWM.Instance.GameDayInRealMinutes * 60;
            RealSecondToGameSecond = (24 * 60 * 60) / _gameDayInRealSeconds;

            if (!S_SaveSystem.HasALoading)
            {
                int startHours = GWM.Instance.StartHours;
                int startMinutes = GWM.Instance.StartMinutes;
                CurrentSecondDay = _gameDayInRealSeconds * ((startHours * 60 * 60) + startMinutes * 60) / (24 * 60 * 60);
            }

            _mainLight = mainLight.transform;
            SetRotationMainLight();

            SetDailyBand();
            _delay = new WaitForSeconds(_secondDelay);
            time = GameTime();
            
            return true;
        }
    }

    //00.00 => 270°; 06.00 => 180°; 12.00 => 90°; 18.00 => 0°;
    public void SetRotationMainLight()
    {
        float angleSecondLight = 360f / _gameDayInRealSeconds;
        float angleXLight = (angleSecondLight * _gameDayInRealSeconds * 0.75f) - (angleSecondLight * CurrentSecondDay);
        _mainLight.eulerAngles = new Vector3(angleXLight, _mainLight.eulerAngles.y, _mainLight.eulerAngles.z);
        _angleSecondLightV3 = new Vector3(angleSecondLight, 0f, 0f);
    }

    private void SetDailyBand()
    {
        int secondHour = _gameDayInRealSeconds / 24;
        _daylyBand[0] = secondHour * 5;
        _daylyBand[1] = secondHour * 7;
        _daylyBand[2] = secondHour * 17;
        _daylyBand[3] = secondHour * 19;

        if (CurrentSecondDay < _daylyBand[0])
        {
            DayTime = DayTime.Night;
        }
        else if (CurrentDay < _daylyBand[1])
        {
            DayTime = DayTime.Dawn;
        }
        else if (CurrentDay < _daylyBand[2])
        {
            DayTime = DayTime.Day;
        }
        else if (CurrentDay < _daylyBand[3])
        {
            DayTime = DayTime.Dusk;
        }
        else
        {
            DayTime = DayTime.Night;
        }
    }

    private void SetIsGameTimeOn(bool value) => _isTimeGameOn = value;
    private void SetIsGamePause(bool value) => _isGamePause = value;

    private IEnumerator GameTime()
    {
        while (_isTimeGameOn)
        {
            //Game in Pause
            if (_isGamePause)
            {

            }
            //Game in Play
            else
            {
                GameInPlay();
            }
            yield return _delay;
        }
    }

    private void GameInPlay()
    {
        //Main Light change direction
        _mainLight.eulerAngles += _angleSecondLightV3 * _secondDelay;

        CurrentSecondDay += _secondDelay;
        SetDayTime();
        onSecondDayChange?.Invoke();
        //A day is passed
        if (CurrentSecondDay >= _gameDayInRealSeconds)
        {
            ++CurrentDay;
            CurrentSecondDay -= _gameDayInRealSeconds;
            onDayChange?.Invoke();
        }
    }

    private void SetDayTime()
    {
        if (DayTime == DayTime.Night && CurrentSecondDay > _daylyBand[0])
        {
            DayTime = DayTime.Dawn;
            onDawn?.Invoke();
        }
        else if (DayTime == DayTime.Dawn && CurrentSecondDay > _daylyBand[1])
        {
            DayTime = DayTime.Day;
            onDay?.Invoke();
        }
        else if (DayTime == DayTime.Day && CurrentSecondDay > _daylyBand[2])
        {
            DayTime = DayTime.Dusk;
            onDusk?.Invoke();
        }
        else if (DayTime == DayTime.Dusk && CurrentSecondDay > _daylyBand[3])
        {
            DayTime = DayTime.Night;
            onNight?.Invoke();
        }
    }

}
