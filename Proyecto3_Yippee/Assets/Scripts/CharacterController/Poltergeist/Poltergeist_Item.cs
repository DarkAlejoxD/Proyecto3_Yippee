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
            //TODO
        }

        public void EndPoltergeist()
        {
            //TODO
        }
    }
}