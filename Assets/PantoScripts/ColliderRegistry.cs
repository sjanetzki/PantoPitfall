using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DualPantoFramework
{
    public class ColliderRegistry
    {
        static List<PantoCollider> ColliderList = new List<PantoCollider>();

        public async static void RegisterObstacles()
        {
            foreach (PantoCollider collider in ColliderList)
            {
                await Task.Delay(10);
                collider.CreateObstacle();
                if (collider.IsEnabled()) collider.Enable();
            }
        }

        public static void AddCollider(PantoCollider collider)
        {
            ColliderList.Add(collider);
        }

        public static void RemoveCollider(PantoCollider collider)
        {
            ColliderList.RemoveAll((c) => c.GetId() == collider.GetId());
        }
    }
}