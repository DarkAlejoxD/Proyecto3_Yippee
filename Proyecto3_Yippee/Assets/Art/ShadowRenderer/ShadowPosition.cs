using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseGame
{
    [ExecuteAlways]
    public class ShadowPosition : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _footHandler;
        [SerializeField] private Transform _shadow;
        [SerializeField, Min(0.01f)] private float _offset = 0.1f;
        [SerializeField] private LayerMask _layerMask;

        private void LateUpdate()
        {
            Ray ray = new(_footHandler.position, Vector3.down);

            bool hit = Physics.Raycast(ray, out RaycastHit info, 100000, _layerMask);

            if (hit)
            {
                _shadow.gameObject.SetActive(true);
                _shadow.position = info.point + info.normal * _offset;
                _shadow.forward = info.normal;
            }
            else
            {
                _shadow.gameObject.SetActive(false);
            }
        }
    }
}
