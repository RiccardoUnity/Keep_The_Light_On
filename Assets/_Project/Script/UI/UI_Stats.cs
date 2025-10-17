using UnityEngine;
using UnityEngine.UI;

public class UI_Stats : MonoBehaviour
{
    private bool _isMyAwake;
    [SerializeField] private bool _debug;

    //Stats
    private PS_Endurance _endurance;

    //UI
    [SerializeField] private Image _uiLife;
    [SerializeField] private Image _uiEndurance;
    [SerializeField] private Image _uiRest;
    [SerializeField] private Image _uiHunger;
    [SerializeField] private Image _uiThirst;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            GameWorldManager.Instance.TimeManager.onNormalPriority += MyUpdate;
            _endurance = GameWorldManager.Instance.PlayerManager.Endurance;
        }
    }

    public void MyUpdate()
    {
        _uiEndurance.fillAmount = _endurance.Value;

        if (_debug)
        {
            Debug.Log("Endurance: " + _endurance.Value);
            Debug.Log(_endurance.HasOnSun);
            Debug.Log(GameWorldManager.Instance.TimeManager.CurrentSecondDay);
        }
    }
}