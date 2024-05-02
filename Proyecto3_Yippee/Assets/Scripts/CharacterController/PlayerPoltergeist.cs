using UnityEngine;
using AvatarController.Data;
using UtilsComplements.Editor;
using Poltergeist;
using UtilsComplements;

namespace AvatarController
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerPoltergeist : MonoBehaviour
    {
        #region Fields
        [Header("References")]
        private PlayerController _controller;
        private PlayerData DataContainer => _controller.DataContainer;
        private Transform CameraTransform => Camera.main.transform;

        [Header("DEBUG")]
        [SerializeField] private Color DEBUG_gizmosColor;
        #endregion

        #region Unity Logic
        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
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
            //Poltergeist_Item item = Singleton.GetSingleton<PoltergeistManager>()._evaluatedPoltergeist;
            //if (item == null)
            //    return;
            //item.StartPoltergeist();
        }

        private void PoltergeistModeUpdate(Vector2 xzDirection, float yDirection)
        {
            //Transform the input by the camera
            Vector3 forward = CameraTransform.forward;
            forward.y = 0;
            forward.Normalize();
            Vector3 right = CameraTransform.right;
            right.y = 0;
            right.Normalize();

            Vector3 movement = xzDirection.y * forward + xzDirection.x * right + yDirection * Vector3.up;
            movement.Normalize();

            //Realize the movement
            Poltergeist_Item item = Singleton.GetSingleton<PoltergeistManager>()._evaluatedPoltergeist;
            if (item == null)
                return;
            Rigidbody rb = item.GetComponent<Rigidbody>();// _evaluatedPoltergeistZone.ObjectAttached;
            Vector3 motion = DataContainer.DefPoltValues.Speed * Time.deltaTime * movement;
            Vector3 newPos = rb.position + motion;

            //Check if is far
            float distance = Vector3.Distance(transform.position, newPos);
            if (distance > DataContainer.DefPoltValues.PoltergeistRadius)
            {
                Vector3 direction = newPos - transform.position;
                direction.Normalize();
                newPos = transform.position + direction * DataContainer.DefPoltValues.PoltergeistRadius;
            }

            if (distance < DataContainer.DefPoltValues.PlayerRadius)
            {
                Vector3 direction = newPos - transform.position;
                direction.Normalize();
                newPos = transform.position + direction * DataContainer.DefPoltValues.PlayerRadius;
            }
            rb.MovePosition(newPos);
            rb.velocity = Vector3.zero;
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
