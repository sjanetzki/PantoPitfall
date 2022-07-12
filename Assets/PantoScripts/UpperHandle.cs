namespace DualPantoFramework
{
    /// <summary>
    /// The upper handle of the Panto, usually the Me Handle.
    /// </summary>
    public class UpperHandle : PantoHandle
    {
        new void Awake()
        {
            base.Awake();
            isUpper = true;
            pantoSync.RegisterUpperHandle(this);
        }
    }
}