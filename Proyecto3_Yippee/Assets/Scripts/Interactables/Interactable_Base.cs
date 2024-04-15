using UnityEngine;

namespace Interactable //add it to a concrete namespace
{

    public class Interactable_Base : MonoBehaviour, IInteractable
    {
        #region Fields
        private Material _outlineMaterial;

        #endregion    

        #region Unity Logic
        private void Awake()
        {                
            _outlineMaterial = GetComponent<MeshRenderer>().materials[1];
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
            _outlineMaterial.SetInt("_ShowOutline", 1);
        }

        public void Unselect()
        {
            _outlineMaterial.SetInt("_ShowOutline", 0);
        }

        #endregion

        #region Private Methods


        #endregion
    }
}
