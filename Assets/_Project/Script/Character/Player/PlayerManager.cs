using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerGroundCheck))]
[RequireComponent(typeof(PlayerController))]
public class PlayerManager : SingletonGeneric<PlayerManager>
{
    #region Singleton
    protected override bool ShouldBeDestroyOnLoad() => true;

    static PlayerManager()
    {
        _useResources = false;
        _resourcesPath = "";
    }
    #endregion

    //Componenti
    public PlayerGroundCheck playerGroundCheck { get; private set; }
    public PlayerController playerController { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        playerGroundCheck = GetComponent<PlayerGroundCheck>();
        playerController = GetComponent<PlayerController>();

        playerGroundCheck.MyAwake();
        playerController.MyAwake();
    }
}
