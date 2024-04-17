using UnityEngine;
using AvatarController.Data;
using Poltergeist;
using System.Collections;

namespace AvatarController
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerPoltergeist : MonoBehaviour
    {
        #region Fields
        [Header("References")]
        private Interactable_PoltergeistZone _evaluatedPoltergeistZone;
        private PlayerController _controller;
        private PlayerStates _lastState;

        private bool _canEnterPoltegeist;
        private PlayerData DataContainer => _controller.DataContainer;
        #endregion

        #region Unity Logic
        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
            _canEnterPoltegeist = true;
        }

        private void OnEnable()
        {
            if (_controller == null)
                _controller = GetComponent<PlayerController>();

            _controller.OnPoltergeistEnter += EnterPoltergeistMode;
            _controller.OnPoltergeistStay += PoltergeistModeUpdate;
            _controller.OnPoltergeistExit += ExitPoltergeistMode;
        }

        private void OnDisable()
        {
            _controller.OnPoltergeistEnter -= EnterPoltergeistMode;
            _controller.OnPoltergeistStay -= PoltergeistModeUpdate;
            _controller.OnPoltergeistExit -= ExitPoltergeistMode;
        }
        #endregion

        #region Public Methods
        public void TryEnterPoltergeist(Interactable_PoltergeistZone zone)
        {
            if (!_canEnterPoltegeist)
                return;
            _evaluatedPoltergeistZone = zone;
            _controller.RequestChangeState(PlayerStates.OnPoltergeist, out _lastState);
        }
        #endregion

        #region Private Methods
        private void EnterPoltergeistMode()
        {
            StartCoroutine(PolterCooldownCoroutine());
            _controller.RequestTeleport(_evaluatedPoltergeistZone.transform.position);
        }

        private void PoltergeistModeUpdate(Vector2 xzDirection, float yDirection)
        {

        }

        private void ExitPoltergeistMode(bool value)
        {
            if (value)
            {
                _evaluatedPoltergeistZone = null;
                _controller.RequestChangeState(_lastState);
            }
        }

        private IEnumerator PolterCooldownCoroutine()
        {
            _canEnterPoltegeist = false;
            yield return new WaitForSeconds(DataContainer.DefOtherValues.PoltergeistCD);
            _canEnterPoltegeist = true;
        }
        #endregion
    }
}
