using UnityEngine;
using UtilsComplements;

namespace Poltergeist
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody), typeof(Collider2D))]
    public class Poltergeist_Item : MonoBehaviour
    {
        [Header("Start Attributes")]
        [SerializeField, Tooltip("It won't use gravity if is kinematic")] private bool _useGravity;
        [SerializeField] private bool _isKinematic;

        Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            EndPoltergeist();
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

        public void StartPoltergeist()
        {
            _rb.isKinematic = false;
            _rb.useGravity = false;
        }

        public void EndPoltergeist()
        {
            _rb.isKinematic = _isKinematic;
            _rb.useGravity = _useGravity;
        }
    }
}