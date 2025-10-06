using UnityEngine.SceneManagement;

public class Key
{
    private int _key = 0;
    public int GetKey() => _key;

    public bool SetKey(int key)
    {
        if (SceneManager.GetActiveScene().buildIndex == MainMenuManager.SceneIndex)
        {
            _key = key;
            return true;
        }
        return false;
    }
}