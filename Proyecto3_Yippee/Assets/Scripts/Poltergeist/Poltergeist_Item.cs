﻿using UnityEngine;
using UtilsComplements;

namespace Poltergeist
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Poltergeist_Item : MonoBehaviour
    {
        [Header("Start Attributes")]
        [SerializeField, Tooltip("It won't use gravity if is kinematic")] private bool _useGravity;
        [SerializeField] private bool _isKinematic;

        Rigidbody _rb;
        bool _freezePosition;
        Vector3 _position;

        #region Unity Logic
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            EndPoltergeist();
            _position = transform.position;
        }

        private void OnEnable()
        {
            if (Singleton.TryGetInstance(out PoltergeistManager manager))
            {
                manager.AddPoltergeist(this);
            }
        }

        private void OnDisable()
        {
            if (Singleton.TryGetInstance(out PoltergeistManager manager))
            {
                manager.RemovePoltergeist(this);
            }
        }

        private void Update()
        {
            if (_freezePosition)
            {
                transform.position = _position;
            }
        }
        #endregion

        #region Public Methods
        public void StartPoltergeist()
        {
            _rb.isKinematic = false;
            _rb.useGravity = false;
            _freezePosition = true;
            _position = transform.position;
        }

        public void EndPoltergeist()
        {
            _rb.isKinematic = _isKinematic;
            _rb.useGravity = _useGravity;
            _freezePosition = false;
        }

        public void Manipulate()
        {
            Debug.Log("Wow, outline chido");
            _freezePosition = false;
        }

        public void NoManipulating()
        {
            Debug.Log("Desactiva, outline chido");
            _position = transform.position;
            _freezePosition = true;
        }
        #endregion
    }
}