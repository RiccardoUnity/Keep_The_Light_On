using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GTM = TimeGame;

[RequireComponent(typeof(PlayerGroundCheck))]
[RequireComponent(typeof(PlayerController))]
public class PlayerManager : Singleton_Generic<PlayerManager>
{
    #region Singleton
    protected override bool ShouldBeDestroyOnLoad() => true;

    static PlayerManager()
    {
        _useResources = false;
        _resourcesPath = "";
    }
    #endregion

    [SerializeField] private Transform _head;
    public Transform head { get =>  _head; }

    //Component
    public PlayerGroundCheck playerGroundCheck { get; private set; }
    public PlayerController playerController { get; private set; }

    //Stats
    public PlayerStat_Endurance endurance { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        playerGroundCheck = GetComponent<PlayerGroundCheck>();
        playerController = GetComponent<PlayerController>();

        playerGroundCheck.MyAwake();
        playerController.MyAwake();

        //Load data
        //S_SaveSystem.
    }

    void Start()
    {
        //Stats
        endurance = PlayerStat_Endurance.Instance(11);
        endurance.MyStart();
    }
}
