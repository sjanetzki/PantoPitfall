using System.Collections.Generic;
using UnityEngine;

namespace DualPantoFramework
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Collider))]
    public class RoomElement : MonoBehaviour
    {
        AudioSource audioSource;
        public LayerMask layerMask;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }

        protected bool IsViewClear(Collider collider)
        {
            Vector3 handlePosition = collider.transform.position;
            Vector3 direction = transform.position - handlePosition;
            RaycastHit hit;
            if (Physics.Raycast(handlePosition, direction, out hit, direction.magnitude, layerMask)) return false;
            else return true;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "PerceptionCone" && IsViewClear(collider))
            {
                if (audioSource.isPlaying) return;
                if (audioSource.enabled) audioSource.Play();
            }
        }

        void OnTriggerStay(Collider collider)
        {
            if (collider.tag == "PerceptionCone" && IsViewClear(collider))
            {
                if (audioSource.isPlaying) return;
                if (audioSource.enabled) audioSource.Play();
            }
            else if (collider.tag == "PerceptionCone")
            {
                audioSource.Pause();
            }
        }

        void OnTriggerExit(Collider collider)
        {
            audioSource.Pause();
        }
    }
}
