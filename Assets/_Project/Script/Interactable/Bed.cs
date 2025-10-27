using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public class Bed : Interactable
{
    private PlayerManager _playerManager;

    void Start()
    {
        _playerManager = GWM.Instance.PlayerManager;
    }

    public void OpenUI()
    {
        GWM.Instance.UIInventory.gameObject.SetActive(true);
        GWM.Instance.UIInventory.OpenUIBed();
        GWM.Instance.UIStats.gameObject.SetActive(false);
    }
}
