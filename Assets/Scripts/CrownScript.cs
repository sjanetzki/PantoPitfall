using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;
using SpeechIO;
using UnityEngine.SceneManagement;

public class CrownScript : MonoBehaviour
{
    PantoHandle upperHandle;
    PantoHandle lowerHandle;
    GameObject panto;
  
    // Start is called before the first frame update
    void Start()
    {
        panto = GameObject.Find("Panto");
        upperHandle = panto.GetComponent<UpperHandle>();
        lowerHandle = panto.GetComponent<LowerHandle>();        
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        MoveItHandle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    async void MoveItHandle()
    {
        await upperHandle.SwitchTo(gameObject, 20f);
        upperHandle.Freeze();
    }
}
