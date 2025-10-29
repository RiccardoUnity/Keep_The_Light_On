using TMPro;
using UnityEngine;
using GWM = GameWorldManager;

public class UI_Statistics : MonoBehaviour
{
    private bool _isMyAwake;

    [SerializeField] private TMP_Text _walk;
    [SerializeField] private TMP_Text _run;

    private PlayerController _playerController;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            GWM.Instance.TimeManager.onNotPriority1 += UpdateWalk;
            GWM.Instance.TimeManager.onNotPriority1 += UpdateRun;

            _playerController = GWM.Instance.PlayerManager.PlayerController;
        }
    }

    private void UpdateWalk(float timeDelay)
    {
        _walk.text = _playerController.WalkTime.ToString();
    }

    private void UpdateRun(float timeDelay)
    {
        _run.text = _playerController.RunTime.ToString();
    }
}
