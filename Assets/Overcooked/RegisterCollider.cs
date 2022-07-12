using System.Threading.Tasks;
using UnityEngine;
using DualPantoFramework;

public class RegisterCollider : MonoBehaviour
{
    async void Start()
    {
        await Task.Delay(2000);
        PantoCollider[] pantoColliders = GameObject.FindObjectsOfType<PantoCollider>();
        foreach (PantoCollider collider in pantoColliders)
        {
            collider.CreateObstacle();
            collider.Enable();
        }
    }
}
