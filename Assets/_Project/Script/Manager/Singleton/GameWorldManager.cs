using System.Collections;
using UnityEngine;

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

    //[Header("Pool Manager")]
    public PoolManager PoolManager { get; private set; }

    [Header("Main GameObject")]
    [SerializeField] private Light _mainLight;
    public Transform mainLight { get => _mainLight.transform; }
    
    public LayerMask blockMainLight { get => _blockMainLight; }
    [SerializeField] private LayerMask _blockMainLight = (1 << 0);
    public QueryTriggerInteraction qti { get => _qti; }
    private QueryTriggerInteraction _qti = QueryTriggerInteraction.Ignore;

    [Header("Asset")]
    [SerializeField] private SO_ItemManager _SOItemManager;

    protected override void Awake()
    {
        base.Awake();

        //Check error
        if (_mainLight == null)
        {
            _hasAnError = true;
            Debug.LogError("Main Light missing", gameObject);
        }

        //Safe execute
        if (!_hasAnError)
        {
            //Create Manager
            TimeManager = TimeManager.Instance(Key.GetKey());
            PoolManager = PoolManager.Instance(Key.GetKey());

            //Load internal data
            if (S_SaveSystem.HasALoading)
            {
                S_SaveSystem.LoadGameWorldManager();
            }
            else
            {

            }
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
}