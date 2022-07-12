using System;
using UnityEngine;

namespace DualPantoFramework
{
    public abstract class PantoCollider : PantoBehaviour
    {
        protected ushort id;
        protected bool pantoEnabled = false;
        public bool onUpper = true;
        public bool onLower = true;
        public bool isPassable = false;
        private bool registered = false;
        protected int containingSpheres = 0;

        public ushort GetId()
        {
            return id;
        }
        public void IncreaseSpheres()
        {
            containingSpheres++;
        }
        public void DecreaseSpheres()
        {
            containingSpheres--;
        }
        public int GetContainingSpheres()
        {
            return containingSpheres;
        }

        // Whether the collider is currently enabled, meaning should provide collision 
        public bool IsEnabled()
        {
            return pantoEnabled;
        }
        protected byte getPantoIndex()
        {
            if (onUpper && onLower) return 0xff;
            if (onUpper) return 0;
            if (onLower) return 1;
            return 2;
        }

        protected Vector2[] CornersFromRotatedRectangle(Vector3 center, float angle, Vector2 dimensions)
        {
            angle *= -Mathf.Deg2Rad;

            Vector2 v1 = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector2 v2 = new Vector2(-v1.y, v1.x);
            v1 *= (dimensions.x / 2);
            v2 *= (dimensions.y / 2);

            Vector2 center2 = new Vector2(center.x, center.z);
            return new Vector2[] {
            center2 + v1 + v2,
            center2 - v1 + v2,
            center2 - v1 - v2,
            center2 + v1 - v2,
        };
        }

        protected Vector2[] RailPointsFromRotatedRectangle(Vector3 center, float angle, Vector2 dimensions)
        {
            angle *= -Mathf.Deg2Rad;

            Vector2 v1 = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector2 v2 = new Vector2(-v1.y, v1.x);
            v1 *= (dimensions.x / 2);
            v2 *= (dimensions.y / 2);

            Vector2 center2 = new Vector2(center.x, center.z);
            // the rail is in the middle of the longer side of the rectangle 
            if (dimensions.x > dimensions.y)
            {
                return new Vector2[] {
                    center2 + v1,
                    center2 - v1
                };
            }
            else
            {
                return new Vector2[] {
                    center2 + v2,
                    center2 - v2
                };
            }
        }


        protected Vector2[] CornersFromBounds(Bounds bounds)
        {
            Vector3 center = bounds.center;
            Vector3 size = bounds.size;
            Vector2 topRight = new Vector2(center.x + size.x / 2, center.z + size.z / 2);
            Vector2 bottomRight = new Vector2(center.x + size.x / 2, center.z - size.z / 2);
            Vector2 bottomLeft = new Vector2(center.x - size.x / 2, center.z - size.z / 2);
            Vector2 topLeft = new Vector2(center.x - size.x / 2, center.z + size.z / 2);
            return new Vector2[] { topRight, bottomRight, bottomLeft, topLeft };
        }

        /// <summary>
        /// Registers the obstacle on the Panto, the shape depends on its type. Don't forget to call Enable()
        /// </summary>
        public abstract void CreateObstacle();

        protected void UpdateId()
        {
            id = pantoSync.GetNextObstacleId();
            if (!registered)
            {
                registered = true;
                ColliderRegistry.AddCollider(this);
            }
        }

        protected void CreateLineObstacle(Vector2 start, Vector2 end)
        {
            byte index = getPantoIndex();
            if (index == 2)
            {
                Debug.LogWarning("[DualPanto] Skipping creation for object with no handles");
            }
            pantoSync.CreateObstacle(index, id, start, end);
            DrawLine(start, end);
        }
        protected void CreateBoxObstacle()
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            Vector2 size = new Vector2(collider.size.x * transform.localScale.x, collider.size.z * transform.localScale.z);
            CreateFromCorners(CornersFromRotatedRectangle(transform.position, transform.eulerAngles.y, size));
        }

