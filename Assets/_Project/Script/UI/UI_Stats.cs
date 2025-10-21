using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Stats : MonoBehaviour
{
    private bool _isMyAwake;
    [SerializeField] private bool _debug;

    private PlayerManager _playerManager;

    //Stats
    private PS_Life _life;
    private PS_Endurance _endurance;
    private PS_Rest _rest;
    private PS_Hunger _hunger;
    private PS_Thirst _thirst;
    private PS_Stamina _stamina;
    private PlayerStat[] _playerStats = new PlayerStat[6];

    //UI
    [Header("UI")]
    [SerializeField] private Image _uiLife;
    [SerializeField] private Image _uiEndurance;
    [SerializeField] private Image _uiRest;
    [SerializeField] private Image _uiHunger;
    [SerializeField] private Image _uiThirst;
    [SerializeField] private Image _uiStamina;
    private Image[] _uiStats = new Image[6];

    [Header("Settings")]
    [Range(0f, 1f)] [SerializeField] private float _limitColor = 0.25f;
    [SerializeField] private Color _goodColor = Color.white;
    [SerializeField] private Color _badColor = Color.red;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            GameWorldManager.Instance.TimeManager.onPriority += MyUpdate;

            _playerManager = GameWorldManager.Instance.PlayerManager;
            _life = _playerManager.Life;
            _endurance = _playerManager.Endurance;
            _rest = _playerManager.Rest;
            _hunger = _playerManager.Hunger;
            _thirst = _playerManager.Thirst;
            _stamina = _playerManager.Stamina;

            _playerStats[0] = _life;
            _playerStats[1] = _endurance;
            _playerStats[2] = _rest;
            _playerStats[3] = _hunger;
            _playerStats[4] = _thirst;
            _playerStats[5] = _stamina;

            _uiStats[0] = _uiLife;
            _uiStats[1] = _uiEndurance;
            _uiStats[2] = _uiRest;
            _uiStats[3] = _uiHunger;
            _uiStats[4] = _uiThirst;
            _uiStats[5] = _uiStamina;

            gameObject.SetActive(true);
        }
    }

    public void MyUpdate(float timeDelay)
    {
        //Update All Stats
        for (int i = 0; i < _playerStats.Length; ++i)
        {
            _uiStats[i].fillAmount = _playerStats[i].Value;
            if (_uiStats[i].fillAmount > _limitColor && _uiStats[i].color != _goodColor)
            {
                _uiStats[i].color = _goodColor;
            }
            else if (_uiStats[i].fillAmount < _limitColor && _uiStats[i].color != _badColor)
            {
                _uiStats[i].color = _badColor;
            }
        }
    }
}