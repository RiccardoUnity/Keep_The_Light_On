using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MainMenuManager : Singleton_Generic<MainMenuManager>
{
    #region Singleton
    protected override bool ShouldBeDestroyOnLoad() => true;

    static MainMenuManager()
    {
        _useResources = false;
        _resourcesPath = "";
    }
    #endregion

    public static int SceneIndex { get; private set; }

    public UI_MainMenu UIMainMenu { get => _uiMainMenu; }
    [SerializeField] private UI_MainMenu _uiMainMenu;
    public UI_Option UIOption { get => _uiOption; }
    [SerializeField] private UI_Option _uiOption;
    public UI_Credits Credits { get =>  _uiCredits; }
    [SerializeField] private UI_Credits _uiCredits;

    [SerializeField] private GameObject _imageExit;

    private AudioSource _audioSource;
    private float _volumeMax;
    private float _volume;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log(Application.persistentDataPath);
        _imageExit.SetActive(false);

        _audioSource = GetComponent<AudioSource>();
        _volumeMax = _audioSource.volume;
        ChangeVolume(0f);
        UIOption.VolumeMaster.onValueChanged.AddListener(ChangeVolume);
        UIOption.VolumeMusic.onValueChanged.AddListener(ChangeVolume);

        //Generate keys for class in the game
        SceneIndex = SceneManager.GetActiveScene().buildIndex;

        int key = 0;
        Action<int> actions = null;
        actions += SetKeyGameWorldManager;
        actions += SetKeyPlayerManager;

        foreach (Action<int> action in actions.GetInvocationList())
        {
            do
            {
                key = Random.Range(int.MinValue, int.MaxValue);
            }
            while (key == 0);
            action.Invoke(key);
        }

        //Init buttons MainMenu
        DeactiveAllPanels();
        ActiveMainMenu();
        _uiMainMenu.MyAwake();
    }

    private void SetKeyGameWorldManager(int key)
    {
        GameWorldManager.Key.SetKey(key);
        TimeManager.Key.SetKey(key);
        PoolManager.Key.SetKey(key);
    }

    private void SetKeyPlayerManager(int key)
    {
        PlayerManager.Key.SetKey(key);
        PlayerStat.Key.SetKey(key);
        PlayerInventory.Key.SetKey(key);
    }


    //Panels
    public void DeactiveAllPanels()
    {
        _uiMainMenu.gameObject.SetActive(false);
        _uiOption.gameObject.SetActive(false);
        _uiCredits.gameObject.SetActive(false);
    }

    public void ActiveMainMenu() => _uiMainMenu.gameObject.SetActive(true);

    public void ActiveOption() => _uiOption.gameObject.SetActive(true);

    public void ActiveCredits() => _uiCredits.gameObject.SetActive(true);

    private void ChangeVolume(float notUse)
    {
        float lerpMusic = Mathf.Lerp(0f, UIOption.VolumeMaster.value, UIOption.VolumeMusic.value);
        _volume = Mathf.Lerp(0f, _volumeMax, lerpMusic);
        _audioSource.volume = _volume;
    }

    public void SwichOffMusic()
    {
        StartCoroutine(StopMusic());
    }

    private IEnumerator StopMusic()
    {
        float maxTime = 2f;
        float currentTime = 0f;
        while (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(0f, _volume, 1f -  currentTime / maxTime);

            yield return null;
        }

        _audioSource.Stop();
    }

    private void OnApplicationQuit()
    {
        _imageExit.SetActive(true);
    }
}
