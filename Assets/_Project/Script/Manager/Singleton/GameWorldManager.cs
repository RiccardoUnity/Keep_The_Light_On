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

    [Header("Game Time")]
    private bool _hasTimeAlreadyStart;
    private IEnumerator _time;
    public TimeGame timeGame { get; private set; }
    public bool isNewGame { get => _isNewGame; }
    [SerializeField] private bool _isNewGame = true;
    public int startHours { get => _startHours; }
    [SerializeField] private int _startHours;
    public int startMinutes { get => _startMinutes; }
    [SerializeField] private int _startMinutes;
    public int gameDayInRealMinutes { get => _gameDayInRealMinutes; }
    [SerializeField] private int _gameDayInRealMinutes = 30;

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
            //Create TimeGame
            timeGame = TimeGame.Instance(Key.GetKey());

            //Load internal data
            S_SaveSystem.LoadGameWorldManager();
        }
    }

    void OnEnable()
    {
        if (!_hasAnError)
        {
            if (_hasTimeAlreadyStart)
            {
                timeGame.SetRotationMainLight();
                StartCoroutine(_time);
            }
        }
    }

    void Start()
    {
        if (!_hasAnError)
        {
            //Load external data
            S_SaveSystem.LoadItems();

            //Time Start
            if (timeGame.currentSecondDay != 0 || timeGame.currentDay != 0)
            {
                _isNewGame = false;
            }
            timeGame.MyStart(_mainLight, out _time);
            _hasTimeAlreadyStart = true;
            StartCoroutine(_time);
        }
    }


}