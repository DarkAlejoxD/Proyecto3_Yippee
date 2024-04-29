using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsComplements;

public class CameraManager : MonoBehaviour, ISingleton<CameraManager>
{

    private CinemachineVirtualCamera _activeCamera;


    public CinemachineVirtualCamera ActiveCamera => _activeCamera;

    public ISingleton<CameraManager> Instance => this;

    private void Awake()
    {
        Instance.Instantiate();
    }


    // Start is called before the first frame update
    void Start()
    {
        //_activeCamera = Camera.main; //DEBUG
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        Instance.RemoveInstance();
    }

    public void SwitchCameras(CinemachineVirtualCamera cam)
    {
        if(_activeCamera != null)
            _activeCamera.enabled = false;

        cam.enabled = true;
        _activeCamera = cam;

    }
}
