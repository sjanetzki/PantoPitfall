using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Vis : MonoBehaviour
{
    public LayerMask blindView;
    public LayerMask enhancedBlindView;
    public LayerMask developmentView;

    LayerMask[] views;
    int current = 0;

    void Awake()
    {
        views = new LayerMask[] { blindView, enhancedBlindView, developmentView };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            current++;
            if (current > 2) current = 0;
            Camera.main.cullingMask = views[current];
        }
    }
}