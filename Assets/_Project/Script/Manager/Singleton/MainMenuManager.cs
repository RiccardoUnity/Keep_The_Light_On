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
    public UI_Credits Credits { get =>  _uiCredits; }
    [SerializeField] private UI_Credits _uiCredits;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log(Application.persistentDataPath);

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
        PS_Life.Key.SetKey(key);
        PS_Endurance.Key.SetKey(key);
        PS_Rest.Key.SetKey(key);
        PS_Hunger.Key.SetKey(key);
        PS_Thirst.Key.SetKey(key);
        PS_Stamina.Key.SetKey(key);
        PS_SunStroke.Key.SetKey(key);
        PS_StomacStroke.Key.SetKey(key);
        PS_HeartStroke.Key.SetKey(key);
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
}
