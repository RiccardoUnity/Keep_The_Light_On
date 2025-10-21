using Save;
using System;
using UnityEngine;

public static partial class S_SaveSystem
{
    [Serializable]
    private class Save_Player
    {
        private PlayerManager _playerManager;
        [SerializeField] private Vector3Save _position;
        [SerializeField] private QuaternionSave _rotation;
        [SerializeField] private float[] _statsValue = new float[9];

        //Call it only in game
        public bool Save()
        {
            SetPlayerManager();
            try
            {
                _position.Update(_playerManager.transform.position);
                _rotation.Update(_playerManager.transform.rotation);

                _statsValue[0] = _playerManager.Life.Value;
                _statsValue[1] = _playerManager.Endurance.Value;
                _statsValue[2] = _playerManager.Rest.Value;
                _statsValue[3] = _playerManager.Hunger.Value;
                _statsValue[4] = _playerManager.Thirst.Value;
                _statsValue[5] = _playerManager.Stamina.Value;
                _statsValue[6] = _playerManager.SunStroke.Value;
                _statsValue[7] = _playerManager.StomachAche.Value;
                _statsValue[8] = _playerManager.HeartAche.Value;
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Call it only in game
        public bool Load()
        {
            SetPlayerManager();
            try
            {
                _playerManager.transform.position = _position.Load();
                _playerManager.transform.rotation = _rotation.Load();

                _playerManager.startValueLife = _statsValue[0];
                _playerManager.startValueEndurance = _statsValue[1];
                _playerManager.startValueRest = _statsValue[2];
                _playerManager.startValueHunger = _statsValue[3];
                _playerManager.startValueThirst = _statsValue[4];
                _playerManager.startValueStamina = _statsValue[5];
                _playerManager.startValueSunStroke = _statsValue[6];
                _playerManager.startValueStomachAche = _statsValue[7];
                _playerManager.startValueHeartAche = _statsValue[8];
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SetPlayerManager()
        {
            if (_playerManager == null)
            {
                _playerManager = GameWorldManager.Instance.PlayerManager;
            }
        }
    }
}