using UnityEngine;

public class UI_Illnesses : MonoBehaviour
{
    private bool _isMyAwake;
    [SerializeField] private bool _debug;

    private PlayerManager _playerManager;

    //Stats
    private PS_SunStroke _sunStroke;
    private PS_StomachAche _stomachAche;
    private PS_HeartAche _heartAche;
    private PlayerStat[] _playerIllnesses;

    //UI
    [Header("UI")]
    [SerializeField] private UI_ImageStroke _uiSunStroke;
    [SerializeField] private UI_ImageStroke _uiStomachAche;
    [SerializeField] private UI_ImageStroke _uiHeartAche;
    private UI_ImageStroke[] _uiIllnesses;

    [Header("Settings")]
    [SerializeField] private Color _goodColor = Color.white;
    [SerializeField] private string _goodText;
    [SerializeField] private Color _warningColor = Color.yellow;
    [SerializeField] private string _warningText;
    [SerializeField] private Color _badColor = Color.red;
    [SerializeField] private string _badText;

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
            _sunStroke = _playerManager.SunStroke;
            _stomachAche = _playerManager.StomachAche;
            _heartAche = _playerManager.HeartAche;
            _playerIllnesses = new PlayerStat[] { _sunStroke, _stomachAche, _heartAche };
            _uiIllnesses = new UI_ImageStroke[] { _uiSunStroke, _uiStomachAche, _uiHeartAche };
        }
    }

    public void MyUpdate(float timeDelay)
    {
        if (gameObject.activeSelf)
        {
            //Update All Illnesses
            float value;
            for (int i = 0; i < _playerIllnesses.Length; ++i)
            {
                value = _playerIllnesses[i].Value;
                _uiIllnesses[i].FillImage(value);
                if (value > 0f && value < 1f)
                {
                    _uiIllnesses[i].ChangeColor(_warningColor);
                    _uiIllnesses[i].Text.text = _warningText;
                }
                else if (value == 1f)
                {
                    _uiIllnesses[i].ChangeColor(_badColor);
                    _uiIllnesses[i].Text.text = _badText;
                }
                else if (value == 0f)
                {
                    _uiIllnesses[i].ChangeColor(_goodColor);
                    _uiIllnesses[i].Text.text = _goodText;
                }
            }
        }
    }
}
