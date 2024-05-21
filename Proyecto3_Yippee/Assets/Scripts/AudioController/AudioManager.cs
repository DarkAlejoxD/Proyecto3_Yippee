using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using UtilsComplements;

namespace AudioController
{
    public class AudioManager : MonoBehaviour, ISingleton<AudioManager>
    {
        #region Fields
        [Header("References")]
        [SerializeField] private AudioReferences _enemiesData;
        private List<EventInstance> _audioInstances;

        public ISingleton<AudioManager> Instance => this;
        #endregion

        #region Unity Logic
        private void Awake()
        {
            Instance.Instantiate();
            _audioInstances = new();
        }

        private void OnDestroy()
        {
            Instance.RemoveInstance();
            CleanUp();
        }
        #endregion

        #region Static Methods

        public static AudioManager GetAudioManager()
        {
            if (Singleton.TryGetInstance(out AudioManager manager))
                return manager;

            GameObject gameObject = new("AudioManager");
            gameObject.AddComponent<AudioManager>();
            return GetAudioManager();
        }

        #endregion

        #region Public Methods
        public void PlayOneShot(EventReference sound, Vector3 worldPos)
        {
            RuntimeManager.PlayOneShot(sound, worldPos);
        }

        public void PlayOneShot(BankType bank, string name, Vector3 worldPos)
        {
            EventReference audioEvent = default;
            switch (bank)
            {
                case BankType.Ambience:
                    break;
                case BankType.Enemies:
                    audioEvent = _enemiesData.GetEvent(name);
                    break;
                case BankType.Music:
                    break;
                case BankType.NPC:
                    break;
                case BankType.Player:
                    break;
            }
            PlayOneShot(audioEvent, worldPos);
        }

        public EventInstance CreateEventInstance(EventReference eventRef)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventRef);
            _audioInstances.Add(eventInstance);
            return eventInstance;
        }

        public EventInstance CreateEventInstance(BankType bank, string name)
        {
            EventReference eventRef = default;

            switch (bank)
            {
                case BankType.Ambience:
                    break;
                case BankType.Enemies:
                    eventRef = _enemiesData.GetEvent(name);
                    break;
                case BankType.Music:
                    break;
                case BankType.NPC:
                    break;
                case BankType.Player:
                    break;
            }

            return CreateEventInstance(eventRef);
        }

        public void RemoveInstance(EventInstance instance)
        {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
            if (_audioInstances.Contains(instance))
                _audioInstances.Remove(instance);
        }
        #endregion

        #region Private Methods
        private void CleanUp()
        {
            foreach (var item in _audioInstances)
            {
                item.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                item.release();
            }
        }
        #endregion
    }
}