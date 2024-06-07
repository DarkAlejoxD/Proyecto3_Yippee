using System.Collections;
using UnityEngine;
using Cinemachine;
using BaseGame;
using UtilsComplements;

namespace Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraEvent : MonoBehaviour
    {
        [Header("References")]
        private CinemachineVirtualCamera _thisCamera;
        private CinemachineVirtualCamera _lastCamera;

        [SerializeField] private float _timeStolen = 3;

        private void Start()
        {
            _thisCamera = GetComponent<CinemachineVirtualCamera>();
            _thisCamera.enabled = false;
        }

        public void ActivateCamera()
        {
            StartCoroutine(SwitchCameraCoroutine());
        }

        private IEnumerator SwitchCameraCoroutine()
        {
            if (Singleton.TryGetInstance(out CameraManager manager))
            {
                GameManager.GetGameManager().PlayerInstance.BlockMovement();

                _lastCamera = manager.ActiveCamera;
                manager.SwitchCameras(_thisCamera);

                yield return new WaitForSeconds(_timeStolen);
                manager.SwitchCameras(_lastCamera);
                GameManager.GetGameManager().PlayerInstance.UnBlockMovement();
            }
            else
                yield return null;
        }
    }
}
