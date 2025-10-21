using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;
using StringConst = StaticData.S_GameManager.StringConst;

[RequireComponent(typeof(PlayerGroundCheck))]
[RequireComponent(typeof(PlayerController))]
public class PlayerManager : MonoBehaviour
{
    private bool _isMyAwake;
    [SerializeField] private bool _mainDebug;
    [SerializeField] private bool _gizmosDebug;

    //Component
    public Transform Head { get => _head; }
    [SerializeField] private Transform _head;
    public CinemachineVirtualCamera Camera { get => _camera; }
    [SerializeField] private CinemachineVirtualCamera _camera;
    public PlayerGroundCheck PlayerGroundCheck { get; private set; }
    public PlayerController PlayerController { get; private set; }
    private TimeManager _timeManager;

    //Stats
    public static Key Key = new Key();
    private PlayerStat[] _playerStat = new PlayerStat[9];

    public PS_Life Life { get => _life; }
    private PS_Life _life;
    [SerializeField] private bool _debugLife;
    [Range(0f, 1f)] public float startValueLife = 1f;
    public PS_Endurance Endurance { get => _endurance; }
    private PS_Endurance _endurance;
    [SerializeField] private bool _debugEndurance;
    [Range(0f, 1f)] public float startValueEndurance = 1f;
    public PS_Rest Rest { get => _rest; }
    private PS_Rest _rest;
    [SerializeField] private bool _debugRest;
    [Range(0f, 1f)] public float startValueRest = 1f;
    public PS_Hunger Hunger { get => _hunger; }
    private PS_Hunger _hunger;
    [SerializeField] private bool _debugHunger;
    [Range(0f, 1f)] public float startValueHunger = 1f;
    public PS_Thirst Thirst { get => _thirst; }
    private PS_Thirst _thirst;
    [SerializeField] private bool _debugThirst;
    [Range(0f, 1f)] public float startValueThirst = 1f;
    public PS_Stamina Stamina { get => _stamina; }
    private PS_Stamina _stamina;
    [SerializeField] private bool _debugStamina;
    [Range(0f, 1f)] public float startValueStamina = 1f;
    public PS_SunStroke SunStroke { get => _sunStroke; }
    private PS_SunStroke _sunStroke;
    [SerializeField] private bool _debugSunStroke;
    [Range(0f, 1f)] public float startValueSunStroke = 0f;
    public PS_StomachAche  StomachAche { get => _stomachAche; }
    private PS_StomachAche _stomachAche;
    [SerializeField] private bool _debugStomachAche;
    [Range(0f, 1f)] public float startValueStomachAche = 0f;
    public PS_HeartAche HeartAche { get => _heartAche; }
    private PS_HeartAche _heartAche;
    [SerializeField] private bool _debugHeartAche;
    [Range(0f, 1f)] public float startValueHeartAche = 0f;

    [Header("Settings")]
    [SerializeField] private float _distanceMainLight = 15f;
    [SerializeField] private LayerMask _blockMainLight = (1 << 0);

    [SerializeField] private float _distanceRayInteractable = 3f;
    [SerializeField] private LayerMask _interactableLayerMask = (1 << 6);
    private RaycastHit _hit;

    //Other
    public bool IsUnderTheSun { get; private set; }
    public bool IsWakeUp { get => _isWakeUp; }
    private bool _isWakeUp = true;

    public bool TrySelectInteractable { get => _trySelectInteractable; }
    private bool _trySelectInteractable;
    private bool _isShowScope;
    private bool _isHarvestStart;
    private Interactable _interactable;
    private Data_Item _dataItem;

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
            Func<bool, float, bool>[] myAwake = new Func<bool, float, bool>[9];
            Func<bool>[] myStart = new Func<bool>[9];

            _life = PS_Life.Instance(Key.GetKey(), out myAwake[0], out myStart[0]);
            _endurance = PS_Endurance.Instance(Key.GetKey(), out myAwake[1], out myStart[1]);
            _rest = PS_Rest.Instance(Key.GetKey(), out myAwake[2], out myStart[2]);
            _hunger = PS_Hunger.Instance(Key.GetKey(), out myAwake[3], out myStart[3]);
            _thirst = PS_Thirst.Instance(Key.GetKey(), out myAwake[4], out myStart[4]);
            _stamina = PS_Stamina.Instance(Key.GetKey(), out myAwake[5], out myStart[5]);
            _sunStroke = PS_SunStroke.Instance(Key.GetKey(), out myAwake[6], out myStart[6]);
            _stomachAche = PS_StomachAche.Instance(Key.GetKey(), out myAwake[7], out myStart[7]);
            _heartAche = PS_HeartAche.Instance(Key.GetKey(), out myAwake[8], out myStart[8]);

