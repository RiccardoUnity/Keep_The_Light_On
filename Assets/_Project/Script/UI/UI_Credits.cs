using UnityEngine;

public class UI_Credits : MonoBehaviour
{
    public void BackInMainMenu()
    {
        MainMenuManager.Instance.DeactiveAllPanels();
        MainMenuManager.Instance.ActiveMainMenu();
    }
}
