using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Disclaimer : MonoBehaviour
{
    [Range(1f, 10f)] [SerializeField] private float _awaitSecond;
    [SerializeField] private UI_Button _skip;

    IEnumerator Start()
    {
        _skip.gameObject.SetActive(false);
        yield return new WaitForSeconds(_awaitSecond);
        _skip.gameObject.SetActive(true);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
