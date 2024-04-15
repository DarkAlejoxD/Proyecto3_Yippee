using System;
using UnityEngine;
using UtilsComplements;

namespace GhostView //add it to a concrete namespace
{
    public class GhostViewManager : MonoBehaviour, ISingleton<GhostViewManager>
    {
        #region Fields
        public ISingleton<GhostViewManager> Instance => this;

        public static Action OnActivateGhostView;
        #endregion

        #region Unity Logic
        private void Awake()
        {
            Instance.Instantiate();
        }

        private void OnDestroy()
        {
            Instance.RemoveInstance();
        }
        #endregion

        #region Static Methods
        public static void RequestGhostView()
        {
            if (!ISingleton<GhostViewManager>.TryGetInstance(out var manager))
                return;

            manager.ActivateGhostView();
        }
        #endregion

        #region Public Methods
        public void ActivateGhostView()
        {
            OnActivateGhostView?.Invoke();
        }
        #endregion

        #region Private Methods
        private void PrivateMethod()
        {
        }
        #endregion
    }
}
