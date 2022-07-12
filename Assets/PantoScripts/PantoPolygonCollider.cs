using UnityEngine;

namespace DualPantoFramework
{
    public class PantoPolygonCollider : PantoCollider
    {
        public override void CreateObstacle()
        {
            UpdateId();
            CreatePolygonObstacle();
        }
    }
}