        protected void CreateCircularCollider(int numberOfCorners)
        {
            Vector3 center = GetComponent<SphereCollider>().center + transform.position;
            Vector3 radius = GetComponent<SphereCollider>().radius * transform.lossyScale;

            Vector2[] corners = new Vector2[numberOfCorners];
            for (var i = 0; i < numberOfCorners; i++)
            {
                float angle = i * Mathf.PI * 2 / numberOfCorners;
                float x = Mathf.Cos(angle) * radius.x;
                float z = Mathf.Sin(angle) * radius.z;
                corners[i] = new Vector2(x + transform.position.x, z + transform.position.z);
            }
            CreateFromCorners(corners);
        }
        protected void CreatePolygonObstacle()
        {
            Vector2[] points = GetComponent<PolygonCollider2D>().points;
            Vector2[] newPoints = new Vector2[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                Vector3 newPoint = transform.TransformPoint(points[i].x, points[i].y, 0);
                newPoints[i] = new Vector2(newPoint.x, newPoint.z);
            }
            CreateFromCorners(newPoints);
        }

        public void CreateFromCorners(Vector2[] corners)
        {
            byte index = getPantoIndex();
            if (index == 2)
            {
                Debug.LogWarning("[DualPanto] Skipping creation for object with no handles");
            }
            if (this.isPassable)
            {
                pantoSync.CreatePassableObstacle(index, id, corners[0], corners[1]);
                DrawLine(corners[0], corners[1]);
            }
            else
            {
                pantoSync.CreateObstacle(index, id, corners[0], corners[1]);
                DrawLine(corners[0], corners[1]);
            }
            for (int i = 1; i < corners.Length - 1; i++)
            {
                pantoSync.AddToObstacle(index, id, corners[i], corners[i + 1]);
                DrawLine(corners[i], corners[i + 1]);
            }
            pantoSync.AddToObstacle(index, id, corners[corners.Length - 1], corners[0]);
            DrawLine(corners[corners.Length - 1], corners[0]);
        }

        void DrawLine(Vector2 start, Vector2 end)
        {
            GameObject n = new GameObject();
            n.transform.parent = transform;
            n.layer = LayerMask.NameToLayer("Walls2");
            LineRenderer lr = n.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(start.x, 5, start.y));
            lr.SetPosition(1, new Vector3(end.x, 5, end.y));
            lr.startWidth = 0.02f * GetPantoSync().gameObject.transform.localScale.magnitude;
            lr.material = Resources.Load("Materials/Colliders") as Material;
        }

        public void CreateRailForLine(Vector2 start, Vector2 end, float displacement)
        {
            byte index = getPantoIndex();
            if (index == 2)
            {
                Debug.LogWarning("[DualPanto] Skipping creation for object with no handles");
            }
            pantoSync.CreateRail(index, id, start, end, displacement);
        }

        public void CreateRail()
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            Vector2 size = new Vector2(collider.size.x * transform.lossyScale.x, collider.size.z * transform.lossyScale.z);
            float displacement = Math.Min(size.x, size.y) / 2;
            Vector2[] points = RailPointsFromRotatedRectangle(transform.position, transform.eulerAngles.y, size);
            byte index = getPantoIndex();
            if (index == 2)
            {
                Debug.LogWarning("[DualPanto] Skipping creation for object with no handles");
            }
            pantoSync.CreateRail(index, id, points[0], points[1], displacement);
            DrawLine(points[0], points[1]);

        }

        /// <summar>
        /// Disables the obstacle
        /// </summary>
        public void Disable()
        {
            pantoEnabled = false;
            GetPantoSync().DisableObstacle(getPantoIndex(), id);
        }

        /// <summar>
        /// Removes the obstacle.
        /// </summary>
        public void Remove()
        {
            GetPantoSync().RemoveObstacle(getPantoIndex(), id);
            foreach (Transform child in this.transform)
            {
                Debug.Log(child);
                if (child.GetComponent<LineRenderer>())
                {
                    Destroy(child.gameObject);
                }
            }
        }

        /// <summar>
        /// Enables the obstacle. This needs to be called after creating the obstacle.
        /// </summary>
        public void Enable()
        {
            pantoEnabled = true;
            GetPantoSync().EnableObstacle(getPantoIndex(), id);
        }
    }
}