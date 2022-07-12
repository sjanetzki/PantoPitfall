namespace DualPantoFramework
{
    public class PantoCircularCollider : PantoCollider
    {
        public int numberOfCorners = 8;
        public override void CreateObstacle()
        {
            UpdateId();
            CreateCircularCollider(numberOfCorners);
        }
    }
}