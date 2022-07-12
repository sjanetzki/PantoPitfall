using UnityEngine;

namespace DualPantoFramework
{
    public class PantoBehaviour : MonoBehaviour
    {
        protected DualPantoSync pantoSync;

        protected virtual void Awake()
        {
            pantoSync = GetPantoGameObject().GetComponent<DualPantoSync>();
        }

        protected GameObject GetPantoGameObject()
        {
            return GameObject.Find("Panto");
        }

        protected DualPantoSync GetPantoSync()
        {
            return pantoSync;
        }
    }
}
