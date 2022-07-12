using System.Threading.Tasks;
using SpeechIO;
using System.Collections.Generic;
using UnityEngine;
namespace DualPantoFramework
{


    public class IntroManager : MonoBehaviour
    {

        public IntroContourStrategy introContourStrategy;
        UpperHandle upper;
        LowerHandle lower;
        public SpeechOut speechOut;
        AudioSource scratchSound;
        // Start is called before the first frame update
        void Start()
        {
            speechOut = new SpeechOut();
            scratchSound = gameObject.AddComponent<AudioSource>();
            scratchSound.clip = Resources.Load<AudioClip>("Sounds/scratch");
            scratchSound.loop = true;
            scratchSound.volume = 1;
            lower = GameObject.Find("Panto").GetComponent<LowerHandle>();
            upper = GameObject.Find("Panto").GetComponent<UpperHandle>();
        }
        public async Task MoveToPosition(Vector3 position, bool onUpper)
        {
            if (onUpper) await upper.MoveToPosition(position);
            else await lower.MoveToPosition(position);
        }
        public async Task SwitchTo(GameObject go, bool onUpper, float speed = 3.0f)
        {
            if (onUpper) await upper.SwitchTo(go, speed);
            else await lower.SwitchTo(go, speed);
        }
        public async Task MoveAndSpeak(Vector3 position, string text, bool onUpper = false)
        {
            if (onUpper)
            {
                await Task.WhenAll(
                    upper.MoveToPosition(position, 5f, true),
                    speechOut.Speak(text)
                );
            }
            else
            {
                await Task.WhenAll(
                    lower.MoveToPosition(position, 5f, true),
                    speechOut.Speak(text)
                );
            }
        }

        public async Task Speak(string text)
        {
            await speechOut.Speak(text);
        }
        public async Task ShowPassage(Vector3 start1, Vector3 end1, Vector3 start2, Vector3 end2, string text, bool onUpper)
        {
            await MoveToPosition(start1, onUpper);
            speechOut.Speak(text);
            Vector3 vector1 = end1 - start1;
            Vector3 vector2 = end2 - start2;
            int segments = 8;
            for (var i = 1; i < segments + 1; i++)
            {
                Vector3 next = i % 2 == 1 ? start2 + ((float)i / (float)segments) * vector2 : start1 + ((float)i / (float)segments) * vector1;
                await MoveToPosition(next, onUpper);
            }
            while (speechOut.IsSpeaking())
            {
                await MoveToPosition(end1, onUpper);
                await MoveToPosition(end2, onUpper);
            }

        }
        // mark the exit by going back and forth using the handle
        public async Task ShowExit(Vector3 start, Vector3 end, string text, bool onUpper)
        {
            speechOut.Speak(text);
            scratchSound.Play();
            while (speechOut.IsSpeaking())
            {
                await MoveToPosition(end, onUpper);
                await MoveToPosition(start, onUpper);
            }
            scratchSound.Pause();
        }
        public async Task IntroduceContours(GameObject[] positions, string text, bool onUpper = false)
        {
            switch (introContourStrategy)
            {
                case IntroContourStrategy.WALL_SCRATCH_SOUND:
                    string[] t = text.Split(' ');
                    scratchSound.Play();
                    for (var i = 0; i < positions.Length; i++)
                    {
                        string textSnippet = i < t.Length ? t[i] : "";
                        await MoveAndSpeak(positions[i].transform.position, textSnippet, onUpper);
                    }
                    scratchSound.Pause();
                    break;

            }
        }

        public async Task IntroduceEnemies(string roomName, bool onUpper = false)
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Guard"))
            {
                if (enemy.name.StartsWith(roomName))
                    await MoveAndSpeak(enemy.gameObject.transform.position, enemy.name.Substring(roomName.Length + 1), onUpper);
            }
        }
    }
}