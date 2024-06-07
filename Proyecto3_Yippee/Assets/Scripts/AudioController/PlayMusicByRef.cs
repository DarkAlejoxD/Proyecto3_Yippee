using UnityEngine;
using FMODUnity;

namespace AudioController
{
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