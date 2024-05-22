using UnityEngine;
using Cinemachine;
using BaseGame;
using UtilsComplements;

namespace Cameras
{
    //[ExecuteAlways]
    public class CameraFollow : MonoBehaviour
    {
        [Header("References")]
        private Transform _target;

        [Header("Camera Attributes")]
        [SerializeField] private float _cameraSmooth;
        [SerializeField] private float _yOffset = -10;
        [SerializeField] private float _maxDistanceToPlayer = 10;
        [SerializeField] private float _minDistanceToPlayer = 8;

        private CinemachineVirtualCamera _camera;
        private CinemachineTransposer _bodyTransposer;
        private CinemachineComposer _aimComposer;

        //void Start()
        //{
        //    _camera = GetComponent<CinemachineVirtualCamera>();

        //    _target = GameObject.FindWithTag("Player").transform;
        //    _camera.Follow = _target;
        //    _camera.LookAt = _target;
        //}

        private void Update()
        {
            //UpdateYPos();
        }

        private void UpdateYPos()
        {
            Vector3 pos = CalculatePos();

            transform.position = Vector3.Lerp(transform.position, pos, _cameraSmooth * Time.deltaTime);
        }

        private Vector3 CalculatePos()
        {
            //Pillar la distancia del player, con un maximo de offset, si se supera hacer lerp usando el forward para acercarse o alejarse.
            //Pillar el forward
            Vector3 pos = _target.position;
            Vector3 forward = _camera.transform.forward;
            float distanceToPlayer = Vector3.Distance(_camera.transform.position, _target.position);

            Debug.Log($"Distance from camera to player: {distanceToPlayer}");

            //Si distancia es mayor q X, pillar la posición deseada (forward x distancia q sobra pa llegar a Y?)
            //Hasta que la distancia sea menor que Y
            pos.x = transform.position.x;
            pos.z = transform.position.z;

            if (distanceToPlayer > _maxDistanceToPlayer)
            {
                float exceedingDistance = distanceToPlayer - _maxDistanceToPlayer;
                Vector3 desiredPos = forward * exceedingDistance;

                pos.x += desiredPos.x;
                pos.z += desiredPos.z;
            }
            else if (distanceToPlayer < _minDistanceToPlayer)
            {
                float exceedingDistance = _minDistanceToPlayer - distanceToPlayer;
                Vector3 desiredPos = -forward * exceedingDistance;

                pos.x += desiredPos.x;
                pos.z += desiredPos.z;
            }

            pos.y = pos.y + _yOffset;
            return pos;
        }
    }
}