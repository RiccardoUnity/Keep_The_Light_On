using System.Collections;
using UnityEngine;
using GWM = GameWorldManager;

public class UI_Start : MonoBehaviour
{
    private bool _isMyAwake;

    private CanvasGroup _ui;
    [SerializeField] private RectTransform _image;
    private AudioSource _audioSource;

    private float _durationAudio = 22f;
    private float _durationFade = 5f;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            _ui = GetComponent<CanvasGroup>();
            _audioSource = GetComponent<AudioSource>();

            _audioSource.loop = false;
            GWM.Instance.UIInventory.gameObject.SetActive(true);
            GWM.Instance.UIInventory.OpenUIStart();
            GWM.Instance.UIStats.gameObject.SetActive(false);

            StartCoroutine(StartAudio());
        }
    }

    void OnDisable()
    {
        if (_isMyAwake)
        {
            _audioSource.Stop();
        }
    }

    private IEnumerator StartAudio()
    {
        float height = _image.sizeDelta.y;
        Vector2 currentSizeDelta = _image.sizeDelta;
        float currentTime = 0f;
        WaitForSeconds wait = new WaitForSeconds(2f);

        yield return wait;

        while (_image.sizeDelta.y > 0f)
        {
            yield return null;

            currentTime += Time.deltaTime;
            currentSizeDelta.y = height * (1f - (currentTime / _durationAudio));
            _image.sizeDelta = currentSizeDelta;

            if (currentTime > 1f && !_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }

        currentTime = 0f;
        yield return wait;

        while (currentTime < _durationFade)
        {
            yield return null;

            currentTime += Time.deltaTime;
            _ui.alpha = 1f - (currentTime / _durationFade);
        }

        GWM.Instance.UIInventory.CloseUIInventory();
    }
}
