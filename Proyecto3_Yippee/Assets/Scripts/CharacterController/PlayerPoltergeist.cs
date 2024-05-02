using UnityEngine;
using AvatarController.Data;
using UtilsComplements.Editor;

namespace AvatarController
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerPoltergeist : MonoBehaviour
    {
        #region Fields
        [Header("References")]
        private PlayerController _controller;

        private bool _canEnterPoltegeist;
        private PlayerData DataContainer => _controller.DataContainer;
        private Transform CameraTransform => Camera.main.transform;

        [Header("DEBUG")]
        [SerializeField] private Color DEBUG_gizmosColor;
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
        }

        private void OnDisable()
        {
            _controller.OnPoltergeistEnter -= EnterPoltergeistMode;
            _controller.OnPoltergeistStay -= PoltergeistModeUpdate;
        }
        #endregion

        #region Private Methods
        private void EnterPoltergeistMode()
        {
            //StartCoroutine(PolterCooldownCoroutine());
            //_evaluatedPoltergeistZone.ObjectAttached.useGravity = false;
        }

        private void PoltergeistModeUpdate(Vector2 xzDirection, float yDirection)
        {
            ////Transform the input by the camera
            //Vector3 forward = CameraTransform.forward;
            //forward.y = 0;
            //forward.Normalize();
            //Vector3 right = CameraTransform.right;
            //right.y = 0;
            //right.Normalize();

            //Vector3 movement = xzDirection.y * forward + xzDirection.x * right + yDirection * Vector3.up;
            //movement.Normalize();

            ////Realize the movement
            //Rigidbody rb = null;// _evaluatedPoltergeistZone.ObjectAttached;
            //Vector3 motion = DataContainer.DefOtherValues.Speed * Time.deltaTime * movement;
            //Vector3 newPos = rb.position + motion;

            ////Check if is far
            //float distance = Vector3.Distance(transform.position, newPos);
            //if (distance > DataContainer.DefOtherValues.PoltergeistRadius)
            //{
            //    Vector3 direction = newPos - transform.position;
            //    direction.Normalize();
            //    newPos = transform.position + direction * DataContainer.DefOtherValues.PoltergeistRadius;
            //}

            //if (distance < DataContainer.DefOtherValues.PlayerRadius)
            //{
            //    Vector3 direction = newPos - transform.position;
            //    direction.Normalize();
            //    newPos = transform.position + direction * DataContainer.DefOtherValues.PlayerRadius;
            //}
            //rb.MovePosition(newPos);
            //rb.velocity = Vector3.zero;
        }
        #endregion

        #region DEBUG
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!_controller)
                _controller = GetComponent<PlayerController>();

            GizmosUtilities.DrawSphere(transform.position, DEBUG_gizmosColor,
                                       DataContainer.DefPoltValues.PoltergeistRadius,
                                       DataContainer.DefPoltValues.DEBUG_DrawPoltergeistRadius);
            GizmosUtilities.DrawSphere(transform.position, DEBUG_gizmosColor,
                                       DataContainer.DefPoltValues.PlayerRadius,
                                       DataContainer.DefPoltValues.DEBUG_DrawPoltergeistRadius);
        }
#endif
        #endregion
    }
}
