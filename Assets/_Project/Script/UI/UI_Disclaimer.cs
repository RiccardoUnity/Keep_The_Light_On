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
        yield return new WaitForSeconds(_awaitSecond + Fader.Instance.TimeCoroutine);
        _skip.gameObject.SetActive(true);
    }

    public void ToMainMenu() => Fader.Instance.ToScene(SceneManager.GetActiveScene().buildIndex + 1);
}
