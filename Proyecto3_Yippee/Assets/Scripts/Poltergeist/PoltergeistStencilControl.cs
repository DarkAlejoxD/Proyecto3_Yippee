using UnityEngine;
using UtilsComplements;

namespace BaseGame
{
    public class PoltergeistStencilControl : MonoBehaviour, ISingleton<PoltergeistStencilControl>
    {
        private enum PolterStates
        {
            NONE,
            EXPANDING,
            REDUCING
        }

        [Header("References")]
        [SerializeField] private Transform _player;
        [SerializeField] private Transform _stencil;
        [SerializeField, Min(0.01f)] private float _threshold = 0.3f;
        private PolterStates _state;

        public ISingleton<PoltergeistStencilControl> Instance => this;

        private void Awake() => Instance.Instantiate();

        private void Start()
        {
            _state = PolterStates.NONE;
        }

        private void Update()
        {
            transform.position = _player.position;
        }

        private void OnDestroy() => Instance.RemoveInstance();
    }
}
