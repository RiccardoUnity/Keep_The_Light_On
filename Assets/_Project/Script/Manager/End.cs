using UnityEngine;
using GWM = GameWorldManager;

public class End : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerManager>())
        {
            GWM.Instance.UIEnd.End();
        }
    }
}
