using UnityEngine;
using AvatarController.PlayerFSM;

namespace AvatarController.LedgeGrabbing
{
    [RequireComponent(typeof(PlayerJump), typeof(PlayerMovement), typeof(PlayerController))]
    public class PlayerLedgeGrab : MonoBehaviour
    {
        [SerializeField] private Transform _headRayOrigin;
        [SerializeField] private Transform _chestRayOrigin;
        [SerializeField] private float _rayLength = 0.75f;
        [SerializeField] private float _edgeRayLength = 1.5f;
        [SerializeField] private float _positionToWallOffset = 0.5f;
        [SerializeField] private LayerMask _grabbableLayers;

        [SerializeField] private float _sideEdgeDetectionOffset = 0.5f;

        private Ray _headRay;
        private Ray _chestRay;

        private bool _chestHit;
        private bool _headHit;
        private bool _rightEdgeReached;
        private bool _leftEdgeReached;
        private RaycastHit _hitInfo;

        private bool _ledgeDetected;
        private bool _grabbingLedge;

        private PlayerJump _jumpController;
        private PlayerController _playerController;

        public bool GrabingLedge => _grabbingLedge;

        //DEBUG
        private bool ShowLedgeDetectionRays => true;//=> (_jumpController.IsFailling || _grabbingLedge) && !_jumpController.IsGrounded;


        #region Unity Logic

        private void Awake()
        {
            _jumpController = GetComponent<PlayerJump>();
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            if (_playerController == null)
                _playerController = GetComponent<PlayerController>();

            _playerController.OnGrabCheck += GrabUpdate;
            _playerController.OnGrabUpdate += OnGrabUpdate;
        }

        private void OnDisable()
        {
            if (_playerController == null)
                _playerController = GetComponent<PlayerController>();

            _playerController.OnGrabCheck -= GrabUpdate;
            _playerController.OnGrabUpdate -= OnGrabUpdate;
        }

        void GrabUpdate()
        {
            //if (_grabbingLedge)
            //    return;

            CastCheckerRays();
            HandleLedgeLogic();
        }

        private void OnGrabUpdate()
        {
            //Cast dos raycast a los lados para detectar cuando llegamos a un borde y no dejar moverse?
            HandleNormalRotation();
            CastEdgeDetectionRays();

            //Get nearestPoint To the player
            Vector3 hitNormal = _hitInfo.normal;
            //Ray rayToNearPos = new(_headRayOrigin.position, -hitNormal);
            //CastRay(rayToNearPos);

            //Attach to the ledge
            Vector3 pos = _hitInfo.point;
            pos.y = transform.position.y;
            pos.z += _positionToWallOffset * _hitInfo.normal.z;
            pos.x += _positionToWallOffset * _hitInfo.normal.x;

            //transform.position = pos;
            transform.position = pos;
        }
        #endregion

        #region Private Methods
        private void CastCheckerRays()
        {
            _headRay = new Ray(_headRayOrigin.position, transform.forward);
            _chestRay = new Ray(_chestRayOrigin.position, transform.forward);

            _headHit = CastRay(_headRay);
            _chestHit = CastRay(_chestRay);

            //Debug.Log(_hitInfo.normal);
        }

        private void CastEdgeDetectionRays()
        {
            Ray rayR = new Ray(transform.position + transform.right * _sideEdgeDetectionOffset, transform.forward);
            Ray rayL = new Ray(transform.position - transform.right * _sideEdgeDetectionOffset, transform.forward);

            _rightEdgeReached = Physics.Raycast(rayR, _edgeRayLength, _grabbableLayers);
            _leftEdgeReached = Physics.Raycast(rayL, _edgeRayLength, _grabbableLayers);


            bool edgeReached = !_rightEdgeReached || !_leftEdgeReached;
            if (edgeReached)
            {
                //No poder moverse para allá
                //O caer
                LetGoLedge();
            }
        }

        private bool CastRay(Ray ray)
        {
            return Physics.Raycast(ray, out _hitInfo, _rayLength, _grabbableLayers);
        }


        private void HandleLedgeLogic()
        {
            //Si el ray del pecho colisiona pero no el de la cabeza significa que hemos detectado un borde
            if (!_ledgeDetected)
            {
                if (_chestHit && !_headHit)
                {
                    _ledgeDetected = true;
                    GrabLedge();
                }
            }

            //Si el borde esta detectado y choca el de la cabeza agarrarse
            //if (_ledgeDetected)
            //{
            //    if (_chestHit && _headHit)
            //    {
            //        if (!_grabbingLedge)
            //        {
            //            GrabLedge();
            //        }
            //    }
            //}
        }

        private void GrabLedge()
        {
            _playerController.SetGravityActive(false);
            _jumpController.SetLedgeGrab(true);

            GetComponent<PlayerMovement>().SetGrabbingLedgeMode(_hitInfo.normal);

            //GetComponent<PlayerMovement>().enabled = true;

            _playerController.ForceChangeState(PlayerStates.Grabbing);
            _grabbingLedge = true;

        }

        public void LetGoLedge()
        {
            GetComponent<PlayerMovement>().DisableGrabbingLedgeMode();
            _playerController.ReturnState();
            _playerController.SetGravityActive(true);
            _playerController.ForceChangeState(PlayerStates.OnGround);
            _jumpController.SetLedgeGrab(false);
            _ledgeDetected = false;
            _grabbingLedge = false;
        }

        private void HandleNormalRotation()
        {
            Vector3 normal = -_hitInfo.normal;
            normal.y = 0;
            Quaternion rot = Quaternion.LookRotation(normal);
            if (transform.rotation != rot)
            {
                transform.rotation = rot;

                GetComponent<PlayerMovement>().SetGrabbingLedgeMode(_hitInfo.normal);

                //update position
                //Vector3 pos = _hitInfo.point;
                //pos.y = transform.position.y;
                //pos.z += _positionToWallOffset * _hitInfo.normal.z;
                //pos.x += _positionToWallOffset * _hitInfo.normal.x;

                //_playerController.RequestTeleport(pos);
            }
        }

        #endregion

        #region DEBUG
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            if (ShowLedgeDetectionRays)
                DrawLedgeDetectionRays();

            if (_grabbingLedge)
                DrawEdgeDetectionRays();

            if (!_hitInfo.Equals(null))
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(_hitInfo.point, 0.1f);
                Gizmos.color = Color.white;
            }
        }

        private void DrawLedgeDetectionRays()
        {
            Vector3 headpos = _headRayOrigin.position + transform.forward * _rayLength;
            Vector3 chestpos = _chestRayOrigin.position + transform.forward * _rayLength;

            Gizmos.color = _headHit ? Color.green : Color.red;
            Gizmos.DrawLine(_headRayOrigin.position, headpos);

            Gizmos.color = _chestHit ? Color.green : Color.red;
            Gizmos.DrawLine(_chestRayOrigin.position, chestpos);
        }

        private void DrawEdgeDetectionRays()
        {
            Vector3 pos = transform.position + transform.right * _sideEdgeDetectionOffset;

            Gizmos.color = _rightEdgeReached ? Color.green : Color.red;
            Gizmos.DrawLine(pos, pos + transform.forward * _edgeRayLength);

            pos = transform.position - transform.right * _sideEdgeDetectionOffset;

            Gizmos.color = _chestHit ? Color.green : Color.red;
            Gizmos.DrawLine(pos, pos + transform.forward * _edgeRayLength);
        }

        #endregion
    }
}
