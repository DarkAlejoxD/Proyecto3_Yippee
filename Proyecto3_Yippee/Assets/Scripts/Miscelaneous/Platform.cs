using BaseGame;
using UnityEngine;

namespace Miscelaneous
{
    public class Platform : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            GameObject player = GameManager.Player?.gameObject;
            if (!player)
                return;
            if (other.gameObject == player)
            {
                player.transform.SetParent(transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            GameObject player = GameManager.Player?.gameObject;
            if (!player)
                return;
            if (other.gameObject == player)
            {
                player.transform.SetParent(null);
            }
        }
    }
}