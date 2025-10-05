using UnityEngine;
using System;
using Save;

[Serializable]
public class Save_Transform
{
    private Transform _transform;
    [SerializeField] private Vector3Save _position;
    [SerializeField] private QuaternionSave _rotation;

    public Save_Transform(Transform transform)
    {
        _transform = transform;
        Save();
    }

    public void Save()
    {
        _position.Update(_transform.position);
        _rotation.Update(_transform.rotation);
    }
}