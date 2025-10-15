using UnityEngine;

public class UI_Option : MonoBehaviour
{
    [Header("Option")]
    [SerializeField] private UI_Toggle _invertMouseVertical;
    [SerializeField] private UI_Toggle _invertMouseHorizontal;
    [SerializeField] private UI_Slider _mouseSensitivity;
    [SerializeField] private UI_Slider _volumeMaster;
    [SerializeField] private UI_Slider _volumeMusic;
    [SerializeField] private UI_Slider _volumeVFX;

    [Header("Navigation Button")]
    [SerializeField] private UI_Button _back;
    [SerializeField] private UI_Button _saveOption;
    [SerializeField] private UI_Button _resetOption;
    [SerializeField] private UI_Button _extra;

    private bool _isAwakeSetted;
    private bool _isInMainMenu;

    public void MyAwake(bool isInMainMenu)
    {
        if (_isAwakeSetted)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isAwakeSetted = true;
            _isInMainMenu = isInMainMenu;
            bool iMV;
            bool iMH;
            float mS;
            float vMaster;
            float vMusic;
            float vVFX;
            S_SaveSystem.LoadOption(out iMV, out iMH, out mS, out vMaster, out vMusic, out vVFX);
            _invertMouseVertical.SetValue(iMV);
            _invertMouseHorizontal.SetValue(iMH);
            _mouseSensitivity.SetValue(mS);
            _volumeMaster.SetValue(vMaster);
            _volumeMusic.SetValue(vMusic);
            _volumeVFX.SetValue(vVFX);
        }
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

    }

    public void SaveOption()
    {
        bool iMV = _invertMouseVertical.Toggle.isOn;
        bool iMH = _invertMouseHorizontal.Toggle.isOn;
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

    public void ResetAllGame()
    {
        if (_isInMainMenu)
        {

        }
    }

    public void ReturnToMainMenu()
    {
        if (!_isInMainMenu)
        {

        }
    }
}