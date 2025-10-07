using UnityEngine;
using System;

namespace Save
{
    [Serializable]
    public struct Vector3Save
    {
        [SerializeField] private float[] _axis;

        public Vector3Save(Vector3 vector)
        {
            _axis = new float[3];
            Update(vector);
        }

        public void Update(Vector3 vector)
        {
            _axis[0] = vector.x;
            _axis[1] = vector.y;
            _axis[2] = vector.z;
        }

        public Vector3 Load() => new Vector3(_axis[0], _axis[1], _axis[2]);
    }

    [Serializable]
    public struct QuaternionSave
    {
        [SerializeField] private float[] _axis;

        public QuaternionSave(Quaternion quaternion)
        {
            _axis = new float[4];
            Update(quaternion);
        }

        public void Update(Quaternion quaternion)
        {
            _axis[0] = quaternion.x;
            _axis[1] = quaternion.y;
            _axis[2] = quaternion.z;
            _axis[3] = quaternion.w;
        }

        public Quaternion Load() => new Quaternion(_axis[0], _axis[1], _axis[2], _axis[3]);
    }
}