using UnityEngine;
using static StaticData.S_GameManager;

public class UI_Pause : MonoBehaviour
{
    private bool _isMyAwake;

    public UI_Option UIOption { get => _uiOption; }
    [SerializeField] private UI_Option _uiOption;

    [Header("NavigationButton")]
    [SerializeField] private GameObject _navigationButton;
    [SerializeField] private UI_Button _resume;
    [SerializeField] private UI_Button _option;
    [SerializeField] private UI_Button _mainMenu;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            _uiOption.MyAwake(false);
            Resume();
        }
    }

    void OnEnable()
    {
        //Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnDisable()
    {
        DeactiveAllPanels();
        //Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DeactiveAllPanels()
    {
        _uiOption.gameObject.SetActive(false);
        _navigationButton.SetActive(true);
    }

    public void ActiveOption() => _uiOption.gameObject.SetActive(true);

    public void Resume()
    {
        gameObject.SetActive(false);
    }

    public void Option()
    {
        _navigationButton.SetActive(false);
        _uiOption.gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        Fader.Instance.ToScene(InfoScene.MainMenu);
    }
}