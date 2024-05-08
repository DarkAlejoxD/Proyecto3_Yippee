using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


namespace MenuManagement //add it to a concrete namespace
{    
    public class MenuController : MonoBehaviour
    {
        #region Fields


        #endregion    

        #region Unity Logic
        private void Awake()
        {                
        }

        private void Update()
        {        
        }
        #endregion

        #region Static Methods
        public static void StaticMethod()
        {
        }
        #endregion

        #region Public Methods
        
        public void ChangeScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        #endregion

        #region Private Methods
        private void PrivateMethod()
        {
        }
        #endregion
    }


}
