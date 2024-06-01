using System;
using UnityEngine;
using static UtilsComplements.AsyncTimer;

namespace Poltergeist.Particles
{
    public class PS_PolterController : MonoBehaviour
    {
        public static Action OnActivate;
        public static Action OnDeactivate;
        public Transform _parent;

        private void Start()
        {
            _parent = transform;
            DeactivateParticles();
        }

        public void ActivateParticles()
        {
            OnActivate?.Invoke();
            transform.SetParent(_parent);
            transform.localScale = Vector3.one;
        }

        public void DeactivateParticles()
        {
            OnDeactivate?.Invoke();
            transform.SetParent(null, true);
            StartCoroutine(TimerCoroutine(5, () =>
            {
                transform.SetParent(_parent);
                transform.localPosition = Vector3.zero;
            }));
        }

    }
}
