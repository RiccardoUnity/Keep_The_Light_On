using UnityEngine;
using System;

namespace Save
{
    [Serializable]
    public struct Vector3Save
    {
        public float[] axis;

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
    }

    [Serializable]
    public struct QuaternionSave
    {
        public float[] axis;

        public QuaternionSave(Quaternion quaternion)
        {
            axis = new float[4];
            Update(quaternion);
        }

        public void Update(Quaternion quaternion)
        {
            axis[0] = quaternion.w;
            axis[1] = quaternion.x;
            axis[2] = quaternion.y;
            axis[3] = quaternion.z;
        }
    }
}