using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;
using SpeechIO;
using UnityEngine.SceneManagement;
using System;

public class PantoControler : MonoBehaviour
{
    PantoHandle upperHandle;
    PantoHandle lowerHandle;
    private SpeechOut speechOut;
    private SpeechIn speechIn;
    public bool paused; //tick in Unity --> true at start
    public string sceneName;
    // Start is called before the first frame update
    async void Start()
    {
        upperHandle = GetComponent<UpperHandle>();
        lowerHandle = GetComponent<LowerHandle>();

        speechOut = new SpeechOut();
        speechIn = new SpeechIn(onSpechRecognized);
        Debug.Log("pre introduction");
        await GetComponent<Level>().PlayIntroduction();
        speechOut.Speak("Say help if you need help.");
        speechIn.StartListening(new string[]{"help"});
        Debug.Log("introduction done");

        SetUpItHandle();
        Debug.Log("setting paused = false");
        paused = false;

        // Create a temporary reference to the current scene. // https://answers.unity.com/questions/1173303/how-to-check-which-scene-is-loaded-and-write-if-co.html
        Scene currentScene = SceneManager.GetActiveScene ();
         // Retrieve the name of this scene.
        sceneName = currentScene.name;
        //StartWithItem();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onSpechRecognized(string command)
    {
        if((command == "help" || command == "stop" || command =="pause" || command == "break") && !paused)
        {
            paused = true;
            Debug.Log("help");
            speechIn.StartListening(new string[]{"resume", "start", "continue"});
            speechOut.Speak("Say start if you want to resume the game.");
        }
        else if((command == "resume" || command == "start" || command == "continue") && paused)
        {
            Debug.Log("resume");
            speechOut.Speak("Game continues.");
            speechIn.StartListening(new string[]{"help", "stop", "pause", "break"});
            paused = false;   
        }
    }

    void OnApplicationQuit()
    {
        speechOut.Stop();
    }

    GameObject FindCrown()
    {
        return GameObject.Find("Crown");
    }

    async void SetUpItHandle()
    {
        GameObject crown = FindCrown();
        await lowerHandle.SwitchTo(crown);
    }

    public void WinLevel()
    {
        //switch level here
        switch (sceneName){
            case ("Level01"):
                	speechOut.Speak("You won the first level.");
                    break;
            case ("Level02"):
                	speechOut.Speak("You won the second level.");
                    break;
            case ("Level03"):
                	speechOut.Speak("You won the third level.");
                    break;
            default:
                	speechOut.Speak("You won"+sceneName);
                    break;
        }
        //speechOut.Speak("You won the first level.");
        
    }

    public void OpenDoor(GameObject door)
    {
        //gameObject.active = false;
        door.SetActive(false); // door should disappear
    }
    public void StartWithItem()
    {
        //level 03 starts with key
        //if (String.Equals(sceneName, "Level03")){
        switch (sceneName){
            case ("Level01"): // fall through not possible with strings because doesn't know the order
            {
                GetComponent<PlayerControler>().has_sword = false;
                GetComponent<PlayerControler>().has_key = false;
                speechOut = new SpeechOut();
                Debug.Log("no sword");
                break;
            }
            case ("Level02"):
            {
                GetComponent<PlayerControler>().has_sword = false;
                GetComponent<PlayerControler>().has_key = false;
                speechOut = new SpeechOut();
                Debug.Log("no sword");
                break;
            }
            case ("Level03"):
            {
                GetComponent<PlayerControler>().has_sword = false;
                GetComponent<PlayerControler>().has_key = false;
                speechOut = new SpeechOut();
                Debug.Log("changed sword to key");
                speechOut.Speak("You have a key");
                break;
            }
            default:
                break;
            }
    }
    
}
