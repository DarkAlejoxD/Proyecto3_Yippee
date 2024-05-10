using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaseGame
{
    public class Deadzone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //DEBUG
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}