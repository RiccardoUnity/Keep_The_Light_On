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

    [Header("Time Manager")]
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
    public Transform mainLight { get => _mainLight.transform; }
    
    public LayerMask blockMainLight { get => _blockMainLight; }
    [SerializeField] private LayerMask _blockMainLight = (1 << 0);
    public QueryTriggerInteraction qti { get => _qti; }
    private QueryTriggerInteraction _qti = QueryTriggerInteraction.Ignore;

    public PlayerManager PlayerManager { get => _playerManager; }
    [SerializeField] private PlayerManager _playerManager;

    [Header("UI")]
    [SerializeField] private UI_Pause _uiPause;
    [SerializeField] private UI_Stats _uiStats;
    public UI_Pause UIPause { get => _uiPause; }

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
        if (_soItemManager == null)
        {
            _hasAnError = true;
            Debug.LogError("SO Item Manager missing", gameObject);
        }

        //Safe execute
        if (!_hasAnError)
        {
            //Create Manager
            TimeManager = TimeManager.Instance(Key.GetKey());
            PoolManager = PoolManager.Instance(Key.GetKey());

            //Load data
            if (S_SaveSystem.HasALoading)
            {
                S_SaveSystem.LoadGameWorldManager();
            }
            else
            {

            }

            //Player (included Player Load in MyAwake)
            _playerManager.MyAwake();

            //UI
            _uiPause.MyAwake();
            _uiStats.MyAwake();
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
            TimeManager.MyStart(_mainLight, out _time);
            _hasTimeAlreadyStart = true;
            StartCoroutine(_time);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown(StringConst.Escape))
        {
            _uiPause.gameObject.SetActive(!_uiPause.gameObject.activeSelf);
        }
    }
}