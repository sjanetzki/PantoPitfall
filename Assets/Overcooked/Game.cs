using System.Threading.Tasks;
using UnityEngine;

public class Game : MonoBehaviour
{
    int failedDeliveries = 0;
    int currentStep = 0;
    private bool customerSatisfied = false;
    string[] steps = {
        "Tomatoes",
        "ChoppingBoard",
        "Stove",
        "Plates",
        "StoveHasSoup",
        "Customers"
    };
    AudioSource audioSource;
    AudioSource customerAudioSource;
    public AudioClip levelStart;
    public AudioClip customerOrdering;
    public AudioClip customerHungry;
    public AudioClip customerAngry;
    public AudioClip customerLeaving;
    public AudioClip gameLost;

    void Awake()
    {
        audioSource = AddAudio();
        customerAudioSource = AddAudio();
    }

    async void OnEnable()
    {
        await playSound(audioSource, levelStart);
        SpawnCustomers();
    }

    async void SpawnCustomers()
    {
        while (true)
        {
            await SpawnCustomer();
        }
    }

    AudioSource AddAudio()
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.loop = false;
        newAudio.playOnAwake = false;
        return newAudio;
    }

    async Task failDelivery()
    {
        failedDeliveries++;
        if (failedDeliveries >= 3)
        {
            await playSound(audioSource, gameLost);
        }
    }

    async Task SpawnCustomer()
    {
        customerSatisfied = false;
        await playSound(customerAudioSource, customerOrdering);
        await Task.Delay(3000);
        if (customerSatisfied) return;
        await playSound(customerAudioSource, customerHungry);
        await Task.Delay(3000);
        if (customerSatisfied) return;
        await playSound(customerAudioSource, customerAngry);
        await Task.Delay(3000);
        if (customerSatisfied) return;
        await playSound(customerAudioSource, customerLeaving);
        await failDelivery();
    }

    void OnApplicationQuit()
    {
        customerSatisfied = true;
    }

    public async Task playSound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
        await Task.Delay(Mathf.RoundToInt(clip.length * 1000));
    }
    public async Task playSound(AudioClip clip)
    {
        await playSound(audioSource, clip);
    }

    public async void EnteredArea(string areaName, AudioClip successSound)
    {
        if (!enabled)
        {
            GetComponent<Tutorial>().EnteredArea(areaName, successSound);
            return;
        }
        if (areaName == steps[currentStep])
        {
            await playSound(successSound);
            currentStep++;
            if (currentStep >= steps.Length)
            {
                customerSatisfied = true;
                customerAudioSource.Stop();
                currentStep = 0;
            }
        }
    }
}
