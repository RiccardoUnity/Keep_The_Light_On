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

        public Save_Player(PlayerManager playerManager)
        {
            _playerManager = playerManager;

        }

        public void Save()
        {
            _position.Update(_playerManager.transform.position);
            _rotation.Update(_playerManager.transform.rotation);
        }

        public void Load()
        {
            _playerManager.transform.position = _position.Load();
            _playerManager.transform.rotation = _rotation.Load();
        }
    }
}