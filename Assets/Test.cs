using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public AudioClip win;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();    
        PlayWin();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float PlayWin()
    {
        audioSource.PlayOneShot(win);
        return win.length;   
    }
}
