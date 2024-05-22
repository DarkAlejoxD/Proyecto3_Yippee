using UnityEngine;

namespace AudioController
{
    public class PlayAudioByName : PlayAudio
    {
        [Header("References")]
        [SerializeField] private BankType _bank;
        [SerializeField] private string _name;

        public override void PlaySound()
        {
            AudioManager.GetAudioManager().PlayOneShot(_bank, _name.ToUpper(), transform.position);
        }
    }
}