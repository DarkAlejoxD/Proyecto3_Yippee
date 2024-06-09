using UnityEngine;
using UnityEngine.SceneManagement;

namespace BaseGame
{
    public class ChangeScene : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float _delayTime = 0.1f;
        [SerializeField, Range(0, 2)] private int _sceneIndex = 0;
        [SerializeField, TextArea(3, 4)]
        private string _scenes =
            "0: Menu" +
            "\n1: Game" +
            "\n2:Credits";

        public void LoadScene()
        {
            SceneManager.LoadSceneAsync(_sceneIndex);
        }
    }
}
