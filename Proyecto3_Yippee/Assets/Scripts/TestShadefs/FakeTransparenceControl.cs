using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BaseGame
{
    public class FakeTransparenceControl : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField] private LayerMask _layers;
        [SerializeField, Min(0.01f)] private float _radius = 1;
        [SerializeField, Min(0.01f)] private float _expansionVelocity = 1;
        [SerializeField, Min(15)] private int _refreshRate = 24; // 24 fps/s
        private float _timeControl = 0;
        private float _radiusControl = 0;

        private Camera CurrentCamera => Camera.main;
        private float TimeToRefreshRaycasts => 1f / _refreshRate;

        private void Start()
        {
            _timeControl = Time.time;
        }

        private void FixedUpdate()
        {
            UpdateRaycast(Time.fixedDeltaTime);
        }

        private void UpdateRaycast(float dt)
        {
            _timeControl += dt;
            if (_timeControl >= TimeToRefreshRaycasts)
            {
                _timeControl -= TimeToRefreshRaycasts;

                //From Center
                if (SendRaycastToCamera(transform.position))
                {
                    ActivateSphere();
                    return;
                }

                Vector3 up = CurrentCamera.transform.up;

                //From up
                if (SendRaycastToCamera(transform.position + up * _radius))
                {
                    ActivateSphere();
                    return;
                }

                //From down
                if (SendRaycastToCamera(transform.position - up * _radius))
                {
                    ActivateSphere();
                    return;
                }

                Vector3 right = CurrentCamera.transform.right;

                //From right
                if (SendRaycastToCamera(transform.position + right * _radius))
                {
                    ActivateSphere();
                    return;
                }

                //From left
                if (SendRaycastToCamera(transform.position - right * _radius))
                {
                    ActivateSphere();
                    return;
                }

                DeactivateSphere();

                SendRaycastToCamera(transform.position);
            }
        }

        private void DeactivateSphere()
        {
            Debug.Log("Deactivate FakeTranparency");
        }

        private void ActivateSphere()
        {
            Debug.Log("Activate FakeTransparency");
        }

        private bool SendRaycastToCamera(Vector3 position)
        {
            Vector3 final = CurrentCamera.transform.position;
            Vector3 direction = final - position;
            float offset = CurrentCamera.nearClipPlane + 0.1f;

            float distance = direction.magnitude - offset;

            Ray ray = new(position, direction);
            bool hit = Physics.Raycast(ray, distance, _layers);

            return hit;
        }

        #region DEBUG
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Color color = new(34 / 255f, 0f, 118 / 255f);

            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, _radius);
            Gizmos.color = Color.white;

            const float distance_Draw = 1f;
            const float distance_Bet_Lines = 5f;

            Vector3 center;
            Vector3 finalPos;
            Vector3 direction;
            Vector3 up = CurrentCamera.transform.up;
            Vector3 right = CurrentCamera.transform.right;

            float offset = CurrentCamera.nearClipPlane + 0.1f;
            float distance;
            Ray ray;
            bool hit;

            #region Center
            //Calculate Center
            center = transform.position;
            direction = CurrentCamera.transform.position - center;
            distance = direction.magnitude - offset;
            ray = new(center, direction);
            hit = Physics.Raycast(ray, distance, _layers);
            finalPos = center + direction.normalized * distance_Draw;

            Handles.color = !hit ? Color.green : Color.red;
            Handles.DrawDottedLine(center, finalPos, distance_Bet_Lines);
            #endregion

            #region Up
            //Calculate Up
            center = transform.position + up * _radius;
            direction = CurrentCamera.transform.position - center;
            distance = direction.magnitude - offset;
            ray = new(center, direction);
            hit = Physics.Raycast(ray, distance, _layers);
            finalPos = center + direction.normalized * distance_Draw;

            Handles.color = !hit ? Color.green : Color.red;
            Handles.DrawDottedLine(center, finalPos, distance_Bet_Lines);
            #endregion

            #region Down
            //Calculate Down
            center = transform.position - up * _radius;
            direction = CurrentCamera.transform.position - center;
            distance = direction.magnitude - offset;
            ray = new(center, direction);
            hit = Physics.Raycast(ray, distance, _layers);
            finalPos = center + direction.normalized * distance_Draw;

            Handles.color = !hit ? Color.green : Color.red;
            Handles.DrawDottedLine(center, finalPos, distance_Bet_Lines);
            #endregion

            #region Right
            //Calculate Down
            center = transform.position + right * _radius;
            direction = CurrentCamera.transform.position - center;
            distance = direction.magnitude - offset;
            ray = new(center, direction);
            hit = Physics.Raycast(ray, distance, _layers);
            finalPos = center + direction.normalized * distance_Draw;

            Handles.color = !hit ? Color.green : Color.red;
            Handles.DrawDottedLine(center, finalPos, distance_Bet_Lines);
            #endregion

            #region Left
            //Calculate Down
            center = transform.position - right * _radius;
            direction = CurrentCamera.transform.position - center;
            distance = direction.magnitude - offset;
            ray = new(center, direction);
            hit = Physics.Raycast(ray, distance, _layers);
            finalPos = center + direction.normalized * distance_Draw;

            Handles.color = !hit ? Color.green : Color.red;
            Handles.DrawDottedLine(center, finalPos, distance_Bet_Lines);
            #endregion

            Handles.color = Color.white;
        }
#endif
        #endregion
    }
}
