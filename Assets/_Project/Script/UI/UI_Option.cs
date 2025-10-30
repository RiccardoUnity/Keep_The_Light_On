using UnityEngine;
using UnityEngine.UI;

public class UI_Option : MonoBehaviour
{
    [Header("Option")]
    private bool _isMyAwake;
    private bool _isInMainMenu;

    public bool InvertMouseY { get => _invertMouseY.Toggle.isOn; }
    [SerializeField] private UI_Toggle _invertMouseY;
    public bool InvertMouseX { get => _invertMouseX.Toggle.isOn; }
    [SerializeField] private UI_Toggle _invertMouseX;
    public float MouseSensitivity { get => _mouseSensitivity.Slider.value; }
    [SerializeField] private UI_Slider _mouseSensitivity;
    public Slider VolumeMaster { get => _volumeMaster.Slider; }
    [SerializeField] private UI_Slider _volumeMaster;
    public Slider VolumeMusic { get => _volumeMusic.Slider; }
    [SerializeField] private UI_Slider _volumeMusic;
    public float VolumeVFX { get => _volumeVFX.Slider.value; }
    [SerializeField] private UI_Slider _volumeVFX;

    [Header("Navigation Button")]
    [SerializeField] private UI_Button _back;
    [SerializeField] private UI_Button _saveOption;
    [SerializeField] private UI_Button _resetOption;
    [SerializeField] private UI_Button _extra;

    private void Load()
    {
        bool iMV;
        bool iMH;
        float mS;
        float vMaster;
        float vMusic;
        float vVFX;
        S_SaveSystem.LoadOption(out iMV, out iMH, out mS, out vMaster, out vMusic, out vVFX);
        _invertMouseY.SetValue(iMV);
        _invertMouseX.SetValue(iMH);
        _mouseSensitivity.SetValue(mS);
        _volumeMaster.SetValue(vMaster);
        _volumeMusic.SetValue(vMusic);
        _volumeVFX.SetValue(vVFX);
    }

    public void MyAwake(bool isInMainMenu)
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;
            _isInMainMenu = isInMainMenu;
            Load();
        }
    }

    private void OnEnable()
    {
        Load();
    }

    public void Back()
    {
        if (_isInMainMenu)
        {
            BackInMainMenu();
        }
        else
        {
            BackInGame();
        }
    }

    private void BackInMainMenu()
    {
        MainMenuManager.Instance.DeactiveAllPanels();
        MainMenuManager.Instance.ActiveMainMenu();
    }

    private void BackInGame()
    {
        GameWorldManager.Instance.UIPause.DeactiveAllPanels();
    }

    public void SaveOption()
    {
        bool iMV = _invertMouseY.Toggle.isOn;
        bool iMH = _invertMouseX.Toggle.isOn;
        float mS = _mouseSensitivity.Slider.value;
        float vMaster = _volumeMaster.Slider.value;
        float vMusic = _volumeMusic.Slider.value;
        float vVFX = _volumeVFX.Slider.value;
        S_SaveSystem.SaveOption(_isInMainMenu, iMV, iMH, mS, vMaster, vMusic, vVFX);

        Back();
    }

    public void ResetOption()
    {
        S_SaveSystem.ResetOption(_isInMainMenu);

        Back();
    }

    public void Extra()
    {
        if (_isInMainMenu)
        {
            ResetAllGame();
        }
        else
        {

        }
    }

    private void ResetAllGame()
    {
        
    }
}