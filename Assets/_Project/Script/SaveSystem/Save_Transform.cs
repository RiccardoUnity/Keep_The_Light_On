using UnityEngine;
using System;
using Save;

[Serializable]
public class Save_Transform
{
    private Transform _transform;
    [SerializeField] private Vector3Save _position;
    [SerializeField] private QuaternionSave _rotation;

    public virtual void Save()
    {
        _position.Update(_transform.position);
        _rotation.Update(_transform.rotation);
    }

    public virtual void Load(Transform transform)
    {
        _transform = transform;
        _transform.position = _position.Load();
        _transform.rotation = _rotation.Load();
    }
}