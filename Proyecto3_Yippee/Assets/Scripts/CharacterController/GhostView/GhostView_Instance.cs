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
        [SerializeField] private bool _inversed;
        private Renderer _renderer;
        private Collider _collider;

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
            TryGetComponent(out _collider);
            GhostViewManager.OnActivateGhostView += GhostView;
        }

        private void OnEnable()
        {
            ApplyColor(_startColor);
        }

        private void Start()
        {
            if (_inversed)
            {
                if (_collider)
                    _collider.enabled = true;
                _art.gameObject.SetActive(true);
            }
            else
            {
                if (_collider)
                    _collider.enabled = false;
                _art.gameObject.SetActive(false);
            }
        }

        private void OnValidate()
        {
            ApplyColor(_startColor);
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

        private void GhostView(Vector3 origin, float radius)
        {
            Debug.Log("Reach");
            float distance = Vector3.Distance(origin, transform.position);

            if (distance > radius)
                return;

            StopAllCoroutines();

            if (_inversed)
            {
                if (_collider)
                    _collider.enabled = false;
                StartCoroutine(DissapearCoroutine(() =>
                {
                    _art.gameObject.SetActive(false);
                    StartCoroutine(StayCoroutine(() =>
                    {
                        _art.gameObject.SetActive(true);
                        StartCoroutine(AppearCoroutine(() =>
                        {
                            if (_collider)
                                _collider.enabled = true;
                            ThisMaterialPropertyBlock.SetColor(COLOR_ID, _startColor);
                            _renderer.SetPropertyBlock(ThisMaterialPropertyBlock);
                        }, false));
                    }, false));
                }, false));
            }
            else
            {
                if (_collider)
                    _collider.enabled = true;
                _art.gameObject.SetActive(true);
                StartCoroutine(AppearCoroutine(() =>
                {
                    StartCoroutine(StayCoroutine(() =>
                    {
                        StartCoroutine(DissapearCoroutine(() =>
                        {
                            if (_collider)
                                _collider.enabled = false;
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
