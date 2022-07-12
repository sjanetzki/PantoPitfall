using UnityEngine;

public class Area : MonoBehaviour
{
    public AudioClip ambientSound;
    public AudioClip successSound;
    private AudioSource audioSource;
    private Game game;
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        audioSource.playOnAwake = false;
        audioSource.clip = ambientSound;
        audioSource.volume = 0.5f;
        game = GameObject.Find("Game").GetComponent<Game>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            audioSource.Play();
            game.EnteredArea(gameObject.name, successSound);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            audioSource.Stop();
        }
    }
}
