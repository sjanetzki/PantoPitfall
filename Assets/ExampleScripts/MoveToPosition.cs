using UnityEngine;
using DualPantoFramework;
using System.Threading.Tasks;

public class MoveToPosition : MonoBehaviour
{
    public bool isUpper;
    public bool shouldFreeHandle;
    public float speed = 10f;
    PantoHandle handle;
    async void Start()
    {
        await Task.Delay(500);
        handle = isUpper
            ? (PantoHandle)GameObject.Find("Panto").GetComponent<UpperHandle>()
            : (PantoHandle)GameObject.Find("Panto").GetComponent<LowerHandle>();

        await handle.MoveToPosition(gameObject.transform.position, speed, shouldFreeHandle);
    }

    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            await handle.MoveToPosition(gameObject.transform.position, speed, shouldFreeHandle);
        }
    }
}
