using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UtilsComplements;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _cameraSmooth;
    [SerializeField] private float _yOffset = -10;

    private Transform _target;
    private Transform _player;


    //A

    // Start is called before the first frame update
    void Start()
    {
        _player = ISingleton<GameManager>.GetInstance().PlayerInstance.transform;
        _target = _player;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateYPos();
    }

    private void UpdateYPos()
    {
        Vector3 pos = _target.position;
        pos.x = transform.position.x;
        pos.z = transform.position.z;
        pos.y = pos.y + _yOffset;
        transform.position = Vector3.Lerp(transform.position, pos, _cameraSmooth * Time.deltaTime);
    }
}
