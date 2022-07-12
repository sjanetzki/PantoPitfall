using System;
using UnityEngine;
using DualPantoFramework;

public class Tutorial : MonoBehaviour
{
    public AudioClip[] tutorialSteps;
    public bool shouldPlayTutorial;
    int currentTutorialStep = -1;
    private PantoHandle lowerHandle;
    int awaitedIndex = 0;
    float handleSpeed = 10f;
    Game game;
    string[][] awaitedAreaNames = {
        //new string[] {},
        new string[] {"Customers"}, //true
        new string[] {"Plates"}, //true
        new string[] {"StoveHasSoup"}, //true
        new string[] {"Customers"}, // false
        new string[] {"Tomatoes"}, //true
        new string[] {"ChoppingBoard"}, //true
        new string[] {"Stove"}, // false
        new string[] {"Plates", "StoveHasSoup", "Customers"}, // false
    };
    async void Start()
    {
        lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
        await lowerHandle.MoveToPosition(new Vector3(0, 0, -10), 10f);
        await GameObject.Find("Panto").GetComponent<UpperHandle>().MoveToPosition(new Vector3(0, 0, -10), handleSpeed);
        game = GetComponent<Game>();
        if (!shouldPlayTutorial)
        {
            game.enabled = true;
            return;
        }
        currentTutorialStep = 0;
        await game.playSound(tutorialSteps[currentTutorialStep]);
        await lowerHandle.SwitchTo(GameObject.Find(awaitedAreaNames[currentTutorialStep][awaitedIndex]), handleSpeed);
    }

    async void endTutorial()
    {
        await game.playSound(tutorialSteps[7]);
        GetComponent<Game>().enabled = true;
    }

    async void playNextTutorialStep(AudioClip successSound)
    {
        currentTutorialStep++;
        if (currentTutorialStep >= 8)
        {
            endTutorial();
            return;
        }
        awaitedIndex = 0;
        await game.playSound(tutorialSteps[currentTutorialStep]);
        if (Array.IndexOf(new int[] { 3, 6, 7 }, currentTutorialStep) <= -1)
        {
            await lowerHandle.SwitchTo(GameObject.Find(awaitedAreaNames[currentTutorialStep][awaitedIndex]), handleSpeed);
        }
        else
        {
            lowerHandle.Free();
        }
    }

    public async void EnteredArea(string areaName, AudioClip successSound)
    {
        if (currentTutorialStep < 0)
        {
            return;
        }
        if (areaName == awaitedAreaNames[currentTutorialStep][awaitedIndex])
        {
            if (successSound)
            {
                await game.playSound(successSound);
            }
            awaitedIndex++;
            if (awaitedIndex == awaitedAreaNames[currentTutorialStep].Length)
            {
                playNextTutorialStep(successSound);
            }
        }
    }
}
