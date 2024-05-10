using UnityEngine;

namespace Mechanisms
{
    public class TriggerInterruptor : AbsInterruptor
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            base.Activate();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            base.Deactivate();
        }
    }
}
