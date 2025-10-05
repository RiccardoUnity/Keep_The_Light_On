using UnityEngine;

//Connect static class / asset / gameObject in scene
public class GameSceneManager : Singleton_Generic<GameSceneManager>
{
    #region Singleton
    protected override bool ShouldBeDestroyOnLoad() => true;

    static GameSceneManager()
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
    private bool _isTimeAlreadyStart;

    [Header("Main GameObject")]
    [SerializeField] private Light _mainLight;

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
            //Load data in scene
            S_SaveSystem.LoadInScene();
        }
    }

    void OnEnable()
    {
        if (!_hasAnError)
        {
            if (_isTimeAlreadyStart)
            {
                S_TimeManager.SetStartRotationMainLight(ref _mainLight);
                //StartCoroutine();
            }
        }
    }

    void Start()
    {
        if (!_hasAnError)
        {
            //Time Start
            if (S_TimeManager.currentSecondDay != 0 || S_TimeManager.currentDay != 0)
            {
                _isNewGame = false;
            }
            S_TimeManager.SetUp();
        }
    }
}