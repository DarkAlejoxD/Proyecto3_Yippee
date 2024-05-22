using System;
using System.Collections;
using UnityEngine;
using UtilsComplements;

namespace GhostView
{
    [SelectionBase]
    public class GhostView_Instance : MonoBehaviour
    {
        #region Fields
        private const string COLOR_ID = "_BaseColor";

        [Header("Render References")]
        [SerializeField] private Transform _art;
        [SerializeField] private Color _startColor;
        [Tooltip("true: appears when button down" +
                 "\nfalse: dissapears when button down")]
        [SerializeField] private bool _inversed;
        private Renderer _renderer;
        private Collider _collider;
        private bool _isPlayerInside;

        private MaterialPropertyBlock _materialPropertyBlock;
        private MaterialPropertyBlock ThisMaterialPropertyBlock
        {
            get
            {
                if (_materialPropertyBlock == null)
                    _materialPropertyBlock = new MaterialPropertyBlock();

                return _materialPropertyBlock;
            }
        }
        #endregion

        #region Unity Logic
        private void Awake()
        {
            _renderer = _art.GetComponent<Renderer>();
            _collider = GetComponent<Collider>();
            GhostViewManager.OnActivateGhostView += GhostView;
        }

        private void OnEnable()
        {
            ApplyColor(_startColor);
        }
        private void OnValidate()
        {
            ApplyColor(_startColor);
        }

        private void Start()
        {
            if (_inversed)
            {
                SetActiveCollision(true);
                _art.gameObject.SetActive(true);
            }
            else
            {
                SetActiveCollision(false);
                _art.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (!_collider)
                return;

            if (!_collider.isTrigger)
                return;

            if (!_isPlayerInside)
                return;

            _collider.isTrigger = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                _isPlayerInside = true;
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
                _isPlayerInside = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                _isPlayerInside = false;
        }

        private void OnDestroy()
        {
            GhostViewManager.OnActivateGhostView -= GhostView;
        }
        #endregion

        #region Private Methods
        private void ApplyColor(Color color)
        {
            ThisMaterialPropertyBlock.SetColor(COLOR_ID, color);
            _art.GetComponent<Renderer>().SetPropertyBlock(ThisMaterialPropertyBlock);
        }

        private void SetActiveCollision(bool state)
        {
            if (!_collider)
                return;

            if (state)
            {
                _isPlayerInside = false;
                _collider.enabled = true;
                _collider.isTrigger = true;
            }
            else
            {
                _collider.enabled = false;
            }
        }

        private void GhostView(Vector3 origin, float radius)
        {
            Debug.Log("Reach");
            float distance = Vector3.Distance(origin, transform.position);

            if (distance > radius)
                return;

            StopAllCoroutines();

            if (_inversed)
            {
                SetActiveCollision(false);
                StartCoroutine(DissapearCoroutine(() =>
                {
                    _art.gameObject.SetActive(false);
                    StartCoroutine(StayCoroutine(() =>
                    {
                        _art.gameObject.SetActive(true);
                        StartCoroutine(AppearCoroutine(() =>
                        {
                            SetActiveCollision(true);
                            ThisMaterialPropertyBlock.SetColor(COLOR_ID, _startColor);
                            _renderer.SetPropertyBlock(ThisMaterialPropertyBlock);
                        }, false));
                    }, false));
                }, false));
            }
            else
            {
                SetActiveCollision(true);
                _art.gameObject.SetActive(true);
                StartCoroutine(AppearCoroutine(() =>
                {
                    StartCoroutine(StayCoroutine(() =>
                    {
                        StartCoroutine(DissapearCoroutine(() =>
                        {
                            SetActiveCollision(false);
                            _art.gameObject.SetActive(false);
                            ThisMaterialPropertyBlock.SetColor(COLOR_ID, _startColor);
                            _renderer.SetPropertyBlock(ThisMaterialPropertyBlock);
                        }));
                    }));
                }));
            }
        }

        private IEnumerator AppearCoroutine(Action end, bool firstAppear = true)
        {
            if (ISingleton<GhostViewManager>.TryGetInstance(out var manager))
            {
                Color startColor = manager._proto.AppearColor;
                float timeToAppear = firstAppear ? manager._proto.AppearTime : manager._proto.DisapearTime;

                ThisMaterialPropertyBlock.SetColor(COLOR_ID, startColor);
                _renderer.SetPropertyBlock(ThisMaterialPropertyBlock);
                yield return new WaitForSeconds(timeToAppear);
                end.Invoke();
            }
        }

        private IEnumerator StayCoroutine(Action end, bool firstAppear = true)
        {
            if (ISingleton<GhostViewManager>.TryGetInstance(out var manager))
            {
                ThisMaterialPropertyBlock.SetColor(COLOR_ID, _startColor);
                _renderer.SetPropertyBlock(ThisMaterialPropertyBlock);
                yield return new WaitForSeconds(manager._proto.Staytime);
                end.Invoke();
            }
        }

        private IEnumerator DissapearCoroutine(Action end, bool firstAppear = true)
        {
            if (ISingleton<GhostViewManager>.TryGetInstance(out var manager))
            {
                Color startColor = manager._proto.DissapearColor;
                float timeToAppear = firstAppear ? manager._proto.DisapearTime : manager._proto.AppearTime;

                ThisMaterialPropertyBlock.SetColor(COLOR_ID, startColor);
                _renderer.SetPropertyBlock(ThisMaterialPropertyBlock);
                yield return new WaitForSeconds(timeToAppear);
                end.Invoke();
            }
        }
        #endregion
    }
}
