using UnityEngine;
using AudioController;
using UtilsComplements;

namespace AvatarController.Animations
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        [Header("Player References")]
        [SerializeField] private PlayerController _player;

        [Header("Step References")]
        [SerializeField] private ParticleSystem _stepsSmoke;
        [SerializeField, Range(0, 1)] private float _stepProbability = 0.7f;
        [SerializeField, Range(0, 1)] private float _threshold = 0.1f;

        [Header("Jump References")]
        [SerializeField] private ParticleSystem _jumpSmoke;

        #region Public Methods
        public void Step()
        {
            float current = _player.Velocity.magnitude;
            float minSpeed = _player.DataContainer.DefaultMovement.MinSpeedToMove;
            float maxSpeed = _player.DataContainer.DefaultMovement.MaxSpeed;
            float minSpeedPct = Mathf.Lerp(minSpeed, maxSpeed, _threshold);

            if (current > minSpeedPct)
            {
                PlayOneShot(Database.Player, "STEP", transform.position);
                _stepsSmoke.Play();
            }
            else
            {
                _stepsSmoke.Stop();
                return;
            }

            float rnd = Random.value;
            if (rnd < _stepProbability)
                _stepsSmoke.Emit(1);
            //_stepsSmoke.Play();
            //Debug.Log("Step SoundAndParticles");
        }

        public void Jump()
        {
            PlayOneShot(Database.Player, "JUMP", transform.position);
        }

        public void FallHit()
        {
            PlayOneShot(Database.Player, "FALL_HIT", transform.position);
        }
        #endregion

        #region Private Methods
        private void PlayOneShot(Database database, string name, Vector3 position)
        {
            AudioManager.GetAudioManager().PlayOneShot(database, name, position);
        }
        #endregion
    }
}
