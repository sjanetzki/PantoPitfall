using UnityEngine;
using DualPantoFramework;

public class ItHandle : MonoBehaviour
{
    PantoHandle lowerHandle;
    bool free = true;
    void Start()
    {
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
    }

    void FixedUpdate()
    {
        transform.position = lowerHandle.HandlePosition(transform.position);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (free)
            {
                lowerHandle.Freeze();
            }
            else
            {
                lowerHandle.Free();
            }
            free = !free;
        }
    }
}
