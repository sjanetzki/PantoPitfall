using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;

public class WallScrip : MonoBehaviour
{
    PantoBoxCollider collider;
    PantoCollider[] pantoColliders;
    // Start is called before the first frame update
    void Start()
    {
        /*pantoColliders = GameObject.FindObjectsOfType<PantoCollider>();
        foreach (PantoCollider collider in pantoColliders)
        {
            collider.CreateObstacle();
            collider.Enable();
        }*/

        collider = gameObject.GetComponent<PantoBoxCollider>();
        collider.CreateObstacle();
        collider.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
