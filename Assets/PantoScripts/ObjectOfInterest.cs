using UnityEngine;
namespace DualPantoFramework
{
    public class ObjectOfInterest : MonoBehaviour
    {
        [Tooltip("If the game object has children, outline it by moving from one child to the next in a closed shape")]
        public bool traceShape = false;
        [Tooltip("Which handle the object should be introduced on")]
        public bool isOnUpper = false;
        //public AudioClip introductionSound;
        [Tooltip("Description will be read aloud on introduction")]
        [TextArea(3, 10)]
        public string description;
        [Tooltip("Objects of interest are sorted by their priority. Highest priority will be introduced first")]
        public int priority = 0;
    }
}
