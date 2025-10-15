using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static partial class S_SaveSystem
{
    private static bool _debug;
    private static int _slot = -1;
    private static string _pathRepoSlot;
    private static string _fileJSonString;
    private static string _extention = ".txt";
    public static bool HasALoading { get => _hasALoading; }
    private static bool _hasALoading;

    private static Save_GameWorldManager _saveGameWorldManager;
    private static Save_Player _savePlayer;
    private static Queue<Save_Item> _toLoadItems = new Queue<Save_Item>();

    public static void SetDebug(bool value) => _debug = value;

    //In MainMenu New Game
    public static int NewSlot()
    {
        int index = Directory.GetDirectories(Application.persistentDataPath).Length;
        Directory.CreateDirectory(Application.persistentDataPath + $"/{index.ToString()}");
        _slot = index;
        return index;
    }

    //In MainMenu Load Game
    public static bool SetSlot(int slot)
    {
        if (_slot < 0)
        {
            int index = Directory.GetDirectories(Application.persistentDataPath).Length;
            if (slot >= 0 && slot < index)
            {
                _slot = slot;
                _pathRepoSlot = Application.persistentDataPath + $"/{_slot.ToString()}";
                return true;
            }
        }
        else
        {
            Debug.LogWarning("SaveSlot to reset slot and change it");
        }
        return false;
    }

    private static void InitClasses()
    {
        if (_saveGameWorldManager == null)
        {
            _saveGameWorldManager = new Save_GameWorldManager();
        }
        if (_savePlayer == null)
        {
            _savePlayer = new Save_Player(PlayerManager.Instance);
        }
    }

    //In MainMenu before to load the GameScene
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
                if (File.Exists(_pathRepoSlot + $"/{nameof(Save_GameWorldManager)}{_extention}"))
                {
                    _fileJSonString = File.ReadAllText(_pathRepoSlot + $"/{nameof(Save_GameWorldManager)}{_extention}");
                    _saveGameWorldManager = JsonUtility.FromJson<Save_GameWorldManager>(_fileJSonString);
                    if (_debug)
                    {
                        Debug.Log("Save_GameScene loaded");
                    }
                }
                //Save_Player
                if (File.Exists(_pathRepoSlot + $"/{nameof(Save_Player)}{_extention}"))
                {
                    _fileJSonString = File.ReadAllText(_pathRepoSlot + $"/{nameof(Save_Player)}{_extention}");
                    _savePlayer = JsonUtility.FromJson<Save_Player>(_fileJSonString);
                    if (_debug)
                    {
                        Debug.Log("Save_Player loaded");
                    }
                }
                if (Directory.Exists(_pathRepoSlot + $"/{nameof(Save_Item)}"))
                {
                    string[] files = Directory.GetFiles(_pathRepoSlot + $"/{nameof(Save_Item)}", $".{_extention}");
                    foreach (string file in files)
                    {
                        Save_Item saveItem = new Save_Item(true);
                        saveItem = JsonUtility.FromJson<Save_Item>(file);
                        _toLoadItems.Enqueue(saveItem);
                    }
                    if (_debug)
                    {
                        Debug.Log("Save_Item loaded");
                    }
                }
                _hasALoading = true;
            }
            catch
            {
                Debug.LogError("Fatal Error while Loading");
            }

            return true;
        }
    }

    //In GameScene after Start
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
            _savePlayer.Save();
            foreach(Save_Item saveItem in _saveItems)
            {
                saveItem.Save();
            }

            //Write save classes
            try
            {
                //Save_GameWorldManager
                _fileJSonString = JsonUtility.ToJson(_saveGameWorldManager, true);
                File.WriteAllText(_pathRepoSlot + $"/{nameof(Save_GameWorldManager)}{_extention}", _fileJSonString);
                if (_debug)
                {
                    Debug.Log("Save_GameScene saved");
                }
                //Save_Player
                _fileJSonString = JsonUtility.ToJson(_savePlayer, true);
                File.WriteAllText(_pathRepoSlot + $"/{nameof(Save_Player)}{_extention}", _fileJSonString);
                if (_debug)
                {
                    Debug.Log("Save_GameScene saved");
                }
                //Save_Item
                foreach (Save_Item saveItem in _saveItems)
                {
                    _fileJSonString = JsonUtility.ToJson(saveItem, true);
                    File.WriteAllText(_pathRepoSlot + $"/{nameof(Save_Item)}/{saveItem.Id}{_extention}", _fileJSonString);
                }
            }
            catch
            {
                Debug.LogError("Fatal Error while Saving");
            }

            //Exit from GameScene to MainMenu
            if (exit)
            {
                _slot = -1;
                _pathRepoSlot = "";
                _fileJSonString = "";
                _hasALoading = false;

                _saveGameWorldManager = null;
                _savePlayer = null;
                //Save_Item ...
            }
            return true;
        }
    }

    //In GameScene in Awake
    public static void LoadGameWorldManager()
    {
        _saveGameWorldManager.Load();
        if (_debug)
        {
            Debug.Log("Save_GameWorldManager loaded in scene");
        }
    }

    //In GameScene in Awake
    public static void LoadPlayerManager()
    {
        _savePlayer.Load();
        if (_debug)
        {
            Debug.Log("Save_Player loaded in scene");
        }
    }


}