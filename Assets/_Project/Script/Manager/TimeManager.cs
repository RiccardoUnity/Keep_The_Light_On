using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public class TimeManager
{
    #region LikeSingleton
    private TimeManager() { }

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
    public float realSecondToGameSecond {  get; private set; }
    public int currentSecondDay { get; private set; }
    public int currentDay { get; private set; }
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
            this.currentSecondDay = currentSecondDay;
            this.currentDay = currentDay;
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
            realSecondToGameSecond = (24 * 60 * 60) / _gameDayInRealSeconds;

            if (GWM.Instance.IsNewGame)
            {
                int startHours = GWM.Instance.StartHours;
                int startMinutes = GWM.Instance.StartMinutes;
                currentSecondDay = _gameDayInRealSeconds * ((startHours * 60 * 60) + startMinutes * 60) / (24 * 60 * 60);
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
        float angleXLight = (angleSecondLight * _gameDayInRealSeconds * 0.75f) - (angleSecondLight * currentSecondDay);
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

    private void SetIsGameTimeOn(bool value) => _isTimeGameOn = value;
    private void SetIsGamePause(bool value) => _isGamePause = value;

    private IEnumerator GameTime()
    {
        while (_isTimeGameOn)
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
        currentSecondDay += _secondDelay;
        onSecondDayChange?.Invoke();
        //A day is passed
        if (currentSecondDay >= _gameDayInRealSeconds)
        {
            ++currentDay;
            currentSecondDay -= _gameDayInRealSeconds;
            onDayChange?.Invoke();
        }

        //Main Light change direction
        _mainLight.eulerAngles += _angleSecondLightV3 * _secondDelay;

        SetDayTime();
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
