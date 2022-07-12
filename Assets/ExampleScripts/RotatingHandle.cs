using UnityEngine;
using DualPantoFramework;

public class RotatingHandle : MonoBehaviour
{
    PantoHandle handle;
    public bool isUpper = true;
    public float angle = 0;
    public float angleStep = 0.01f;
    void Start()
    {
        handle = isUpper ? (PantoHandle) GameObject.Find("Panto").GetComponent<UpperHandle>() : (PantoHandle) GameObject.Find("Panto").GetComponent<LowerHandle>();
    }

    void FixedUpdate()
    {
        // transform.eulerAngles = new Vector3(0, handle.GetRotation(), 0);
        // transform.RotateAround(transform.position, Vector3.up, 0.5f);
        handle.Rotate(angle);
        angle += angleStep;
    }
}
