using UnityEngine;
using FMODUnity;

namespace AudioController
{
    public class PlayAudioByRef : PlayAudio
    {
        [Header("References")]
        [SerializeField] private EventReference _event;

        public override void PlaySound()
        {
            AudioManager.GetAudioManager().PlayOneShot(_event, transform.position);
        }
    }
}