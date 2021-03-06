using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;
using SpeechIO;
using UnityEngine.SceneManagement;

public class PlayerControler : MonoBehaviour
{
    PantoHandle upperHandle;
    PantoHandle lowerHandle;
    GameObject panto;
    private SpeechOut speechOut;
    public bool has_sword = false;
    public bool has_key = true;
    public bool has_shield = false;
    public AudioClip doorOpen;
    public AudioClip doorKnock;
    public AudioClip chestOpen;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        panto = GameObject.Find("Panto");
        upperHandle = panto.GetComponent<UpperHandle>();
        lowerHandle = panto.GetComponent<LowerHandle>();
        panto.GetComponent<PantoControler>().StartWithItem();
        audioSource = panto.GetComponent<AudioSource>();
        speechOut = new SpeechOut();
    }

    // Update is called once per frame
    void Update()
    {
        if(!panto.GetComponent<PantoControler>().paused)
        {
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            transform.position = upperHandle.GetPosition();
        } else{
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            upperHandle.MoveToPosition(transform.position);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        //CROWN
        if(other.gameObject.CompareTag("Crown"))
        {
            Debug.Log("Collision with Crown)");
            panto.GetComponent<PantoControler>().WinLevel();
        }

        //DOOR
        else if(other.gameObject.CompareTag("Door"))
        {
            Debug.Log("Collision with Door)");
            if(has_key){
                audioSource.PlayOneShot(doorOpen, 0.5f);
                other.gameObject.GetComponent<PantoBoxCollider>().Disable();
                //has_key = false;
            }
            else{
                audioSource.PlayOneShot(doorKnock, 0.7f);
                speechOut.Speak("I need a key for this door.");
            }
        }

        //CHEST
        else if(other.gameObject.CompareTag("Chest"))
        {
            Debug.Log("Collision with Chest");
            if(!has_key && other.gameObject.GetComponent<ChestScript>().chestOpen == false){
                audioSource.PlayOneShot(chestOpen);
                speechOut.Speak("You found a key in this chest.");
                has_key = true;
                other.gameObject.GetComponent<ChestScript>().chestOpen = true;
            }
        }

        //LADDER
        else if(other.gameObject.CompareTag("Ladder"))
        {
            Debug.Log("Collision with Ladder");
            // if y position of player == 0 --> say Up to get up
                //start listening
                //wenn UP, dann kletterger??usch ca. 3 sekunden
                // y position von player auf 5 setzen
                // feedback, dass man oben ist
            // if y position of player > 4 (y sollte eigentlich 5 sein) --> say down to get down)
                // analog
        }
    }
}
