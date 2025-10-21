using System.Collections;
using UnityEngine;
using StringConst = StaticData.S_GameManager.StringConst;

//Connect class / static class / asset / gameObject in scene
public class GameWorldManager : Singleton_Generic<GameWorldManager>
{
    #region Singleton
    protected override bool ShouldBeDestroyOnLoad() => true;

    static GameWorldManager()
    {
        _useResources = false;
        _resourcesPath = "";
    }
    #endregion

    public static Key Key = new Key();

    private bool _hasAnError;
    public bool MainDebug { get => _mainDebug; }
    [SerializeField] private bool _mainDebug;

    public bool IsGamePause { get; private set; }

    [Header("Time Manager")]
    [SerializeField] private bool _debugTime;
    private bool _hasTimeAlreadyStart;
    private IEnumerator _time;
    public TimeManager TimeManager { get; private set; }
    public int StartHours { get => _startHours; }
    [SerializeField] private int _startHours;
    public int StartMinutes { get => _startMinutes; }
    [SerializeField] private int _startMinutes;
    public int GameDayInRealMinutes { get => _gameDayInRealMinutes; }
    [SerializeField] private int _gameDayInRealMinutes = 30;

    [Header("Main GameObject")]
    [SerializeField] private Light _mainLight;
    public Transform MainLight { get => _mainLight.transform; }

    public QueryTriggerInteraction Qti { get => _qti; }
    private QueryTriggerInteraction _qti = QueryTriggerInteraction.Ignore;

    public PlayerManager PlayerManager { get => _playerManager; }
    [SerializeField] private PlayerManager _playerManager;

    [Header("UI")]
    private bool _null;
    public UI_Pause UIPause { get => _uiPause; }
    [SerializeField] private UI_Pause _uiPause;
    public UI_Stats UIStats { get => _uiStats; }
    [SerializeField] private UI_Stats _uiStats;
    public UI_Scope UIScope { get => _uiScope; }
    [SerializeField] private UI_Scope _uiScope;
    public UI_Inventory UIInventory { get => _uiInventory; }
    [SerializeField] private UI_Inventory _uiInventory;

    [Header("Asset")]
    [SerializeField] private SO_ItemManager _soItemManager;

    //Other
    public PoolManager PoolManager { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Debug.Log(Application.persistentDataPath);

        //Check error
        if (_mainLight == null)
        {
            _hasAnError = true;
            Debug.LogError("Main Light missing", gameObject);
        }
        if (_playerManager == null)
        {
            _hasAnError = true;
            Debug.LogError("Player Manager missing", gameObject);
        }
        if (_uiPause == null)
        {
            _hasAnError = true;
            Debug.LogError("UI Pause missing", gameObject);
        }
        if (_uiInventory == null)
        {
            _hasAnError = true;
            Debug.LogError("UI Inventory missing", gameObject);
        }
        if (_soItemManager == null)
        {
            _hasAnError = true;
            Debug.LogError("SO Item Manager missing", gameObject);
        }

        //Safe execute
        if (!_hasAnError)
        {
            //Create Manager
            TimeManager = TimeManager.Instance(Key.GetKey(), _debugTime);
            PoolManager = PoolManager.Instance(Key.GetKey());

            //Load data
            if (S_SaveSystem.HasALoading)
            {
                S_SaveSystem.LoadGameWorldManager();
            }
            else
            {

            }

            //Awake Manager
            TimeManager.MyAwake(_mainLight, out _time);

            //Player (included Player Load in MyAwake)
            _playerManager.MyAwake();

            //UI
            _uiPause.MyAwake();
            _uiStats.MyAwake();
            _uiScope.MyAwake();
            _uiInventory.MyAwake();
        }
    }

    void OnEnable()
    {
        if (!_hasAnError)
        {
            if (_hasTimeAlreadyStart)
            {
                TimeManager.SetRotationMainLight();
                StartCoroutine(_time);
            }
        }
    }

    void Start()
    {
        if (!_hasAnError)
        {
            //Load external data
            if (S_SaveSystem.HasALoading)
            {
                S_SaveSystem.LoadItems();
            }

            //Time Start
            _hasTimeAlreadyStart = true;
            StartCoroutine(_time);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown(StringConst.Escape))
        {
            if (_uiInventory.gameObject.activeSelf)
            {
                _uiInventory.gameObject.SetActive(false);
                _uiStats.gameObject.SetActive(true);
            }
            else
            {
                if (TimeManager.GameTimeType != GameTimeType.Accelerate)
                {
                    _uiPause.gameObject.SetActive(!_uiPause.gameObject.activeSelf);
                    IsGamePause = _uiPause.gameObject.activeSelf;
                }
            }  
        }

        if (Input.GetButtonDown(StringConst.Inventory))
        {
            _uiInventory.gameObject.SetActive(!_uiInventory.gameObject.activeSelf);
            _uiStats.gameObject.SetActive(!_uiStats.gameObject.activeSelf);
        }
    }

    public bool SetGameInPauseTrue(int key)
    {
        if (key == Key.GetKey())
        {
            IsGamePause = true;
            return true;
        }
        return false;
    }

    public void SetGameInPauseFalse() => IsGamePause = false;
}