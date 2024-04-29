using UnityEngine;
using UtilsComplements;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Poltergeist
{
    public class PoltergeistManager : MonoBehaviour, ISingleton<PoltergeistManager>
    {
        private List<Poltergeist_Item> _poltergeist = new();
        public ISingleton<PoltergeistManager> Instance => this;

        #region Unity Logic
        private void Awake()
        {
            Instance.Instantiate();
        }

        private void OnDestroy() => Instance.RemoveInstance();
        #endregion

        #region Public Methods
        public void AddPoltergeist(Poltergeist_Item item)
        {
            if (!_poltergeist.Contains(item))
                _poltergeist.Add(item);
            else
                Debug.LogWarning("You're trying to ad an existing polter Item in the Polter manager", item);
        }

        public void RemovePoltergeist(Poltergeist_Item item)
        {
            if (_poltergeist.Contains(item))
                _poltergeist.Remove(item);
        }
        #endregion

        #region Private Methods
        #endregion

        #region DEBUG
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //TODO: Draw beziers? jsjs
        }
#endif
        #endregion
    }
}