using UnityEngine;
using System.Threading.Tasks;
namespace DualPantoFramework
{
    public class ColliderPolyline : PantoBehaviour
    {
        public Vector2[] points;
        protected Color gizmoColor = Color.green;
        async public void CreateObstacles()
        {
            for (int i = 0; i < points.Length - 1; i++)
            {
                CreateObstacle(i);
                await Task.Delay(20);
            }
        }

        protected virtual void CreateObstacle(int i)
        {
            PantoLineCollider lc = this.gameObject.AddComponent<PantoLineCollider>();
            lc.start = points[i];
            lc.end = points[i + 1];
            // lc.CreateObstacle();
            // lc.Enable();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.DrawLine(new Vector3(points[i].x, 5, points[i].y), new Vector3(points[i + 1].x, 5, points[i + 1].y));
            }

        }
    }
}