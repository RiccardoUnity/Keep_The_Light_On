using UnityEngine;
using GWM = GameWorldManager;

public class Campfire : Interactable
{
    public bool IsOn {  get; private set; }
    public float TotalMinutes { get; private set; }

    private PlayerInventory _playerInventory;

    [SerializeField] private GameObject[] _wood;
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private Light _light;

    void Start()
    {
        _playerInventory = GWM.Instance.PlayerManager.PlayerInventory;

        SetOff();
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
    }

    private void Resume()
    {
        _fire.Play();
    }

    private void SetOff()
    {
        IsOn = false;
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
}
