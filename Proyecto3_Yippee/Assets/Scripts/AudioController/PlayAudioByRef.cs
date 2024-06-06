using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace AudioController
{
    public class PlayAudioByRef : AbsPlayAudio
    {
        [Header("References")]
        [SerializeField] private EventReference _event;

        public override void PlaySound()
        {
            AudioManager.GetAudioManager().PlayOneShot(_event, transform.position);
        }
    }

    public class PlayMusicByRef : AbsPlayMusic
    {
        [Header("References")]
        [SerializeField] private EventReference _event;
        

        public override void PlaySound()
        {
            if (_eventInstance.Equals(null) || _eventInstance.Equals(default))
                _eventInstance = AudioManager.GetAudioManager().CreateEventInstance(_event);

            _eventInstance.start();
        }
        public override void StopSound()
        {
            if (!_eventInstance.Equals(null) && !_eventInstance.Equals(default))
                _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}