using UnityEngine;
using ClipperLib;

using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;
using Paths = System.Collections.Generic.List<System.Collections.Generic.List<ClipperLib.IntPoint>>;

namespace DualPantoFramework
{
    public class PantoCompoundCollider : PantoCollider
    {
        public override void CreateObstacle()
        {
            UpdateId();
            CreateCompoundObstacle();
        }

        private IntPoint IntPointFromVector2(Vector2 vector)
        {
            return new IntPoint(Mathf.RoundToInt(vector.x * 1000), Mathf.RoundToInt(vector.y * 1000));
        }

        private Path PathFromBounds(Bounds bounds)
        {
            Vector3 center = bounds.center;
            Vector3 size = bounds.size;
            Vector2 topRight = new Vector2(center.x + size.x / 2, center.z + size.z / 2);
            Vector2 bottomRight = new Vector2(center.x + size.x / 2, center.z - size.z / 2);
            Vector2 bottomLeft = new Vector2(center.x - size.x / 2, center.z - size.z / 2);
            Vector2 topLeft = new Vector2(center.x - size.x / 2, center.z + size.z / 2);
            Path value = new Path(4);
            value.Add(IntPointFromVector2(topRight));
            value.Add(IntPointFromVector2(bottomRight));
            value.Add(IntPointFromVector2(bottomLeft));
            value.Add(IntPointFromVector2(topLeft));
            return value;
        }

        private Vector2[] Vector2ArrayFromPath(Path path)
        {
            Vector2[] value = new Vector2[path.Count];
            for (int i = 0; i < path.Count; i++)
            {
                value[i] = (new Vector2(path[i].X / 1000f, path[i].Y / 1000f));
            }
            return value;
        }

        public void CreateCompoundObstacle()
        {
            Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
            Collider coll = colliders[0];

            Paths solution = new Paths();
            solution.Add(PathFromBounds(colliders[0].bounds));

            for (int i = 1; i < colliders.Length; i++)
            {
                Paths newPath = new Paths(1);
                newPath.Add(PathFromBounds(colliders[i].bounds));

                Clipper c = new Clipper();
                c.AddPaths(solution, PolyType.ptSubject, true);
                c.AddPaths(newPath, PolyType.ptClip, true);
                c.Execute(ClipType.ctUnion, solution);
            }
            CreateFromCorners(Vector2ArrayFromPath(solution[0]));
        }
    }
}