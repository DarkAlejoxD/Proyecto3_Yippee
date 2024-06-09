﻿using UnityEngine;
using FMODUnity;
using FMOD.Studio;

namespace AudioController
{
    public class PlayMusicByRef : AbsPlayMusic
    {
        [Header("References")]
        [SerializeField] private EventReference _event;

        public override void PlaySound()
        {
            var audioManager = AudioManager.GetAudioManager();
            audioManager.CrossFadeMusic(_event);
        }
        public override void StopSound()
        {
        }
    }
}