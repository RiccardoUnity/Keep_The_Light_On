using System;
using System.Collections.Generic;
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
    public RectTransform Credits { get =>  _credits; }
    [SerializeField] private RectTransform _credits;

    protected override void Awake()
    {
        base.Awake();

        //Generate keys for class in the game
        SceneIndex = SceneManager.GetActiveScene().buildIndex;

        int key = 0;
        List<Action<int>> actions = new List<Action<int>>();
        actions.Add(SetKeyGameWorldManager);
        actions.Add(SetKeyPlayerManager);

        foreach (Action<int> action in actions)
        {
            do
            {
                key = Random.Range(int.MinValue, int.MaxValue);
            }
            while (key == 0);
            action.Invoke(key);
        }

        Debug.Log(Application.persistentDataPath);

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
        PlayerStat_Endurance.Key.SetKey(key);
    }


    //Panels
    public void DeactiveAllPanels()
    {
        _uiMainMenu.gameObject.SetActive(false);
        _uiOption.gameObject.SetActive(false);
    }

    public void ActiveMainMenu() => _uiMainMenu.gameObject.SetActive(true);

    public void ActiveOption() => _uiOption.gameObject.SetActive(true);
}
