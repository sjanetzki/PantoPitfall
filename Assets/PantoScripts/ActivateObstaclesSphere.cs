using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DualPantoFramework
{
    public class ActivateObstaclesSphere : MonoBehaviour
    {
        void OnTriggerEnter(Collider collider)
        {
            PantoCollider pc = collider.GetComponent<PantoCollider>();
            if (pc != null && pc.enabled)
            {
                if (pc.GetContainingSpheres() == 0)
                {
                    pc.CreateObstacle();
                    //if (pc.IsEnabled()) pc.Enable();
                    pc.Enable();
                }
                pc.IncreaseSpheres();
            }
        }

        void OnTriggerExit(Collider collider)
        {
            PantoCollider pc = collider.GetComponent<PantoCollider>();
            if (pc != null)
            {
                pc.DecreaseSpheres();
                if (pc.GetContainingSpheres() == 0)
                {
                    pc.Remove();
                }
            }
        }
    }
}