            _playerStat[0] = _life;
            _playerStat[1] = _endurance;
            _playerStat[2] = _rest;
            _playerStat[3] = _hunger;
            _playerStat[4] = _thirst;
            _playerStat[5] = _stamina;
            _playerStat[6] = _sunStroke;
            _playerStat[7] = _stomachAche;
            _playerStat[8] = _heartAche;

            //Load internal data
            if (S_SaveSystem.HasALoading)
            {
                S_SaveSystem.LoadPlayerManager();
            }
            else
            {

            }

            //Init Stats
            int i;
            bool[] debug = { _debugLife, _debugEndurance, _debugRest, _debugHunger, _debugThirst, _debugStamina, _debugSunStroke, _debugStomachAche, _debugHeartAche };
            float[] startValue = { startValueLife, startValueEndurance, startValueRest, startValueHunger, startValueThirst, startValueStamina, startValueSunStroke, startValueStomachAche, startValueHeartAche };
            //Awake
            for (i = 0; i < _playerStat.Length; i++)
            {
                if (!myAwake[i].Invoke(debug[i], startValue[i]))
                {
                    Debug.LogError($"My Awake of {_playerStat[i].Name} doesn't execute");
                }
            }
            //Start
            for (i = 0; i < _playerStat.Length; i++)
            {
                if (!myStart[i].Invoke())
                {
                    Debug.LogError($"My Start of {_playerStat[i].Name} doesn't execute");
                }
            }
            //Event
            for (i = 0; i < _playerStat.Length; ++i)
            {
                _playerStat[i].CallEvent();
            }

            //Time
            _timeManager = GWM.Instance.TimeManager;
            _timeManager.onPriority += MyUpdatePriority;
            _timeManager.onNotPriority1 += MyUpdateNotPriority1;
        }
    }

    //Input
    void Update()
    {
        if (Input.GetButtonDown(StringConst.MouseLeft))
        {
            _trySelectInteractable = true;
        }
        else if (Input.GetButtonUp(StringConst.MouseLeft))
        {
            _trySelectInteractable = false;
        }
    }

    private void MyUpdatePriority(float timeDelay)
    {
        SelectInteractable();
    }

    private void MyUpdateNotPriority1(float timeDelay)
    {
        UnderTheSun();
    }

    private void UnderTheSun()
    {
        if (_timeManager.DayTime == DayTime.Day)
        {
            Ray ray = new Ray(_head.position, GWM.Instance.MainLight.rotation * Vector3.forward);
            if (Physics.Raycast(ray, _distanceMainLight, _blockMainLight, GWM.Instance.Qti))
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

        if (_mainDebug)
        {
            Debug.Log($"Is Player under the Sun: {IsUnderTheSun}");
        }
    }

    private void SelectInteractable()
    {
        _interactable = null;
        _dataItem = null;
        if (Physics.Linecast(_head.position, _head.position + (_head.forward * _distanceRayInteractable), out _hit, _interactableLayerMask, GWM.Instance.Qti))
        {
            _interactable = _hit.collider.gameObject.GetComponent<Interactable>();
            if (_interactable != null)
            {
                if (!_isShowScope)
                {
                    _isShowScope = true;
                    GWM.Instance.UIScope.StartShowScope(_isShowScope);
                }

                //Interactable not persistent
                if (_interactable is Prefab_Item prefab_item)
                {
                    _dataItem = prefab_item.DataItem;
                    if (_trySelectInteractable)
                    {
                        if (!_isHarvestStart)
                        {
                            _isHarvestStart = true;
                            GWM.Instance.UIScope.StartSelect(_dataItem, HarvestPrefabItem);
                        }
                    }
                }
                //Interactable persistent
                else
                {

                }
            }
        }
        else
        {
            DeactiveScope();
        }
    }

    private void DeactiveScope()
    {
        if (_isShowScope)
        {
            _isShowScope = false;
            _isHarvestStart = false;
            GWM.Instance.UIScope.StartShowScope(_isShowScope);
            GWM.Instance.UIScope.OverrideSelectFill();
        }
    }

    public void HarvestFail() => _isHarvestStart = false;

    private void HarvestPrefabItem(Data_Item dataItem)
    {
        if (_isHarvestStart)
        {
            _isHarvestStart = false;
            //The selected item remained the same for the duration of the animation
            if (_trySelectInteractable && dataItem == _dataItem)
            {

                if (_mainDebug)
                {
                    Debug.Log($"Item {_dataItem.SOItem.Name} harvested");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_gizmosDebug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_head.position, _head.position + (_head.forward * _distanceRayInteractable));
        }
    }
}
