using UnityEngine;

namespace AvatarController.Animations
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        [Header("Step References")]
        [SerializeField] private ParticleSystem _stepsSmoke;

        [Header("Jump References")]
        [SerializeField] private ParticleSystem _jumpSmoke;

        public void Step()
        {
            _stepsSmoke.Play();
            Debug.Log("Step SoundAndParticles");
        }

        public void Jump()
        {
            Debug.Log("Jump SoundAndParticles");
        }
    }
}
