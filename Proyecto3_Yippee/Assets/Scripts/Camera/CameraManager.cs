using UnityEngine;
using Cinemachine;
using UtilsComplements;

namespace Cameras
{
    public class CameraManager : MonoBehaviour, ISingleton<CameraManager>
    {
        private CinemachineVirtualCamera _activeCamera;

        public CinemachineVirtualCamera ActiveCamera => _activeCamera;

        public ISingleton<CameraManager> Instance => this;

        private void Awake()
        {
            Instance.Instantiate();
        }

        private void OnDestroy()
        {
            Instance.RemoveInstance();
        }

        public void SwitchCameras(CinemachineVirtualCamera cam)
        {
            if (_activeCamera != null)
                _activeCamera.enabled = false;

            cam.enabled = true;
            _activeCamera = cam;
        }
    }
}