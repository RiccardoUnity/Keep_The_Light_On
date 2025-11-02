using StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;
using GWM = GameWorldManager;

public class Campfire : Interactable
{
    public bool IsOn {  get; private set; }
    public float TotalMinutes { get; private set; }

    private PlayerInventory _playerInventory;
    private AudioSource _audioSource;
    private float _volumeMax;
    private float _volumeMaster;
    private float _volumeVFX;

    [SerializeField] private GameObject[] _wood;
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private Light _light;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _volumeMax = _audioSource.volume;

        if (SceneManager.GetActiveScene().buildIndex == S_GameManager.InfoScene.GameWorld)
        {
            GWM.Instance.AudioManager.onVomuneVFXChange += ChangeVolume;
            _playerInventory = GWM.Instance.PlayerManager.PlayerInventory;

            SetOff();
        }
        else if (SceneManager.GetActiveScene().buildIndex == S_GameManager.InfoScene.MainMenu)
        {
            MainMenuManager.Instance.UIOption.VolumeVFX.onValueChanged.AddListener(ChangeVolumeInMainMenu);
        }
    }

    public void OpenUI()
    {
        GWM.Instance.UIInventory.gameObject.SetActive(true);
        GWM.Instance.UIInventory.Craft();
        GWM.Instance.UIStats.gameObject.SetActive(false);
    }

    public void SetOn(int keyTrigger, int keyFuse, int keyFuel)
    {
        AddFuel(keyFuel);
        IsOn = true;
        _audioSource.Play();
        GWM.Instance.TimeManager.onNotPriority1 += UpdateNotPriority;
        GWM.Instance.TimeManager.onPause += Pause;
        GWM.Instance.TimeManager.onResume += Resume;

        _playerInventory.RemoveItemInventory(keyTrigger, true);
        _playerInventory.RemoveItemInventory(keyFuse, true);

        _light.gameObject.SetActive(true);
        _fire.gameObject.SetActive(true);
        foreach (GameObject gameObject in _wood)
        {
            gameObject.SetActive(true);
        }

        GWM.Instance.UIInventory.UICraft.ReCheckAll();
    }

    public void AddFuel(int keyFuel)
    {
        SO_Item soItem = _playerInventory.ViewInventoryItem(keyFuel);
        TotalMinutes += soItem.MinutesFuel;
        _playerInventory.RemoveItemInventory(keyFuel, true);
    }

    private void UpdateNotPriority(float timeDelay)
    {
        TotalMinutes -= timeDelay;
        if (TotalMinutes <= 0)
        {
            SetOff();
        }
    }

    private void Pause()
    {
        _fire.Pause();
        if (IsOn)
        {
            _audioSource.Stop();
        }
    }

    private void Resume()
    {
        _fire.Play();
        if (IsOn)
        {
            _audioSource.Play();
        }
    }

    private void SetOff()
    {
        IsOn = false;
        _audioSource.Stop();
        GWM.Instance.TimeManager.onNotPriority1 -= UpdateNotPriority;
        GWM.Instance.TimeManager.onPause -= Pause;
        GWM.Instance.TimeManager.onResume -= Resume;

        _light.gameObject.SetActive(false);
        _fire.gameObject.SetActive(false);
        foreach (GameObject gameObject in _wood)
        {
            gameObject.SetActive(false);
        }
    }

    private void ChangeVolume(float value) => _audioSource.volume = Mathf.Lerp(0f, _volumeMax, value);

    private void ChangeVolumeInMainMenu(float value)
    {
        _volumeMaster = MainMenuManager.Instance.UIOption.VolumeMaster.value;
        _volumeVFX = MainMenuManager.Instance.UIOption.VolumeVFX.value;
        value = Mathf.Lerp(0f, _volumeMaster, _volumeVFX);
        ChangeVolume(value);
    }
}
