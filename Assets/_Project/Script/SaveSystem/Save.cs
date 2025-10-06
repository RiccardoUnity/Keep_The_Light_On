using UnityEngine;
using System;

namespace Save
{
    [Serializable]
    public struct Vector3Save
    {
        [SerializeField] private float[] axis;

        public Vector3Save(Vector3 vector)
        {
            axis = new float[3];
            Update(vector);
        }

        public void Update(Vector3 vector)
        {
            axis[0] = vector.x;
            axis[1] = vector.y;
            axis[2] = vector.z;
        }

        public Vector3 Load() => new Vector3(axis[0], axis[1], axis[2]);
    }

    [Serializable]
    public struct QuaternionSave
    {
        [SerializeField] private float[] axis;

        public QuaternionSave(Quaternion quaternion)
        {
            axis = new float[4];
            Update(quaternion);
        }

        public void Update(Quaternion quaternion)
        {
            axis[0] = quaternion.x;
            axis[1] = quaternion.y;
            axis[2] = quaternion.z;
            axis[3] = quaternion.w;
        }

        public Quaternion Load() => new Quaternion(axis[0], axis[1], axis[2], axis[3]);
    }
}