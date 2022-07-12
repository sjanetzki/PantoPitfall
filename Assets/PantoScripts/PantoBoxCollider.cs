namespace DualPantoFramework
{
    public class PantoBoxCollider : PantoCollider
    {
        public override void CreateObstacle()
        {
            UpdateId();
            CreateBoxObstacle();
        }
    }
}