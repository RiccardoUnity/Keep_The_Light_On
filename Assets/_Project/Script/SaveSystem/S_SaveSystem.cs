using System.IO;
using UnityEngine;

public static class S_SaveSystem
{
    private static bool _debug;
    private static int _slot = -1;
    private static string _pathRepoSlot;
    private static string _fileJSonString;

    private static Save_GameWorldManager _saveGameWorldManager;
    private static Save_Player _savePlayer;

    public static bool SetSlot(int slot)
    {
        if (_slot < 0)
        {
            _slot = slot;
            _pathRepoSlot = Application.persistentDataPath + $"/{_slot.ToString()}";

            //Init all classes
            _saveGameWorldManager = new Save_GameWorldManager();
            return true;
        }
        else
        {
            Debug.LogWarning("SaveSlot to reset slot and change it");
            return false;
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
            try
            {
                if (File.Exists(_pathRepoSlot + $"/{nameof(Save_GameWorldManager)}"))
                {
                    _fileJSonString = File.ReadAllText(_pathRepoSlot + $"/{nameof(Save_GameWorldManager)}");
                    _saveGameWorldManager = JsonUtility.FromJson<Save_GameWorldManager>(_fileJSonString);
                    if (_debug)
                    {
                        Debug.Log("Save_GameScene loaded");
                    }
                }
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
            //Save classes
            _saveGameWorldManager.Save();

            //Write save classes
            try
            {
                _fileJSonString = JsonUtility.ToJson(_saveGameWorldManager);
                File.WriteAllText(_pathRepoSlot + $"/{nameof(Save_GameWorldManager)}", _fileJSonString);
                if (_debug)
                {
                    Debug.Log("Save_GameScene saved");
                }

            }
            catch
            {
                Debug.LogError("Fatal Error while Saving");
            }

            //Exit from game
            if (exit)
            {
                _slot = -1;
                _pathRepoSlot = "";
            }
            return true;
        }
    }

    public static bool LoadGameWorldManager()
    {
        if (_slot < 0)
        {
            Debug.LogWarning("Can't LoadGameWorldManager without set the slot");
            return false;
        }
        else
        {
            _saveGameWorldManager.Load();
            if (_debug)
            {
                Debug.Log("Save_GameWorldManager loaded in scene");
            }
            return true;
        }
    }

    public static bool LoadPlayerManager()
    {
        if (_slot < 0)
        {
            Debug.LogWarning("Can't LoadPlayerManager without set the slot");
            return false;
        }
        else
        {
            _savePlayer.Load();
            if (_debug)
            {
                Debug.Log("Save_Player loaded in scene");
            }
            return true;
        }
    }
}