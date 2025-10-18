using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

[RequireComponent(typeof(PlayerGroundCheck))]
[RequireComponent(typeof(PlayerController))]
public class PlayerManager : MonoBehaviour
{
    private bool _isMyAwake;

    public bool IsWakeUp { get => _isWakeUp; }
    private bool _isWakeUp;

    //Component
    public Transform Head { get => _head; }
    [SerializeField] private Transform _head;
    public CinemachineVirtualCamera Camera { get => _camera; }
    [SerializeField] private CinemachineVirtualCamera _camera;
    public PlayerGroundCheck PlayerGroundCheck { get; private set; }
    public PlayerController PlayerController { get; private set; }

    //Stats
    public static Key Key = new Key();
    public PS_Life Life { get => _life; }
    private PS_Life _life;
    [SerializeField] private bool _debugLife;
    public PS_Endurance Endurance { get => _endurance; }
    private PS_Endurance _endurance;
    [SerializeField] private bool _debugEndurance;
    public PS_Rest Rest { get => _rest; }
    private PS_Rest _rest;
    [SerializeField] private bool _debugRest;
    public PS_Hunger Hunger { get => _hunger; }
    private PS_Hunger _hunger;
    [SerializeField] private bool _debugHunger;
    public PS_Thirst Thirst { get => _thirst; }
    private PS_Thirst _thirst;
    [SerializeField] private bool _debugThirst;
    public PS_Stamina Stamina { get => _stamina; }
    private PS_Stamina _stamina;
    [SerializeField] private bool _debugStamina;
    public PS_SunStroke SunStroke { get => _sunStroke; }
    private PS_SunStroke _sunStroke;
    [SerializeField] private bool _debugSunStroke;
    public PS_StomacStroke  StomacStroke { get => _stomacStroke; }
    private PS_StomacStroke _stomacStroke;
    [SerializeField] private bool _debugStomacStroke;
    public PS_HeartStroke HeartStroke { get => _heartStroke; }
    private PS_HeartStroke _heartStroke;
    [SerializeField] private bool _debugHeartStroke;
    private event Func<bool> _myAwakeStats;
    [SerializeField] private bool _debug;

    private TimeManager _timeManager;

    [Header("Under The Sun")]
    [SerializeField] private float _distanceRay = 15f;
    public bool IsUnderTheSun { get; private set; }


    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            PlayerGroundCheck = GetComponent<PlayerGroundCheck>();
            PlayerController = GetComponent<PlayerController>();

            PlayerGroundCheck.MyAwake();
            PlayerController.MyAwake();

            //Stats
            _myAwakeStats += PS_Life.Instance(Key.GetKey(), out _life);

            _myAwakeStats += PS_Endurance.Instance(Key.GetKey(), out _endurance);
            _myAwakeStats += PS_Rest.Instance(Key.GetKey(), out _rest);
            _myAwakeStats += PS_Hunger.Instance(Key.GetKey(), out _hunger);
            _myAwakeStats += PS_Thirst.Instance(Key.GetKey(), out _thirst);
            _myAwakeStats += PS_Stamina.Instance(Key.GetKey(), out _stamina);
            _myAwakeStats += PS_SunStroke.Instance(Key.GetKey(), out _sunStroke);
            _myAwakeStats += PS_StomacStroke.Instance(Key.GetKey(), out _stomacStroke);
            _myAwakeStats += PS_HeartStroke.Instance(Key.GetKey(), out _heartStroke);

            //Load internal data
            if (S_SaveSystem.HasALoading)
            {
                S_SaveSystem.LoadPlayerManager();
            }
            else
            {

            }

            //Awake Stats
            _myAwakeStats?.Invoke();
            _myAwakeStats = null;

            _timeManager = GWM.Instance.TimeManager;
            _timeManager.onNormalPriority += UnderTheSun;
        }
    }

    private void UnderTheSun(int secondDelay)
    {
        if (_timeManager.DayTime == DayTime.Day)
        {
            Ray ray = new Ray(_head.position, GWM.Instance.mainLight.rotation * Vector3.forward);
            if (Physics.Raycast(ray, _distanceRay, GWM.Instance.blockMainLight, GWM.Instance.qti))
            {
                IsUnderTheSun = true;
            }
            else
            {
                IsUnderTheSun = false;
            }
        }
        else
        {
            IsUnderTheSun = false;
        }
    }
}
