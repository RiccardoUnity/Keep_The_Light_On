using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public PS_Life Life { get; private set; }
    public PS_Endurance Endurance { get; private set; }
    public PS_Rest Rest { get; private set; }
    public PS_Hunger Hunger { get; private set; }
    public PS_Thirst Thirst { get; private set; }
    public PS_Stamina Stamina { get; private set; }
    public PS_SunStroke SunStroke { get; private set; }
    public PS_StomacStroke  StomacStroke { get; private set; }
    public PS_HeartStroke HeartStroke { get; private set; }

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
            Life = PS_Life.Instance(Key.GetKey());
            Endurance = PS_Endurance.Instance(Key.GetKey());
            Rest = PS_Rest.Instance(Key.GetKey());
            Hunger = PS_Hunger.Instance(Key.GetKey());
            Thirst = PS_Thirst.Instance(Key.GetKey());
            Stamina = PS_Stamina.Instance(Key.GetKey());
            SunStroke = PS_SunStroke.Instance(Key.GetKey());
            StomacStroke = PS_StomacStroke.Instance(Key.GetKey());
            HeartStroke = PS_HeartStroke.Instance(Key.GetKey());

            //Load internal data
            if (S_SaveSystem.HasALoading)
            {
                S_SaveSystem.LoadPlayerManager();
            }
            else
            {

            }
        }
    }

    void Start()
    {
        //Stats
        Life.MyStart();
        Endurance.MyStart();
        Rest.MyStart();
        Hunger.MyStart();
        Thirst.MyStart();
        Stamina.MyStart();
        SunStroke.MyStart();
        StomacStroke.MyStart();
        HeartStroke.MyStart();
    }
}
