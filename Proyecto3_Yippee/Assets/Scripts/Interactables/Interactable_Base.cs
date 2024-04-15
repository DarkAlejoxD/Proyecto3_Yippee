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
       
        public virtual void Interact()
        {
            Debug.Log($"Interacted with {name}");
        }

        public virtual bool CanInteract()
        {
            return true;
        }

        public virtual void Select()
        {
            Debug.Log($"{name} is selected");
            _outlineMaterial.SetInt("_ShowOutline", 1);
        }

        public virtual void Unselect()
        {
            _outlineMaterial.SetInt("_ShowOutline", 0);
        }

        #endregion

        #region Private Methods


        #endregion
    }
}
