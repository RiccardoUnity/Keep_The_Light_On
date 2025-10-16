using System;
using System.IO;
using UnityEngine;

public static partial class S_SaveSystem
{
    [Serializable]
    private class Save_Option
    {
        public int _lastSlotUsed = -1;

        //Mouse
        public bool _invertMouseVertical = true;
        public bool _invertMouseHorizontal;
        public float _mouseSensitivity = 0.5f;

        //Volume
        public float _volumeMaster = 1f;
        public float _volumeMusic = 1f;
        public float _volumeVFX = 1f;
    }

    private static Save_Option _saveOption;
    private static string _pathOptionExtension = "/option.txt";

    //I make sure the class exists
    private static void InitSaveOption()
    {
        string path = Application.persistentDataPath + _pathOptionExtension;
        if (File.Exists(path))
        {
            _fileJSonString = File.ReadAllText(path);
            _saveOption = JsonUtility.FromJson<Save_Option>(_fileJSonString);
        }
        else
        {
            _saveOption = new Save_Option();
        }
    }

    //Resume in MainMenu
    public static int LastSlotUsed()
    {
        if (_saveOption == null)
        {
            InitSaveOption();
        }

        return _saveOption._lastSlotUsed;
    }

    public static void LoadOption(out bool iMV, out bool iMH, out float mS, out float vMaster, out float vMusic, out float vVFX)
    {
        if (_saveOption == null)
        {
            InitSaveOption ();
        }

        iMV = _saveOption._invertMouseVertical;
        iMH = _saveOption._invertMouseHorizontal;
        mS = _saveOption._mouseSensitivity;
        vMaster = _saveOption._volumeMaster;
        vMusic = _saveOption._volumeMusic;
        vVFX = _saveOption._volumeVFX;
    }

    public static void SaveOption(bool isInMainMenu, bool iMV, bool iMH, float mS, float vMaster, float vMusic, float vVFX)
    {
        if (_saveOption == null)
        {
            InitSaveOption();
        }

        _saveOption._invertMouseVertical = iMV;
        _saveOption._invertMouseHorizontal = iMH;
        mS = Mathf.Clamp01(mS);
        _saveOption._mouseSensitivity = mS;
        vMaster = Mathf.Clamp01(vMaster);
        _saveOption._volumeMaster = vMaster;
        vMusic = Mathf.Clamp01(vMusic);
        _saveOption._volumeMusic = vMusic;
        vVFX = Mathf.Clamp01(vVFX);
        _saveOption._volumeVFX = vVFX;

        WriteSaveOption(isInMainMenu);
    }

    public static void ResetOption(bool isInMainMenu)
    {
        _saveOption = new Save_Option();

        WriteSaveOption(isInMainMenu);
    }

    private static void WriteSaveOption(bool isInMainMenu)
    {
        if (!isInMainMenu)
        {
            _saveOption._lastSlotUsed = _slot;
        }

        string path = Application.persistentDataPath + _pathOptionExtension;
        _fileJSonString = JsonUtility.ToJson(_saveOption, true);
        File.WriteAllText(path, _fileJSonString);
    }
}
