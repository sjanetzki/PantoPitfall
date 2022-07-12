using UnityEngine;
using SpeechIO;
using DualPantoFramework;

public class IntroductionManager : MonoBehaviour
{
    SpeechOut speech;
    async void Start()
    {
        speech = new SpeechOut();
        Level level = GameObject.Find("Panto").GetComponent<Level>();
        await level.PlayIntroduction();
        await speech.Speak("Introduction finished, handles are free");
    }

    void OnApplicationQuit()
    {
        speech.Stop();
    }

}
