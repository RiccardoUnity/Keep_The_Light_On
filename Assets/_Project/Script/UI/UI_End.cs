using TMPro;
using UnityEngine;
using static StaticData.S_GameManager;

public class UI_End : MonoBehaviour
{
    private bool _isMyAwake;

    [SerializeField] private TMP_Text _text;
    [SerializeField] private UI_Button _startAgain;
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

            gameObject.SetActive(false);
            GameWorldManager.Instance.PlayerManager.Life.onValueBecomesZero += Death;
        }
    }

    private void Death()
    {
        _text.text = "YOU ARE DEAD";
        Exit();
    }

    public void End()
    {
        _text.text = "YOU SURVIVED, TOP";
        Exit();
    }

    private void Exit()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameWorldManager.Instance.UIStats.gameObject.SetActive(false);
    }

    public void MainMenu()
    {
        Fader.Instance.ToScene(InfoScene.MainMenu);
    }
}
