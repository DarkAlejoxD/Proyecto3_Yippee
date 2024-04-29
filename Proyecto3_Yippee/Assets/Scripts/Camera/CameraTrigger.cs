using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsComplements;

public class CameraTrigger : MonoBehaviour
{
    private CameraManager _manager;
    [SerializeField] private CinemachineVirtualCamera _cam;
    [SerializeField] private bool isStartingCamera = false;


    private void Awake()
    {
        //_cam = GetComponentInChildren<CinemachineVirtualCamera>();

        

    }

    private void Start()
    {
        _manager = ISingleton<CameraManager>.GetInstance();
        if (isStartingCamera) _manager.SwitchCameras(_cam);
        
    }

    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("Camera confiner entered");
        if (other.CompareTag("Player"))
        {
            _manager.SwitchCameras(_cam);
        }
    }
}
