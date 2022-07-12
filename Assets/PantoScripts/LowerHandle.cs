using UnityEngine;

namespace DualPantoFramework
{
    /// <summary>
    /// The lower handle of the Panto, usually the It Handle.
    /// </summary>
    public class LowerHandle : PantoHandle
    {
        new void Awake()
        {
            base.Awake();
            isUpper = false;
            pantoSync.RegisterLowerHandle(this);
        }
    }
}
