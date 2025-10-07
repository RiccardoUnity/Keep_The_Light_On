using System;
using System.Collections.Generic;
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

    protected override void Awake()
    {
        base.Awake();

        //Generate key for class in the game
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
            while (key != 0);
            action.Invoke(key);
        }
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
}
