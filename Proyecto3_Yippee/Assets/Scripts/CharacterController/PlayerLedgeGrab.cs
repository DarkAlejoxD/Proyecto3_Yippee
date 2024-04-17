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

        private Ray _headRay;
        private Ray _chestRay;

        private bool _chestHit;
        private bool _headHit;
        private RaycastHit _hitInfo;

        private bool _ledgeDetected;
        private bool _grabbingLedge;

        private PlayerJump _jumpController;


        public bool GrabingLedge => _grabbingLedge;

        private void Awake()
        {
            _jumpController = GetComponent<PlayerJump>();
        }


        void Update()
        {
            if (!_jumpController.IsFailling && !_grabbingLedge) return;

            _headRay = new Ray(_headRayOrigin.position, transform.forward);
            _chestRay = new Ray(_chestRayOrigin.position, transform.forward);

            _headHit = CastRay(_headRay);
            _chestHit = CastRay(_chestRay);

            HandleLedgeLogic();
            if (_grabbingLedge)
            {
                //Cast dos raycast a los lados para detectar cuando llegamos a un borde y no dejar moverse?
            }
        }


        #region Private Methods

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
            GetComponent<PlayerMovement>().SetGrabbingLedgeMode(_hitInfo.normal);
            transform.rotation = Quaternion.LookRotation(-_hitInfo.normal);

            _grabbingLedge = true;
        }

        private void LetGoLedge()
        {
            GetComponent<PlayerMovement>().DisableGrabbingLedgeMode();
            _jumpController.EnableGravity();
            _ledgeDetected = false;
            _grabbingLedge = false;

        }

        #endregion

        #region DEBUG
        private void OnDrawGizmos()
        {
            Vector3 headpos = _headRayOrigin.position + transform.forward * _rayLength;
            Vector3 chestpos = _chestRayOrigin.position + transform.forward * _rayLength;

            Gizmos.color = _headHit ? Color.green : Color.red;
            Gizmos.DrawLine(_headRayOrigin.position, headpos);

            Gizmos.color = _chestHit ? Color.green : Color.red;
            Gizmos.DrawLine(_chestRayOrigin.position, chestpos);
        }
        #endregion
    }
}
