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

    public static TimeManager Instance(int key, bool debug = false)
    {
        if (key == Key.GetKey())
        {
            _debug = debug;
            return new TimeManager();
        }
        return null;
    }
    
    public static Key Key = new Key();
    private static bool _debug;
    #endregion

    private float _gameDayInRealSeconds = -1;
    public float RealSecondToGameSecond {  get; private set; }
    public float CurrentSecondDay { get; private set; }
    public int CurrentDay { get; private set; }
    public float DelayTimePriority { get; private set; }
    public float DelayTimeNotPriority1 { get; private set; }
    public float DelayTimeNotPriority2 { get; private set; }
    private bool _isPauseCalled = true;
    private float _accelerationInGameSecond;
    private float _accelerationMoltiplier = 1f;
    private float _realSecondAwait;
    private bool _isStartTimeSetted;
    private bool _isMyAwake;
    private bool _isTimeGameOn = true;
    public GameTimeType GameTimeType { get; private set; }
    private int _delayFramePriority = 20;   //Need multiple of 4
    private int _delayFrameNotPriority1;
    private int _delayFrameNotPriority2;
    private int _delayFrameCount;
    private int _delayFrameCountRestart;

    private Transform _mainLight;
    private Vector3 _angleSecondLightV3;
    private Vector3 _angleMainLight;

    public DayTime DayTime { get; private set; }
    private float[] _daylyBand = new float[4];

    public event Action onPause;
    public event Action onResume;
    public event Action<float> onPriority;
    public event Action<float> onNotPriority1;
    public event Action<float> onNotPriority2;
    public event Action onEndAceleration;
    public event Action onDayChange;
    public event Action onDawn;
    public event Action onDay;
    public event Action onDusk;
    public event Action onNight;

    //In Loading
    public bool StartTime(float currentSecondDay, int currentDay)
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

    //In Awake of GameWorldManager
    public bool MyAwake(Light mainLight, out IEnumerator time)
    {
        if (_isMyAwake)
        {
            Debug.LogError("Time has already started");
            time = null;
            return false;
        }
        else
        {
            _isMyAwake = true;

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
            onNotPriority2 += SetMainLightDay;

            GameTimeType = GameTimeType.Normal;
            _delayFrameNotPriority1 = _delayFramePriority / 2;
            _delayFrameNotPriority2 = _delayFramePriority + _delayFrameNotPriority1;
            _delayFrameCountRestart = _delayFramePriority * 2;

            time = GameTime();
            
            return true;
        }
    }

    //00.00 => 270°; 06.00 => 180°; 12.00 => 90°; 18.00 => 0°;
    public void SetRotationMainLight()
    {
        float angleSecondLight = 360f / _gameDayInRealSeconds;
        float angleXLight = (angleSecondLight * _gameDayInRealSeconds * 0.75f) - (angleSecondLight * CurrentSecondDay);
        _angleMainLight = new Vector3(angleXLight, _mainLight.eulerAngles.y, _mainLight.eulerAngles.z);
        _mainLight.eulerAngles = _angleMainLight;
        _angleSecondLightV3 = new Vector3(angleSecondLight, 0f, 0f);
    }

    private void SetDailyBand()
    {
        float secondHour = _gameDayInRealSeconds / 24;
        _daylyBand[0] = secondHour * 5;
        _daylyBand[1] = secondHour * 7;
        _daylyBand[2] = secondHour * 17;
        _daylyBand[3] = secondHour * 19;

        if (CurrentSecondDay < _daylyBand[0])
        {
            DayTime = DayTime.Night;
        }
        else if (CurrentSecondDay < _daylyBand[1])
        {
            DayTime = DayTime.Dawn;
        }
        else if (CurrentSecondDay < _daylyBand[2])
        {
            DayTime = DayTime.Day;
        }
        else if (CurrentSecondDay < _daylyBand[3])
        {
            DayTime = DayTime.Dusk;
        }
        else
        {
            DayTime = DayTime.Night;
        }
    }

    private void SetIsGameTimeOn(bool value) => _isTimeGameOn = value;

    private IEnumerator GameTime()
    {
        while (_isTimeGameOn)
        {
            if (GWM.Instance.IsGamePause && !_isPauseCalled)
            {
                GameTimeType = GameTimeType.Pause;
                _isPauseCalled = true;
                onPause?.Invoke();
            }
            else if (!GWM.Instance.IsGamePause && _isPauseCalled)
            {
                GameTimeType = GameTimeType.Normal;
                _isPauseCalled = false;
                onResume?.Invoke();
            }

            if (GameTimeType == GameTimeType.Pause)
            {
                
            }
            else
            {
                //Time Count
                CurrentSecondDay += Time.deltaTime * _accelerationMoltiplier;
                if (CurrentSecondDay >= _gameDayInRealSeconds)
                {
                    ++CurrentDay;
                    CurrentSecondDay -= _gameDayInRealSeconds;
                    onDayChange?.Invoke();
                }
                DelayTimePriority += Time.deltaTime * _accelerationMoltiplier;
                DelayTimeNotPriority1 += Time.deltaTime * _accelerationMoltiplier;
                DelayTimeNotPriority2 += Time.deltaTime * _accelerationMoltiplier;
                ++_delayFrameCount;

                if (GameTimeType == GameTimeType.Normal)
                {
                    if (_delayFrameCount >= _delayFrameCountRestart)
                    {
                        _delayFrameCount = 0;
                    }

                    if (_delayFrameCount == 0 || _delayFrameCount == _delayFramePriority)
                    {
                        onPriority?.Invoke(DelayTimePriority);
                        DelayTimePriority = 0f;

                        if (_debug)
                        {
                            Debug.Log($"SecondDay {CurrentSecondDay} - Day {CurrentDay} - DayTime {DayTime} - GameTimeType {GameTimeType}");
                        }
                    }
                    else if (_delayFrameCount == _delayFrameNotPriority1)
                    {
                        onNotPriority1?.Invoke(DelayTimeNotPriority1);
                        DelayTimeNotPriority1 = 0f;
                    }
                    else if (_delayFrameCount == _delayFrameNotPriority2)
                    {
                        onNotPriority2?.Invoke(DelayTimeNotPriority2);
                        DelayTimeNotPriority2 = 0f;
                    }
                }
                else if(GameTimeType == GameTimeType.Accelerate)
                {
                    _realSecondAwait -= Time.deltaTime;
                    if (_realSecondAwait > 0)
                    {
                        if (_delayFrameCount >= 4)
                        {
                            _delayFrameCount = 0;
                        }

                        if (_delayFrameCount == 0 || _delayFrameCount == 2)
                        {
                            onPriority?.Invoke(DelayTimePriority);
                            DelayTimePriority = 0f;

                            if (_debug)
                            {
                                Debug.Log($"SecondDay {CurrentSecondDay} - Day {CurrentDay} - DayTime {DayTime} - GameTimeType {GameTimeType}");
                            }
                        }
                        else if (_delayFrameCount == 1)
                        {
                            onNotPriority1?.Invoke(DelayTimeNotPriority1);
                            DelayTimeNotPriority1 = 0;
                        }
                        else if (_delayFrameCount == 3)
                        {
                            onNotPriority2?.Invoke(DelayTimeNotPriority2);
                            DelayTimeNotPriority2 = 0;
                        }
                    }
                    else
                    {
                        GameTimeType = GameTimeType.Normal;
                        _accelerationMoltiplier = 1f;
                        onEndAceleration?.Invoke();
                    }


                    
                }
            }
            yield return null;
        }
    }

    //Main Light change direction
    private void SetMainLightDay(float timeDelay)
    {
         _angleMainLight -= _angleSecondLightV3 * timeDelay;
        _mainLight.eulerAngles = _angleMainLight;
        if (DayTime != DayTime.Dawn && CurrentSecondDay > _daylyBand[0] && CurrentSecondDay < _daylyBand[1])
        {
            DayTime = DayTime.Dawn;
            onDawn?.Invoke();
        }
        else if (DayTime != DayTime.Day && CurrentSecondDay > _daylyBand[1] && CurrentSecondDay < _daylyBand[2])
        {
            DayTime = DayTime.Day;
            onDay?.Invoke();
        }
        else if (DayTime != DayTime.Dusk && CurrentSecondDay > _daylyBand[2] && CurrentSecondDay < _daylyBand[3])
        {
            DayTime = DayTime.Dusk;
            onDusk?.Invoke();
        }
        else if (DayTime != DayTime.Night && (CurrentSecondDay > _daylyBand[3] || CurrentSecondDay < _daylyBand[0]))
        {
            DayTime = DayTime.Night;
            onNight?.Invoke();
        }
    }

    public void SetGamePlayAccelerate(float realSecondAccelerate, float realSecondAwait)
    {
        GameTimeType = GameTimeType.Accelerate;
        _realSecondAwait = realSecondAwait;
        _accelerationInGameSecond = realSecondAccelerate / RealSecondToGameSecond;
        _accelerationMoltiplier = _accelerationInGameSecond / _realSecondAwait;
        GWM.Instance.UIAcceleratedTime.StartAcceleration(_realSecondAwait);
    }
}
