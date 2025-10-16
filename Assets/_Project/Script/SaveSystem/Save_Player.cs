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

        public bool Save()
        {
            if (_playerManager != null)
            {
                _position.Update(_playerManager.transform.position);
                _rotation.Update(_playerManager.transform.rotation);
                return true;
            }
            return false;
        }

        public bool Load()
        {
            if (_playerManager != null)
            {
                _playerManager.transform.position = _position.Load();
                _playerManager.transform.rotation = _rotation.Load();
                return true;
            }
            return false;
        }
    }
}