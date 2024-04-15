using UnityEngine;

namespace Interactable //add it to a concrete namespace
{

    public class Interactable_Base : MonoBehaviour, IInteractable
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


        #region Public Methods
       
        public void Interact()
        {
            Debug.Log($"Interacted with {name}");
        }

        public bool CanInteract()
        {
            return true;
        }

        public void Select()
        {
            Debug.Log($"{name} is selected");

        }

        public void Unselect()
        {
            
        }
        
        #endregion

        #region Private Methods

        
        #endregion
    }
}
