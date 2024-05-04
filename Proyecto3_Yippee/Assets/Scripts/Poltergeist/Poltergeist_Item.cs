using UnityEngine;
using UtilsComplements;

namespace Poltergeist
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Poltergeist_Item : MonoBehaviour
    {
        [Header("Start Attributes")]
        [SerializeField, Tooltip("It won't use gravity if is kinematic")] private bool _useGravity;
        [SerializeField] private bool _isKinematic;

        Rigidbody _rb;

        #region Unity Logic
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
        #endregion

        #region Public Methods
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

        public void Manipulate()
        {
            Debug.Log("Wow, outline chido");
        }

        public void NoManipulating()
        {
            Debug.Log("Desactiva, outline chido");
        }
        #endregion
    }
}