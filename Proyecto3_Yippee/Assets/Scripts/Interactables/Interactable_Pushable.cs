using AvatarController;
using Unity.VisualScripting;
using UnityEngine;
using UtilsComplements;

namespace Interactable //add it to a concrete namespace
{
//[RequireComponent(typeof(Transform))] //Add this if necessary, delete it otherwise
    public class Interactable_Pushable : Interactable_Base
    {
        [SerializeField] private Transform[] _pushPoints;
        private Transform _grabbedPoint;
        private PlayerController _player;

        private bool _isGrabbed;

        #region Public Methods

        private void Start()
        {
            _player = ISingleton<GameManager>.GetInstance().PlayerInstance;
        }


        public override void Interact()
        {
            base.Interact();

            //TODO: Select closest push point and tp player to it
            

            //TODO: Block horizontal movement
            //TODO: Block Jump
            //TODO: Set parent to player
            //TODO: Limites?
            //Subscribe to OnMove
            if(_isGrabbed )
            {
                LetGo();
            }
            else
            {
                Grab();

            }
            

            

        }

        private void Grab()
        {
            _grabbedPoint = _pushPoints[GetClosestPushPoint()];
            _player.transform.position = _grabbedPoint.position;
            _player.OnMovement += OnMove;
            _isGrabbed = true;
        }

        private void LetGo()
        {
            _player.OnMovement -= OnMove;
            _isGrabbed = false;

        }

        #endregion

        #region Private Methods

        private int GetClosestPushPoint()
        {
            return MathUtils.GetClosestPoint(_player.transform.position, _pushPoints);
        }

        private Vector3 GetDirection()
        {
            Vector3 dir = transform.position - _grabbedPoint.position;
            dir.Normalize();

            return dir;
        }

        private void OnMove(Vector2 input)
        {
            Vector3 movement = Vector3.zero;            

            movement = GetDirection() * input.y * 5 * Time.deltaTime;
            transform.position += movement;
        }

        #endregion

    }
}
