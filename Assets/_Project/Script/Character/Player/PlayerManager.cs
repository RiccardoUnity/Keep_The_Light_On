using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerGroundCheck))]
[RequireComponent(typeof(PlayerController))]
public class PlayerManager : MonoBehaviour
{
    private bool _isMyAwake;

    public Transform Head { get => _head; }
    [SerializeField] private Transform _head;

    public CinemachineVirtualCamera Camera { get => _camera; }
    [SerializeField] private CinemachineVirtualCamera _camera;

    //Component
    public PlayerGroundCheck PlayerGroundCheck { get; private set; }
    public PlayerController PlayerController { get; private set; }

    //Stats
    public static Key Key = new Key();
    public PlayerStat_Endurance Endurance { get; private set; }

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
            Endurance = PlayerStat_Endurance.Instance(Key.GetKey());

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
        Endurance.MyStart();
    }
}
