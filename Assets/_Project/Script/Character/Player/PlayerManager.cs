using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GTM = TimeManager;

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
    public static Key Key = new Key();
    public PlayerStat_Endurance endurance { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        playerGroundCheck = GetComponent<PlayerGroundCheck>();
        playerController = GetComponent<PlayerController>();

        playerGroundCheck.MyAwake();
        playerController.MyAwake();

        //Stats
        endurance = PlayerStat_Endurance.Instance(Key.GetKey());

        //Load internal data
        if (S_SaveSystem.HasALoading)
        {
            S_SaveSystem.LoadPlayerManager();
        }
        else
        {

        }
    }

    void Start()
    {
        //Stats
        endurance.MyStart();
    }
}
