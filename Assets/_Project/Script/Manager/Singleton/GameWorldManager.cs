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

    private bool _hasAnError;

    [Header("Game Time")]
    [SerializeField] private bool _isNewGame = true;
    public bool isNewGame { get => _isNewGame; }
    [SerializeField] private int _startHours;
    public int startHours { get => _startHours; }
    [SerializeField] private int _startMinutes;
    public int startMinutes { get => _startMinutes; }
    [SerializeField] private int _gameDayInRealMinutes = 30;
    public int gameDayInRealMinutes { get => _gameDayInRealMinutes; }
    private bool _hasTimeAlreadyStart;
    public TimeGame timeGame { get; private set; }
    private static int GenerateKeyTimeGame() => 10;
    private IEnumerator _time;

    [Header("Main GameObject")]
    [SerializeField] private Light _mainLight;
    public Transform mainLight { get => _mainLight.transform; }
    [SerializeField] private LayerMask _blockMainLight = (1 << 0);
    public LayerMask blockMainLight { get => _blockMainLight; }
    private QueryTriggerInteraction _qti = QueryTriggerInteraction.Ignore;
    public QueryTriggerInteraction qti { get => _qti; }

    [Header("Asset")]
    [SerializeField] private ScriptableObject _mainSO;

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
            timeGame = TimeGame.Instance(GenerateKeyTimeGame());

            //Load data
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