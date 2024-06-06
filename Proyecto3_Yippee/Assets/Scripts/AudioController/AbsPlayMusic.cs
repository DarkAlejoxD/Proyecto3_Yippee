using FMOD.Studio;

namespace AudioController
{
    public abstract class AbsPlayMusic : AbsPlayAudio
    {
        protected EventInstance _eventInstance;
        public abstract void StopSound();
    }
}