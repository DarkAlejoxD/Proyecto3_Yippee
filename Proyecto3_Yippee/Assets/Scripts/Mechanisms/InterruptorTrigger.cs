using UnityEngine;

namespace Mechanism
{
    [RequireComponent(typeof(Collider))]
    public class InterruptorTrigger : AbsInterruptor
    {
        protected override InterruptorStyle InterStyle => InterruptorStyle.Once;

        private void Start() => GetComponent<Collider>().isTrigger = true;

        private void OnTriggerEnter(Collider other)
        {
            if (Activated)
                return;
            if (other.CompareTag("Player"))
                base.KeyPressed();
        }
    }
}
