using System.Collections;
using UnityEngine;
using Cinemachine;
using BaseGame;
using UtilsComplements;

namespace Cameras
{
    [RequireComponent(typeof(Collider))]
    public class CameraEvent : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CinemachineVirtualCamera _thisCamera;
        private CinemachineVirtualCamera _lastCamera;

        [SerializeField] private float _timeStolen = 3;
        [SerializeField] private int _timesToStole = 1;
        private int _timesControl = 0;

        private void Start()
        {
            _timeStolen = 0;
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

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
