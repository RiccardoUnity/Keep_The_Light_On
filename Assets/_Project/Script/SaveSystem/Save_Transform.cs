using UnityEngine;
using System;
using Save;

public static partial class S_SaveSystem
{
    [Serializable]
    private abstract class Save_Transform
    {
        private Transform _transform;
        [SerializeField] private Vector3Save _position;
        [SerializeField] private QuaternionSave _rotation;

        public Save_Transform(Transform transform)
        {
            _transform = transform;
        }

        public virtual void Save()
        {
            _position.Update(_transform.position);
            _rotation.Update(_transform.rotation);
        }

        public virtual void Load()
        {
            _transform.position = _position.Load();
            _transform.rotation = _rotation.Load();
        }
    }
}