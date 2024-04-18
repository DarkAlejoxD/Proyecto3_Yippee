using AvatarController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarController.LedgeGrabbing
{
    public class PlayerLedgeGrab : MonoBehaviour
    {
        [SerializeField] private Transform _headRayOrigin;
        [SerializeField] private Transform _chestRayOrigin;
        [SerializeField] private float _rayLength = 0.25f;

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

        public bool GrabingLedge => _grabbingLedge;

        //DEBUG
        private bool ShowLedgeDetectionRays => (_jumpController.IsFailling || _grabbingLedge) && !_jumpController.IsGrounded;


        #region Unity Logic

        private void Awake()
        {
            _jumpController = GetComponent<PlayerJump>();
        }


        void Update()
        {
            if (!_jumpController.IsFailling && !_grabbingLedge) return;
            
            CastCheckerRays();      
            HandleLedgeLogic();

            if (_grabbingLedge)
            {
                //Cast dos raycast a los lados para detectar cuando llegamos a un borde y no dejar moverse?
                CastEdgeDetectionRays();
            }
        }

        #endregion

        #region Private Methods

        private void CastCheckerRays()
        {
            _headRay = new Ray(_headRayOrigin.position, transform.forward);
            _chestRay = new Ray(_chestRayOrigin.position, transform.forward);

            _headHit = CastRay(_headRay);
            _chestHit = CastRay(_chestRay);
        }

        private void CastEdgeDetectionRays()
        {
            Ray rayR = new Ray(transform.position + transform.right * _sideEdgeDetectionOffset, transform.forward);
            Ray rayL = new Ray(transform.position - transform.right * _sideEdgeDetectionOffset, transform.forward);

            _rightEdgeReached = Physics.Raycast(rayR, _rayLength);
            _leftEdgeReached = Physics.Raycast(rayL, _rayLength);


            bool edgeReached = !_rightEdgeReached || !_leftEdgeReached;
            if(edgeReached)
            {
                //No poder moverse para all�
                //O caer
                LetGoLedge();
            }
        }

        private bool CastRay(Ray ray)
        {
            return Physics.Raycast(ray, out _hitInfo, _rayLength);
        }


        private void HandleLedgeLogic()
        {
            //Si el ray del pecho colisiona pero no el de la cabeza significa que hemos detectado un borde
            if (!_ledgeDetected)
            {
                if (_chestHit && !_headHit)
                    _ledgeDetected = true;
            }

            //Si el borde esta detectado y choca el de la cabeza agarrarse
            if (_ledgeDetected)
            {
                if (_chestHit && _headHit)
                {
                    if (!_grabbingLedge)
                    {
                        GrabLedge();
                    }
                }

                //Soltarse? DEBUG                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    LetGoLedge();
                }
            }
        }

        private void GrabLedge()
        {
            _jumpController.StopGravity();
            _jumpController.SetLedgeGrab(true);
            GetComponent<PlayerMovement>().SetGrabbingLedgeMode(_hitInfo.normal);
            transform.rotation = Quaternion.LookRotation(-_hitInfo.normal);

            _grabbingLedge = true;
        }

        public void LetGoLedge()
        {
            GetComponent<PlayerMovement>().DisableGrabbingLedgeMode();
            _jumpController.EnableGravity();
            _jumpController.SetLedgeGrab(false);
            _ledgeDetected = false;
            _grabbingLedge = false;
        }

        #endregion

        #region DEBUG
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            if(ShowLedgeDetectionRays)
                DrawLedgeDetectionRays();

            if(_grabbingLedge)
                DrawEdgeDetectionRays();

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
            Gizmos.DrawLine(pos, pos + transform.forward * _rayLength);

            pos = transform.position - transform.right * _sideEdgeDetectionOffset;

            Gizmos.color = _chestHit ? Color.green : Color.red;
            Gizmos.DrawLine(pos, pos + transform.forward * _rayLength);
        }

        #endregion
    }
}
