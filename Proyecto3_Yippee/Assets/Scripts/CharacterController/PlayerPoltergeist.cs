using System.Collections;
using UnityEngine;
using AvatarController.Data;
using Poltergeist;
using UtilsComplements.Editor;

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
        private Transform CameraTransform => Camera.main.transform;
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
            _evaluatedPoltergeistZone.ObjectAttached.isKinematic = true;
        }

        private void PoltergeistModeUpdate(Vector2 xzDirection, float yDirection) //Maybe encapsulate some funcitons?? idk toi cansado jefe
        {
            //Transform the input by the camera
            Vector3 forward = CameraTransform.forward;
            forward.y = 0;
            forward.Normalize();
            Vector3 right = CameraTransform.right;
            right.y = 0;
            right.Normalize();

            Vector3 movement = xzDirection.x * forward + xzDirection.y * right + yDirection * Vector3.up;
            movement.Normalize();


            //Realize the movment
            Rigidbody rb = _evaluatedPoltergeistZone.ObjectAttached;
            Vector3 motion = DataContainer.DefOtherValues.Speed * Time.deltaTime * movement;
            Vector3 newPos = rb.position + motion;

            //Check if is far
            float distance = Vector3.Distance(transform.position, newPos);
            if (distance > DataContainer.DefOtherValues.PoltergeistRadius)
            {
                Vector3 direction = newPos - transform.position;
                direction.Normalize();
                newPos = transform.position + direction * DataContainer.DefOtherValues.PoltergeistRadius;
            }

            rb.MovePosition(newPos);
        }

        private void ExitPoltergeistMode(bool value)
        {
            if (value)
            {
                _evaluatedPoltergeistZone.ObjectAttached.isKinematic = _evaluatedPoltergeistZone.StartedKinematic;
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

        #region DEBUG
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_controller)
                _controller = GetComponent<PlayerController>();

            GizmosUtilities.DrawSphere(transform.position, Color.magenta,
                                       DataContainer.DefOtherValues.PoltergeistRadius,
                                       DataContainer.DefOtherValues.DEBUG_DrawPoltergeistRadius);
        }
#endif
        #endregion
    }
}
