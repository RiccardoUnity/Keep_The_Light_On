using UnityEngine;
using InfoScene = StaticData.S_GameManager.InfoScene;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UI_MainMenu : MonoBehaviour
{
    private bool _isMyAwake;

    [Header("Buttons")]
    [SerializeField] private UI_Button _resume;
    [SerializeField] private UI_Button _newGame;
    [SerializeField] private UI_Button _loadGame;
    [SerializeField] private UI_Button _option;
    [SerializeField] private UI_Button _credits;
    [SerializeField] private UI_Button _exit;

    private int _lastSlot;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //Resume Init
            _lastSlot = S_SaveSystem.LastSlotUsed();
            //if (_lastSlot < 0)
            //{
            //    _resume.SetActive(false);
            //}
            _resume.SetActive(false);
            _loadGame.SetActive(false);

            //Load Option
            MainMenuManager.Instance.UIOption.MyAwake(true);
        }
    }

    public void Resume()
    {
        
    }

    public void NewGame()
    {
        S_SaveSystem.NewSlot();
        //Load scene
        Fader.Instance.ToScene(InfoScene.GameWorld);
    }

    public void LoadGame()
    {

    }

    public void Option()
    {
        MainMenuManager.Instance.DeactiveAllPanels();
        MainMenuManager.Instance.ActiveOption();
    }

    public void Credits()
    {
        MainMenuManager.Instance.DeactiveAllPanels();
        MainMenuManager.Instance.ActiveCredits();
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
