using UnityEngine;

namespace Miscelaneous
{
    public class Platform : MonoBehaviour
    {
        Transform _player;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            _player = other.transform;
            _player.SetParent(transform);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            _player.SetParent(null);
        }
    }
}