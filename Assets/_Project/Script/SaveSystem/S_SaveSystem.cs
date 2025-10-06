using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class S_SaveSystem
{
    private static bool _debug;
    private static int _slot = -1;
    private static string _pathRepoSlot;
    private static string _fileJSonString;
    private static bool _loadingComplete;

    private static Save_GameWorldManager _saveGameWorldManager;
    private static Save_Player _savePlayer;

    public static void SetDebug(bool value) => _debug = value;

    public static bool SetSlot(int slot)
    {
        if (_slot < 0)
        {
            _slot = slot;
            _pathRepoSlot = Application.persistentDataPath + $"/{_slot.ToString()}";
            return true;
        }
        else
        {
            Debug.LogWarning("SaveSlot to reset slot and change it");
            return false;
        }
    }

    private static void InitClasses()
    {
        if (_saveGameWorldManager == null)
        {
            _saveGameWorldManager = new Save_GameWorldManager();
        }
        if (_savePlayer == null)
        {
            _savePlayer = new Save_Player(PlayerManager.Instance.transform, PlayerManager.Instance);
        }
    }

    public static bool LoadSlot()
    {
        if (_slot < 0)
        {
            Debug.LogWarning("Can't LoadSlot without set the slot");
            return false;
        }
        else
        {
            InitClasses();
            try
            {
                //Save_GameWorldManager
                if (File.Exists(_pathRepoSlot + $"/{nameof(Save_GameWorldManager)}.txt"))
                {
                    _fileJSonString = File.ReadAllText(_pathRepoSlot + $"/{nameof(Save_GameWorldManager)}.txt");
                    _saveGameWorldManager = JsonUtility.FromJson<Save_GameWorldManager>(_fileJSonString);
                    if (_debug)
                    {
                        Debug.Log("Save_GameScene loaded");
                    }
                }
                //Save_Player
                if (File.Exists(_pathRepoSlot + $"/{nameof(Save_Player)}.txt"))
                {
                    _fileJSonString = File.ReadAllText(_pathRepoSlot + $"/{nameof(Save_Player)}.txt");
                    _savePlayer = JsonUtility.FromJson<Save_Player>(_fileJSonString);
                    if (_debug)
                    {
                        Debug.Log("Save_Player loaded");
                    }
                }
                _loadingComplete = true;
            }
            catch
            {
                Debug.LogError("Fatal Error while Loading");
            }

            return true;
        }
    }

    public static bool SaveSlot(bool exit = false)
    {
        if (_slot < 0)
        {
            Debug.LogWarning("Can't SaveSlot without set the slot");
            return false;
        }
        else
        {
            InitClasses();

            //Save classes
            _saveGameWorldManager.Save();

            //Write save classes
            try
            {
                //Save_GameWorldManager
                _fileJSonString = JsonUtility.ToJson(_saveGameWorldManager);
                File.WriteAllText(_pathRepoSlot + $"/{nameof(Save_GameWorldManager)}.txt", _fileJSonString);
                if (_debug)
                {
                    Debug.Log("Save_GameScene saved");
                }
                //Save_Player
                _fileJSonString = JsonUtility.ToJson(_savePlayer);
                File.WriteAllText(_pathRepoSlot + $"/{nameof(Save_Player)}.txt", _fileJSonString);
                if (_debug)
                {
                    Debug.Log("Save_GameScene saved");
                }
            }
            catch
            {
                Debug.LogError("Fatal Error while Saving");
            }

            //Exit to MainMenu
            if (exit)
            {
                _slot = -1;
                _pathRepoSlot = "";
                _fileJSonString = "";
                _loadingComplete = false;

                _saveGameWorldManager = null;
                _savePlayer = null;
            }
            return true;
        }
    }

    public static bool LoadGameWorldManager()
    {
        if (_loadingComplete)
        {
            _saveGameWorldManager.Load();
            if (_debug)
            {
                Debug.Log("Save_GameWorldManager loaded in scene");
            }
            return true;
        }
        else
        {
            Debug.LogWarning("There isn't Save_GameWorldManager to load");
            return false;
        }
    }

    public static bool LoadPlayerManager()
    {
        if (_loadingComplete)
        {
            _savePlayer.Load();
            if (_debug)
            {
                Debug.Log("Save_Player loaded in scene");
            }
            return true;
        }
        else
        {
            Debug.LogWarning("There isn't Save_Player to load");
            return false;
        }
    }


}